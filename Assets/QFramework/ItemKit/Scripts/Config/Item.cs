#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;


namespace QFramework
{
    [CreateAssetMenu(menuName = "@ItemKit/Creat Item")]
    public class Item : ScriptableObject,IItem
    {
        [DisplayLabel("名称")]
        public string Name = string.Empty;
        [DisplayLabel("关键字")]
        public string Key = string.Empty;
        public Sprite Icon;
        public string GetName => Name;
        public string GetKey => Key;
        public Sprite GetIcon => Icon;
    }

    
    #if UNITY_EDITOR
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor
    {
        private SerializedProperty mIcon;
        private void OnEnable()
        {
            mIcon = serializedObject.FindProperty("Icon");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            serializedObject.DrawProperties(false,0,"Icon");
                    
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("图标");
            mIcon.objectReferenceValue = EditorGUILayout.ObjectField(mIcon.objectReferenceValue, 
                typeof(Sprite),true,GUILayout.Height(48),GUILayout.Width(48));
                    
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
    #endif
}

