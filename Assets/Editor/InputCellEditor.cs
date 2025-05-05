using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class InputCellEditor : Editor
{
    private static string inputCellPath = "Assets/Prefab/CustomInputCell.prefab";

    [MenuItem("GameObject/MyTools/InputCell",priority = 0)]
    public static void CreateInputCell()
    {
        GameObject inputCell = AssetDatabase.LoadAssetAtPath(inputCellPath, typeof(GameObject)) as GameObject;
        if (inputCell != null)
        {
            Instantiate(inputCell,Selection.activeGameObject.transform).name = "InputCell";
        }
    }
}
