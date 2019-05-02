using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelSettings : ScriptableObject
{
    public List<Color> HexCellColors;
    public int ScoreToReachExplodeHex;
    public int ExplodeTimer;
    public int SpacesBetweenHexas;
    public bool showCoordinates;
    public OrientationType orientationType;
    public GridShape gridShape;
    public int MapColumns;
    public int MapRows;
    public void LoadDefault()
    { 
    
    }
}
