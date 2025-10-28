using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public List<ItemConfig> Items;
        
    }
    #if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ItemDatabase))]
    public class ItemDatabaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if(GUILayout.Button("Create Code"))
            {
                
                var itemDb = target as ItemDatabase;
                var filePath = AssetDatabase.GetAssetPath(target).GetFolderPath() + "/Items.cs";

                var rootCode = new RootCode()
                    .Using("UnityEngine")
                    .Using("QFramework")
                    .EmptyLine()
                    .Namespace("QFramework.Example",ns=>
                    {
                        ns.Class("Items", string.Empty, false, false, c =>
                        {
                            foreach (var itemConfig in itemDb.Items)
                            {
                                c.Custom($"public static string {itemConfig.name} = \"{itemConfig.Key}\";");
                            
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
        }
    }
    
    #endif
    
}