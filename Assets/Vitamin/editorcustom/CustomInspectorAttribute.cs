using UnityEditor;
using UnityEngine;

namespace vitamin
{
    /// <summary>
    /// 使字段在Inspector中显示自定义的名称。
    /// </summary>
    public class CustomInspectorAttribute : PropertyAttribute
    {
        public string name;

        /// <summary>
        /// 使字段在Inspector中显示自定义的名称。
        /// </summary>
        /// <param name="name">自定义名称</param>
        public CustomInspectorAttribute(string name)
        {
            this.name = name;
        }
    }

    /// <summary>
    /// 定义对带有 `CustomLabelAttribute` 特性的字段的面板内容的绘制行为。
    /// </summary>
    [CustomPropertyDrawer(typeof(CustomInspectorAttribute))]
    public class CustomInspectorAttributeDrawer : PropertyDrawer
    {
        private GUIContent _label = null;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_label == null)
            {
                string name = (attribute as CustomInspectorAttribute).name;
                _label = new GUIContent(name);
            }
            EditorGUI.PropertyField(position, property, _label);
        }
    }
}


