using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "ScriptableObjects/GameConfiguration")]
public class GameConfiguration : ScriptableObject
{
    public BoardConfiguration boardConfig;
    public bool loadGame;
}