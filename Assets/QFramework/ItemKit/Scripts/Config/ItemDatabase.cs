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
        [DisplayLabel("命名空间:")]
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
            
            RefreshItemEditors();
        }

        private void RefreshItemEditors()
        {
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

        private string mSearchKey = "";
        private FluentGUIStyle mHeader = FluentGUIStyle.Label().FontBold();

        private Queue<Action> mActionQueue = new Queue<Action>();
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.BeginVertical("box");
            serializedObject.DrawProperties(false,0,"Items");
            
            if (mItems.arraySize != mItemEditors.Count)
            {
                RefreshItemEditors();
            }
            
            if(GUILayout.Button("生成代码"))
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
            GUILayout.EndVertical();
           
            EditorGUILayout.Separator();
            GUILayout.Label("物品列表:",mHeader);
            GUILayout.BeginHorizontal();
            GUILayout.Label("搜索:",GUILayout.Width(40));
            mSearchKey = EditorGUILayout.TextField(mSearchKey);
            GUILayout.EndHorizontal();
            
            
            for (var i = 0; i < mItemEditors.Count; i++)
            {
                var itemEditor = mItemEditors[i];
                if (!itemEditor.Item.Name.Contains(mSearchKey) && !itemEditor.Item.Key.Contains(mSearchKey))
                {
                    continue;
                }
                
                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();

                itemEditor.Foldout = EditorGUILayout.Foldout(itemEditor.Foldout, itemEditor.Item.GetName);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("-"))
                {
                    var index = i;
                    if (EditorUtility.DisplayDialog("删除物品", "确定要删除吗？", "确定", "取消"))
                    {
                        mActionQueue.Enqueue(() =>
                        {
                            var arrayElement = mItems.GetArrayElementAtIndex(index);
                            AssetDatabase.RemoveObjectFromAsset(arrayElement.objectReferenceValue);
                            mItems.DeleteArrayElementAtIndex(index);
                            serializedObject.ApplyModifiedPropertiesWithoutUndo();
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        });
                    }
                    
                    
                }
                GUILayout.EndHorizontal();
                if (itemEditor.Foldout)
                { 
                    itemEditor.Editor.OnInspectorGUI();
                }
                
                GUILayout.EndVertical();
                
            }
            
            if (GUILayout.Button("创建物品"))
            {
                mActionQueue.Enqueue(() =>
                {
                    var item = CreateInstance<Item>();
                    item.name = nameof(Item);
                    item.Name = "新物品";
                    item.Key = "item_key";
                    AssetDatabase.AddObjectToAsset(item,target);
                    mItems.InsertArrayElementAtIndex(mItems.arraySize);
                    var arrayElement = mItems.GetArrayElementAtIndex(mItems.arraySize - 1);
                    arrayElement.objectReferenceValue = item;
                    serializedObject.ApplyModifiedPropertiesWithoutUndo();
                
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                });
            }

            if (mActionQueue.Count > 0)
            {
                mActionQueue.Dequeue().Invoke();
            }
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
    
    #endif
    
}