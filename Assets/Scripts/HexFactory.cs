using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface IExplode
{
    bool ISExploded { get; set; }
    TextMeshPro TimeText { get; set; }
    void HexMoved();
    void TimeFinishedShouldExplode();
    void Destoryed();
    void UpdateText();
}
public class CanExplode : IExplode
{
    public int CurrentTime { get; private set; }
    public TextMeshPro TimeText { get; set; }
    public bool ISExploded { get; set; }

    public GameEvent_SO HexExploded;
    public CanExplode(int explodTime,GameEvent_SO HexExplodedEvent) {
        HexExploded = HexExplodedEvent;
        CurrentTime = explodTime;
        ISExploded = false;
    }
    public void HexMoved()
    {
        CurrentTime--;
        UpdateText();
        if (CurrentTime <= 0)
        {
            TimeFinishedShouldExplode();
            ISExploded = true;
        }
    }

    public void TimeFinishedShouldExplode()
    {
        HexExploded.Rais();
    }

    public void Destoryed()
    {
        TimeText.text = "";
    }

    public void UpdateText()
    {
        TimeText.text = CurrentTime.ToString();
    }
}
public class CanNotExplode : IExplode
{
    public TextMeshPro TimeText { get; set; }
    public bool ISExploded { get ; set ; }
    public void UpdateText() {
        TimeText.text = "";
    }
    public void Destoryed()
    {
        ISExploded = false;
    }
    public void HexMoved()
    {
    }

    public void TimeFinishedShouldExplode()
    {
    }
}

public class HexFactory : MonoBehaviour
{
    [SerializeField]
    GameEvent_SO HexExplodedEvent;
    [SerializeField]
    GameEvent_SO HexMovedEvent;
    [SerializeField]
    LevelSettings levelSetting;
    [SerializeField]
    LevelData levelData;
    [SerializeField]
    HexCell hexCellObject;

    public HexCell InstantiateHex(Coordinates hexCoordinate,ref int countExplodeCellInScene)
    {
        Point hexPoint = Layout.Instance.HexToPixel(hexCoordinate);
        HexCell hexCell = Instantiate(hexCellObject, new Vector2((float)hexPoint.x, (float)hexPoint.y), Quaternion.identity, transform);
        RefactorHex(hexCoordinate,hexCell, ref countExplodeCellInScene,true);
        return hexCell;
    }
    private IExplode GetHexExplodeType(ref int countExplodeCellInScene)
    {
        IExplode explodeObject;
        if (levelSetting.ScoreToReachExplodeHex < levelData.LevelScore &&
           ((levelData.LevelScore/levelSetting.ScoreToReachExplodeHex) - countExplodeCellInScene) > 1)
        {
            countExplodeCellInScene++;
            explodeObject = new CanExplode(levelSetting.ExplodeTimer, HexExplodedEvent);
        }
        else
        {
            explodeObject = new CanNotExplode();
        }
        return explodeObject;
    }
    public void RefactorHex(Coordinates hexCoordinate, HexCell hexCellToRefactor, ref int countExplodeCellInScene, bool changeColor)
    {
        Color hexColor = changeColor ? PickColor() : hexCellToRefactor.HexColor;
        hexCellToRefactor.SetHexData(hexColor, hexCoordinate);  if (hexCellToRefactor.explode == null || hexCellToRefactor.explode.ISExploded )
        {
            hexCellToRefactor.explode = GetHexExplodeType(ref countExplodeCellInScene);
        }
        hexCellToRefactor.explode.TimeText = hexCellToRefactor.TimerText;
        hexCellToRefactor.BringHexToLife();
        hexCellToRefactor.hexCorrdinates = hexCoordinate;
    }
    private Color PickColor()
    {
        return levelSetting.HexCellColors[Random.Range(0, levelSetting.HexCellColors.Count)];
    }
}