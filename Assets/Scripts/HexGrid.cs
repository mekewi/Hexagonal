using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [SerializeField]
    public HexCell HexPrefab;
    public int mapRows;
    public int mapColumns;
    public float spaces;
    public LevelSettings levelSetting;
    public HexFactory hexFactory;
    public LineRenderer line;
    public bool startRotate = false;
    public Vector3 rotateArround = new Vector3(0,0,0);
    public List<Coordinates> listOfSelectedHex = new List<Coordinates>();
    public Transform SelectedCenter;
    [SerializeField]
    public Dictionary<Coordinates,HexCell> HexCoordinates = new Dictionary<Coordinates, HexCell>();
    [SerializeField]
    IntegerEvent_SO HexagonsMatchesColor;
    [SerializeField]
    GameEvent_SO hexMovedEvent;
    ShapeFactory shapeFactory;
    IShape currentGridShape;
    public int checkCount = 1;
    private TouchRotateDirction swipeDirction;
    int countOfExplodHexInScene =0;
    private void Start()
    {
        Layout layout = new Layout(levelSetting.orientationType, new Point(1 + spaces, 1 + spaces), new Point(0, 0));
        shapeFactory = new ShapeFactory(levelSetting);
        GenerateGrid();
        _camera = Camera.main;
        UpdateCamera();
    }
    public float sceneWidth = 10;

    Camera _camera;
    public float widthRatio;
    public float HeightRatio;
    public float size;

    void UpdateCamera()
    {
        if (levelSetting.gridShape == GridShape.HexagonShape)
        {
            size = 12.5f;
        }
        else {
            size = 6.5f;
        }
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio =  (widthRatio * size) / (HeightRatio * size);

        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = (widthRatio * size) / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = (widthRatio * size) / 2 * differenceInSize;
        }
    }

    // Start is called before the first frame update
    void GenerateGrid()
    {
        currentGridShape = shapeFactory.GetShape();
        List<Coordinates> gridCoordinates = currentGridShape.DrawShape();
        float difPositionOfFirstElementX = Mathf.Abs( (float)Layout.Instance.HexToPixel(gridCoordinates[gridCoordinates.Count - 1]).x)-Mathf.Abs((float)Layout.Instance.HexToPixel(gridCoordinates[0]).x);
        float difPositionOfFirstElementY = Mathf.Abs((float)Layout.Instance.HexToPixel(gridCoordinates[gridCoordinates.Count - 1]).y) - Mathf.Abs((float)Layout.Instance.HexToPixel(gridCoordinates[0]).y);

        Camera.main.transform.position = new Vector3(difPositionOfFirstElementX / 2, difPositionOfFirstElementY/2, Camera.main.transform.position.z);

        ClearHexasDictionary(gridCoordinates);
        for (int i = 0; i < gridCoordinates.Count; i++)
        {
            instantiateCell(gridCoordinates[i]);
        }
    }
    void ClearHexasDictionary(List<Coordinates> gridCoordinates) {
        List<Coordinates> keys = HexCoordinates.Keys.ToList();
        foreach (var oldKey in keys)
        {
            if (!gridCoordinates.Contains(oldKey))
            {
                Destroy(HexCoordinates[oldKey].gameObject);
                HexCoordinates.Remove(oldKey);
            }
        }
    }
    public void instantiateCell(Coordinates hexCor) {
        if (HexCoordinates.ContainsKey(hexCor))
        {
            hexFactory.RefactorHex(hexCor, HexCoordinates[hexCor], ref countOfExplodHexInScene, true);
        }
        else
        {
            HexCell hexCell = hexFactory.InstantiateHex(hexCor, ref countOfExplodHexInScene);
            HexCoordinates.Add(hexCor, hexCell);
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
        }
        UpdateCamera();
    }
    public double cross(Point O, Point A, Point B)
    {
        return (A.x - O.x) * (B.y - O.y) - (A.y - O.y) * (B.x - O.x);
    }

    public List<Point> GetConvexHull(List<Point> points)
    {
        if (points == null)
            return null;

        if (points.Count <= 1)
            return points;

        int n = points.Count, k = 0;
        List<Point> H = new List<Point>(new Point[2 * n]);

        points.Sort((a, b) =>
        {
            bool v = System.Math.Abs(a.x - b.x) < 0;
            return v ? a.y.CompareTo(b.y) : a.x.CompareTo(b.x);
        });

        rotateArround.x = (float)points[8].x;
        rotateArround.y = (float)points[7].y;
        SelectedCenter.position = rotateArround;
        // Build lower hull
        for (int i = 0; i < n; ++i)
        {
            while (k >= 2 && cross(H[k - 2], H[k - 1], points[i]) <= 0)
                k--;
            H[k++] = points[i];
        }

        // Build upper hull
        for (int i = n - 2, t = k + 1; i >= 0; i--)
        {
            while (k >= t && cross(H[k - 2], H[k - 1], points[i]) <= 0)
                k--;
            H[k++] = points[i];
        }

        return H.Take(k - 1).ToList();
    }
    float totalDegree;

    void FixedUpdate()
    {
        if (startRotate)
        {
            bool ReachedAngle = false;
            for (int i = 0; i < listOfSelectedHex.Count; i++)
            {
                float degree = 120 * Time.deltaTime;
                totalDegree += degree;
                HexCoordinates[listOfSelectedHex[i]].transform.RotateAround(rotateArround, Vector3.forward, degree * (int)swipeDirction);
                ReachedAngle = totalDegree/ listOfSelectedHex.Count >= 120;
            }
            if (ReachedAngle)
            {
                startRotate = false;
                CheckSuccess();
            }
        }
    }
    public void CheckSuccess() {
        ShiftList((int)swipeDirction);
        List<Coordinates> NeighboorsWithSameColor = new List<Coordinates>();
        for (int i = 0; i < listOfSelectedHex.Count; i++)
        {
            for (int s = 0; s < 6; s++)
            {
                int nextNeighboorIndex = s + 1 == 6? 0 : s+1;
                Coordinates MainCoordinates = listOfSelectedHex[i];
                Coordinates Neighboor = HexCalculations.Neighbor(s, MainCoordinates);
                Coordinates NextNeighboor = HexCalculations.Neighbor(nextNeighboorIndex, MainCoordinates);
                bool isSameColor = 
                HexCoordinates.ContainsKey(Neighboor) &&
                HexCoordinates.ContainsKey(NextNeighboor)&&
                HexCoordinates[Neighboor].HexColor == HexCoordinates[MainCoordinates].HexColor &&
                HexCoordinates[NextNeighboor].HexColor == HexCoordinates[MainCoordinates].HexColor;
                if (isSameColor)
                {
                    if (!NeighboorsWithSameColor.Contains(MainCoordinates)) NeighboorsWithSameColor.Add(MainCoordinates);
                    if (!NeighboorsWithSameColor.Contains(Neighboor)) NeighboorsWithSameColor.Add(Neighboor);
                    if (!NeighboorsWithSameColor.Contains(NextNeighboor)) NeighboorsWithSameColor.Add(NextNeighboor);
                }
            }
        }
        
        foreach (Coordinates item in NeighboorsWithSameColor)
        {
            HexCoordinates[item].DestroyHex();
        }
        HexagonsMatchesColor.Rais(NeighboorsWithSameColor.Count);
        hexMovedEvent.Rais();
        ReGenerateHexs(NeighboorsWithSameColor);
        currentlySelectHexas = false;
        checkingMatchingColors = false;
        totalDegree = 0;
    }
    public void ReGenerateHexs(List<Coordinates> DiedHexs) {
        for (int i = 0; i < DiedHexs.Count; i++)
        {
            int countOfDiedHexInColumn = DiedHexs.Count(hexCordinate => hexCordinate.q == DiedHexs[i].q);
            Coordinates nextHex = HexCalculations.GetHexCoordinatesInColumn(DiedHexs[i], countOfDiedHexInColumn);
            if (!HexCoordinates.ContainsKey(nextHex)) hexFactory.RefactorHex(DiedHexs[i], HexCoordinates[DiedHexs[i]],ref countOfExplodHexInScene,true);
            while (HexCoordinates.ContainsKey(nextHex))
            {
                HexCell currentObject = HexCoordinates[DiedHexs[i]];
                HexCoordinates[DiedHexs[i]] = HexCoordinates[nextHex];
                hexFactory.RefactorHex(DiedHexs[i], HexCoordinates[DiedHexs[i]], ref countOfExplodHexInScene,false);
                HexCoordinates[nextHex] = currentObject;
                hexFactory.RefactorHex(nextHex, HexCoordinates[nextHex], ref countOfExplodHexInScene,true);
                nextHex = HexCalculations.GetHexCoordinatesInColumn(nextHex, countOfDiedHexInColumn);
            }
        }
    }
    void ShiftList(int rotation)
    {
        if (rotation == -1)
        {
            HexCell tmp = HexCoordinates[listOfSelectedHex[0]];
            for (int scan = 1; scan < listOfSelectedHex.Count; scan++)
            {

                HexCoordinates[listOfSelectedHex[scan - 1]] = HexCoordinates[listOfSelectedHex[scan]];
                HexCoordinates[listOfSelectedHex[scan - 1]].hexCorrdinates = listOfSelectedHex[scan - 1];

            }
            HexCoordinates[listOfSelectedHex[listOfSelectedHex.Count - 1]] = tmp;
            HexCoordinates[listOfSelectedHex[listOfSelectedHex.Count - 1]].hexCorrdinates = listOfSelectedHex[listOfSelectedHex.Count - 1];

            HexCoordinates[listOfSelectedHex[0]].UpdateText();
            HexCoordinates[listOfSelectedHex[1]].UpdateText();
            HexCoordinates[listOfSelectedHex[2]].UpdateText();

        }
        else
        {

            HexCell tmp = HexCoordinates[listOfSelectedHex[listOfSelectedHex.Count - 1]];
            for (int scan = listOfSelectedHex.Count - 1; scan > 0; scan--)
            {

                HexCoordinates[listOfSelectedHex[scan]] = HexCoordinates[listOfSelectedHex[scan - 1]];
                HexCoordinates[listOfSelectedHex[scan]].hexCorrdinates = listOfSelectedHex[scan];

            }
            HexCoordinates[listOfSelectedHex[0]] = tmp;
            HexCoordinates[listOfSelectedHex[0]].hexCorrdinates = listOfSelectedHex[0];

            HexCoordinates[listOfSelectedHex[0]].UpdateText();
            HexCoordinates[listOfSelectedHex[1]].UpdateText();
            HexCoordinates[listOfSelectedHex[2]].UpdateText();
        }
    }
    public void RestartGame() {
        GenerateGrid();
    }
    bool currentlySelectHexas;
    bool checkingMatchingColors;
    public void TouchEventStarted(Vector3 beginTouchPosition, TouchRotateDirction swipeDirction) {
        if (swipeDirction == TouchRotateDirction.NotDetected)
        {
            HandleTouchClickedAndGetSelectedHexas(beginTouchPosition);
            return;
        }
        if (!checkingMatchingColors && swipeDirction != TouchRotateDirction.NotDetected)
        {
            this.swipeDirction = swipeDirction;
            startRotate = true;
            checkingMatchingColors = true;
        }

    }
    private void HandleTouchClickedAndGetSelectedHexas(Vector3 beginTouchPosition) {
        Point clickedPoint = new Point(beginTouchPosition.x, beginTouchPosition.y);
        Coordinates[] sleectedHex = Layout.Instance.PixelToHex(clickedPoint).HexRound();
        listOfSelectedHex.Clear();
        for (int i = 0; i < sleectedHex.Length; i++)
        {
            if (!HexCoordinates.ContainsKey(sleectedHex[i]))
            {
                listOfSelectedHex.Clear();
                return;
            }
            listOfSelectedHex.Add(sleectedHex[i]);
        }
        listOfSelectedHex.Sort((a, b) =>
        {
            return a.s.CompareTo(b.s);
        });

        line.positionCount = 0;
        List<Point> points = new List<Point>();
        for (int i = 0; i < sleectedHex.Length; i++)
        {
            List<Point> pointsInHex = Layout.Instance.PolygonCorners(sleectedHex[i]);
            points.AddRange(pointsInHex);
        }
        points = GetConvexHull(points);
        line.positionCount += points.Count;
        for (int s = 0; s < points.Count; s++)
        {
            line.SetPosition(s, new Vector3((float)points[s].x, (float)points[s].y, 0));
        }
    }
}
