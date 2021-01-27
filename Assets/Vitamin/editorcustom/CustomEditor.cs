using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace vitamin
{
    public class CustomEditor
    {

        [MenuItem("💊维他命💊/生成Entry", true)]
        public static bool ___VitaminCreateEntryEnable()
        {
            return !(new DirectoryInfo(Application.dataPath + "/src").Exists);
        }
        [MenuItem("💊维他命💊/生成Entry", false)]
        public static void ___VitaminCreateEntry()
        {
            __createFiles();
            __createSceneObjects();
        }

        private static void __createSceneObjects()
        {
            Type VitaminType = Type.GetType("vitamin.Vitamin");
            Type UIType = Type.GetType("game.UI");
            Type GameType = Type.GetType("game.Game");
            Type UIContentScalarType = Type.GetType("FairyGUI.UIContentScaler");

            if (VitaminType == null) { Debug.LogError("vitamin.Vitamin 定义未找到!"); return; }
            if (UIType == null) { Debug.LogError("game.UI 定义未找到!"); return; }
            if (GameType == null) { Debug.LogError("game.Game 定义未找到!"); return; }
            if (UIContentScalarType == null) { Debug.LogError("FairyGUI.UIContentScaler 定义未找到!"); return; }

            if (UnityEditor.SceneAsset.FindObjectOfType(VitaminType) != null)
            {
                Debug.LogError("vitamin.Vitamin 已存在!");
                return;
            }
            var VitaminObject = new GameObject("VitaminObject");
            Component vitaminComponent = VitaminObject.AddComponent(VitaminType);

            var UIObject = new GameObject("UI");
            Component uiComponent = UIObject.AddComponent(UIType);
            UIObject.transform.parent = VitaminObject.transform;

            var GameEntryObject = new GameObject("Game");
            Component gameComponent = GameEntryObject.AddComponent(GameType);
            GameEntryObject.transform.parent = VitaminObject.transform;

            var UIContentScalarObject = new GameObject("Scalar");
            Component scalerComponent = UIContentScalarObject.AddComponent(UIContentScalarType) as FairyGUI.UIContentScaler;
            UIContentScalarObject.transform.parent = VitaminObject.transform;


            SetPropertyValue(scalerComponent, "scaleMode", FairyGUI.UIContentScaler.ScaleMode.ScaleWithScreenSize);
            SetPropertyValue(scalerComponent, "designResolutionX", 640);
            SetPropertyValue(scalerComponent, "designResolutionY", 1136);
            SetPropertyValue(scalerComponent, "screenMatchMode", FairyGUI.UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);

            SetPropertyValue(vitaminComponent, "uiContext", uiComponent);
            SetPropertyValue(vitaminComponent, "gameContext", gameComponent);
            SetPropertyValue(vitaminComponent, "uiScaler", scalerComponent);

            Debug.LogFormat("Vitamin 框架创建成功!");
        }

        private static void __createFiles()
        {
            DirectoryInfo srcFolder = new DirectoryInfo(Application.dataPath + "/src");
            Debug.Log(srcFolder.Exists);
            if (srcFolder.Exists == true)
            {
                Debug.LogError("Assets/src文件路径已存在...");
                return;
            }
            srcFolder.Create();
            FileInfo sampleFile = new FileInfo(Application.dataPath + "/Vitamin/context/sample.txt");
            using (FileStream stream = sampleFile.OpenRead())
            {
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                stream.Close();
                string sample = System.Text.Encoding.UTF8.GetString(data);
                string[] contents = System.Text.RegularExpressions.Regex.Split(sample, "\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                List<string> lines = new List<string>();
                
                for (var i = 0; i < contents.Length; i++)
                {
                    if (contents[i].IndexOf("####") >= 0 || i == contents.Length - 1)
                    {
                        if (lines.Count > 0)
                        {
                            var title = lines[0];
                            var name = title.Substring(4, title.Length - 4);
                            lines.RemoveAt(0);
                            var content= string.Join("\n", lines);
                            __createFile(name, content, srcFolder);

                            //Debug.Log(name);
                            //Debug.Log(files[name]);
                        }
                        lines.Clear();
                    }
                    lines.Add(contents[i]);
                }
            }
        }

        private static void __createFile(string name,string content, DirectoryInfo rootFolder)
        {
            if (name.IndexOf("/") >= 0)
            {
                var path = name.Substring(0, name.IndexOf("/"));
                name = name.Substring(name.IndexOf("/") + 1);
                DirectoryInfo folderInfo = new DirectoryInfo(rootFolder.FullName + "/" + path);
                Debug.LogWarning(folderInfo.FullName);
                if (!folderInfo.Exists) folderInfo.Create();

                if (name.IndexOf("/") >= 0)
                {
                    __createFile(name, content, folderInfo);
                    return;
                }
                rootFolder = folderInfo;
            }
            FileInfo fileInfo = new FileInfo(rootFolder.FullName + "/" + name);
            using (FileStream filestream = fileInfo.OpenWrite())
            {
                byte[] filedata = System.Text.Encoding.UTF8.GetBytes(content);
                filestream.Write(filedata, 0, filedata.Length);
                filestream.Close();
                alert(String.Format("写入文件{0}完成!",name));
            }
        }

        [MenuItem("💊维他命💊/打包Entry", true)]
        public static bool ___VitaminEntryPackEnable()
        {
            return UnityEditor.CloudProjectSettings.userName == "kevin-chen@foxmail.com";
        }
        [MenuItem("💊维他命💊/打包Entry", false)]
        public static void ___VitaminEntryPack()
        {
            
            if (UnityEditor.CloudProjectSettings.userName != "kevin-chen@foxmail.com")
            {
                alert("你无权进行此操作!");
                return;
            }
            string[] filePaths = { "UI.cs", "Game.cs", "command/CmdRename.cs", "model/ModelUser.cs", "view/MainView.cs" };
            DirectoryInfo srcFolder = new DirectoryInfo(Application.dataPath + "/Scenes");
            if (srcFolder.Exists)
            {
                string sample = "";
                foreach(string value in filePaths)
                {
                    FileInfo uiFile = new FileInfo(srcFolder.FullName +"/"+ value);
                    string content = uiFile.OpenText().ReadToEnd();
                    sample += "####"+ value+"\n"+content + "\n";
                }
                //Debug.Log(sample);
                FileInfo sampleFile= new FileInfo(Application.dataPath+ "/Vitamin/context/sample.txt");
                FileStream stream;
                using (stream = sampleFile.OpenWrite())
                {

                    stream.Seek(0, SeekOrigin.Begin);
                    stream.SetLength(0);
                    stream.Close();
                }
                using (stream = sampleFile.OpenWrite())
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(sample);
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                    alert("打包Sample完成!");
                }
            }
        }

        /// ////////////////////////////////////////////////////////////////////

        [MenuItem("Assets/💊维他命💊/测试", true)]
        public static bool ___Menu1HandlerEnable()
        {
            foreach (var guid in Selection.assetGUIDs)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Debug.Log(assetPath);
                if (assetPath.EndsWith(".unity"))
                {
                    return true;
                }
            }
            return false;
        }


        [MenuItem("Assets/💊维他命💊/测试", false, 10)]
        public static void ___AssetsMenu1()
        {
            __menu1Handler();
        }

        private static void __menu1Handler()
        {
            Debug.Log("Menu1...");
        }

        private static void SetPropertyValue(object obj, string prop, object value)
        {
            foreach (FieldInfo pi in obj.GetType().GetFields())
            {
                if (pi.Name == prop)
                {
                    pi.SetValue(obj, value);
                    break;
                }
            }
        }

        private static bool alert(string content,bool showcancel=false){
            return EditorUtility.DisplayDialog("提示",content,"确定",showcancel?"取消":null);
        }
    }
}
