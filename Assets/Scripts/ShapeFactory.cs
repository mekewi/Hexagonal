using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridShape {
    HexagonShape,
    RectangleShape
}
public interface IShape {
    int mapColumns { get; set; }
    int mapRows { get; set; }
    List<Coordinates> DrawShape();
}
public class HexagonShape : IShape
{
    public int mapColumns { get; set; }
    public int mapRows { get; set; }
    public HexagonShape(int currentMapColumns,int currentMpRows) {
        mapColumns = currentMapColumns;
        mapRows = currentMpRows;
    }
    public List<Coordinates> DrawShape()
    {
        List<Coordinates> listOfCoordinates = new List<Coordinates>();
        for (int q = -mapRows; q <= mapRows; q++)
        {
            int r1 = Mathf.Max(-mapRows, -q - mapRows);
            int r2 = Mathf.Min(mapRows, -q + mapRows);
            for (int r = r1; r <= r2; r++)
            {
                listOfCoordinates.Add(new Coordinates(q, r, -q - r));
            }
        }
        return listOfCoordinates;
    }
}
public class RectangleShape : IShape
{
    public int mapColumns { get; set; }
    public int mapRows { get; set; }

    public RectangleShape(int currentMapColumns, int currentMpRows)
    {
        mapColumns = currentMapColumns;
        mapRows = currentMpRows;
    }
    public List<Coordinates> DrawShape()
    {
        List<Coordinates> listOfCoordinates = new List<Coordinates>();
        for (int q = 0; q < mapColumns; q++)
        {
            int q_offset = (int)Mathf.Floor(q / 2);
            for (int r = -q_offset; r < mapRows - q_offset; r++)
            {
                listOfCoordinates.Add(new Coordinates(q, r, -q - r));
            }
        }

        return listOfCoordinates;
    }
}
public class ShapeFactory
{
    public LevelSettings levelSettings;
    public ShapeFactory(LevelSettings gameLevelSettings) {
        levelSettings = gameLevelSettings;
    }
    public IShape GetShape() 
    {
        switch (levelSettings.gridShape)
        {
            case GridShape.HexagonShape:
                return new HexagonShape(levelSettings.MapColumns, levelSettings.MapRows);
            case GridShape.RectangleShape:
                return new RectangleShape(levelSettings.MapColumns, levelSettings.MapRows);
            default:
                return null;
        }
    }
}
