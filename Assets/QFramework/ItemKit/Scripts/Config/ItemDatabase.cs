using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using QFramework.Example;
#if UNITY_EDITOR
#endif
using UnityEditor;
using UnityEngine;

namespace QFramework
{
    [CreateAssetMenu(menuName = "@ItemKit/Creat Item Database")]
    public class ItemDatabase : ScriptableObject
    {
        public string NameSpace = "QFramework.Example";
        public List<Item> Items;
        
    }
    #if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ItemDatabase))]
    public class ItemDatabaseEditor : UnityEditor.Editor
    {
        private SerializedProperty mItems;
        
        public class ItemEditorObj
        {
            public bool Foldout = false;
            public Editor Editor = null;
            public Item Item = null;
        }

        private List<ItemEditorObj> mItemEditors = new List<ItemEditorObj>();
        private void OnEnable()
        {
            mItems = serializedObject.FindProperty("Items");
            
            mItemEditors.Clear();

            for (int i = 0; i < mItems.arraySize; i++)
            {
                var itemSo = mItems.GetArrayElementAtIndex(i);
                var editor = CreateEditor(itemSo.objectReferenceValue);
                mItemEditors.Add(new ItemEditorObj()
                {
                    Editor = editor,
                    Item =itemSo.objectReferenceValue as Item,
                });
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            serializedObject.DrawProperties(true,0,"Items");


            if (GUILayout.Button("+"))
            {
                var itemConfig = CreateInstance<Item>();
                AssetDatabase.AddObjectToAsset(itemConfig,target);
                mItems.InsertArrayElementAtIndex(mItems.arraySize);
                var arrayElement = mItems.GetArrayElementAtIndex(mItems.arraySize - 1);
                arrayElement.objectReferenceValue = itemConfig;
                
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                OnEnable();
            }

            serializedObject.ApplyModifiedProperties();
            
            for (var i = 0; i < mItemEditors.Count; i++)
            {
                var itemEditor = mItemEditors[i];
                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();

                itemEditor.Foldout = EditorGUILayout.Foldout(itemEditor.Foldout, itemEditor.Item.GetName);
                var itemSo = new SerializedObject(itemEditor.Item);
                itemSo.Update();
                
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("-"))
                {
                    if (EditorUtility.DisplayDialog("删除物品", "确定要删除吗？", "确定", "取消"))
                    {
                        var arrayElement = mItems.GetArrayElementAtIndex(i);
                        AssetDatabase.RemoveObjectFromAsset(arrayElement.objectReferenceValue);
                        mItems.DeleteArrayElementAtIndex(i);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        
                        OnEnable();
                    }
                    
                    
                }
                GUILayout.EndHorizontal();
                if (itemEditor.Foldout)
                { 
                    itemEditor.Editor.OnInspectorGUI();
                }

                itemSo.ApplyModifiedPropertiesWithoutUndo();
                GUILayout.EndVertical();
                
            }
            
            if(GUILayout.Button("Create Code"))
            {
                
                var itemDb = target as ItemDatabase;
                var filePath = AssetDatabase.GetAssetPath(target).GetFolderPath() + "/Items.cs";

                var rootCode = new RootCode()
                    .Using("UnityEngine")
                    .Using("QFramework")
                    .EmptyLine()
                    .Namespace(itemDb.NameSpace,ns=>
                    {
                        ns.Class("Items", string.Empty, false, false, c =>
                        {
                            foreach (var itemConfig in itemDb.Items)
                            {
                                c.Custom($"public static string {itemConfig.Key} = \"{itemConfig.Key}\";");
                            
                                Debug.Log(itemConfig.Key);
                            }
                        });
                    });
                using var fileWriter = File.CreateText(filePath);
                var codeWriter = new FileCodeWriter(fileWriter);
                rootCode.Gen(codeWriter);
            
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
    
    #endif
    
}