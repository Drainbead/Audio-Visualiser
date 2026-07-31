using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SavedShapes : MonoBehaviour
{
    public ShapeData shapeAsset;


    public void SaveShape(string name, Vector3[] positions)
    {
        if (shapeAsset == null)
        {
            Debug.LogError("No ShapeData asset assigned.");
            return;
        }


        if (positions == null || positions.Length == 0)
        {
            Debug.LogError("Cannot save empty shape.");
            return;
        }


        shapeAsset.shapeName = name;
        shapeAsset.positions = positions;
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(shapeAsset);
#endif

        Debug.Log(
            "Saved " + name +
            " with " + positions.Length + " positions"
        );

#if UNITY_EDITOR
        EditorUtility.SetDirty(shapeAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif


        Debug.Log(
            "Saved " + name +
            " with " + positions.Length + " positions"
        );
    }
}