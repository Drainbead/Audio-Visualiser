using UnityEngine;

[CreateAssetMenu(
    fileName = "New Shape",
    menuName = "Audio Visualiser/Shape Data"
)]
public class ShapeData : ScriptableObject
{
    public string shapeName;

    public Vector3[] positions;
}