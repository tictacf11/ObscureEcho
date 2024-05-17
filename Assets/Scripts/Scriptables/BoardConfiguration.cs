using UnityEngine;

[CreateAssetMenu(fileName = "BoardConfiguration", menuName = "ScriptableObjects/BoardConfiguration")]
public class BoardConfiguration : ScriptableObject
{
    public int rows;
    public int columns;
}
