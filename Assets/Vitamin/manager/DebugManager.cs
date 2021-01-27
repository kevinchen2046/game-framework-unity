
using vitamin;
using UnityEngine;
using System;
using System.Collections.Generic;

public class DebugManager : MonoBehaviour {
    GameObject debugPanelObject;
    DebugPannel debugPanel;
    internal DebugCmdBase[] cmds;
    KeyCode __debugHotKey;
    public void initialize(bool fpsEnabled,bool memoryEnabled, bool deviceEnabled,KeyCode debugHotKey)
    {
        __debugHotKey = debugHotKey;
        if (fpsEnabled || deviceEnabled)
        {
            GameObject gameobject = new GameObject("fps");
            CodeStage.AdvancedFPSCounter.AFPSCounter comp=gameobject.AddComponent<CodeStage.AdvancedFPSCounter.AFPSCounter>();
            comp.fpsCounter.Enabled = fpsEnabled;
            comp.memoryCounter.Enabled = memoryEnabled;
            comp.deviceInfoCounter.Enabled = deviceEnabled;
            comp.AutoScale = true;
        }
        GameObject Prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Vitamin/prefabs/DebugPanel.prefab");
        debugPanelObject = GameObject.Instantiate(Prefab);
        debugPanel = debugPanelObject.GetComponent<DebugPannel>();
        debugPanelObject.transform.SetParent(GameObject.FindObjectOfType<Vitamin>().transform);
        debugPanelObject.SetActive(false);
        
        //初始化 CMD
        List<Type> types=Injector.getChildrenTypes(typeof(DebugCmdBase));
        cmds = new DebugCmdBase[types.Count];
        int i = 0;
        foreach (var type in types){
            DebugCmdBase cmd =Activator.CreateInstance(type) as DebugCmdBase;
            cmds[i++] = cmd;
        }

        //关联输入输出
        debugPanel.onInput((string input) =>
        {
            string[] args=input.Split(',');
            List<string> list = new List<string>(args);
            string cmdname = list[0];
            list.RemoveAt(0);
            foreach (var cmd in cmds)
            {
                if (cmd.name == cmdname)
                {
                    cmd.exec(list.ToArray(),(string output)=> {
                        debugPanel.addToQueue(output);
                    });
                    break;
                }
            }
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(__debugHotKey))
        {
            if (debugPanelObject.activeSelf) debugPanelObject.SetActive(false);
            else debugPanelObject.SetActive(true);
        }
    }
}