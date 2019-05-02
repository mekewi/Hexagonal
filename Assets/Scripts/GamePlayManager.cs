using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField]
    LevelData levelData;
    [SerializeField]
    GameEvent_SO ScoreIsChanged;
    public void HexagonsAreMatchingColor(int numberOfHexagon) {
        levelData.LevelScore += (numberOfHexagon * 5);
        ScoreIsChanged.Rais();
    }
    public void Start()
    {
        levelData.ResetDefault();
    }
    public void RestartGame() {
        levelData.ResetDefault();
    }
}
