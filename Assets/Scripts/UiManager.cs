using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    LevelData levelData;
    [SerializeField]
    Text scoreText;
    [SerializeField]
    GameEvent_SO playAgainEvent;
    [SerializeField]
    LevelSettings levelSettings;

    public void ChangeScore() {
        scoreText.text = levelData.LevelScore.ToString();
    }
    public void GameOver()
    {
        scoreText.text = "GameOver";
    }
    public void PlayAgain() {
        playAgainEvent.Rais();
    }
    public void ChangeGridShape(int gridShape) {
        levelSettings.gridShape = (GridShape)gridShape;
        PlayAgain();
    }
}
