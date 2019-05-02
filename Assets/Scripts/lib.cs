// Generated code -- CC0 -- No Rights Reserved -- http://www.redblobgames.com/grids/hexagons/

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public Point(double x, double y)
    {
        this.x = x;
        this.y = y;
    }
    public readonly double x;
    public readonly double y;
    public override string ToString()
    {
        return " Point: " + x + " Point: " + y;
    }

}

public struct Coordinates {
    [SerializeField]
    public int q;
    [SerializeField]
    public int r;
    [SerializeField]
    public int s;
    public Coordinates(int q, int r, int s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
        if (q + r + s != 0) throw new ArgumentException("q + r + s must be 0");
    }
    public override string ToString()
    {
        return " Qi: " + q + " Ri: " + r + " Si: " + s;
    }

}
[Serializable]
public class HexCalculations
{
    static private Coordinates coordinates;
    //public HexCalculations(int q, int r, int s)
    //{
    //    coordinates = new Coordinates(q, r, s);
    //}
    public override string ToString()
    {
        return " Qi: " + coordinates.q + " Ri: " + coordinates.r + " Si: " + coordinates.s;
    }
    static public Coordinates Add(Coordinates b)
    {
        return new Coordinates(coordinates.q + b.q, coordinates.r + b.r, coordinates.s + b.s);
    }
    static public Coordinates GetHexCoordinatesInColumn(Coordinates originalHex,int Spaces)
    {
        coordinates = originalHex;
        Coordinates ColumnDirction = new Coordinates(directions[5].q * Spaces, directions[5].r * Spaces, directions[5].s * Spaces);
        return Add(ColumnDirction);
    }


    //public Hex Subtract(Hex b)
    //{
    //    return new Hex(q - b.q, r - b.r, s - b.s);
    //}


    //public Hex Scale(int k)
    //{
    //    return new Hex(q * k, r * k, s * k);
    //}


    //public Hex RotateLeft()
    //{
    //    return new Hex(-s, -q, -r);
    //}


    //public Hex RotateRight()
    //{
    //    return new Hex(-r, -s, -q);
    //}

    static public List<Coordinates> directions = new List<Coordinates>{new Coordinates(1, 0, -1), new Coordinates(1, -1, 0), new Coordinates(0, -1, 1), new Coordinates(-1, 0, 1), new Coordinates(-1, 1, 0), new Coordinates(0, 1, -1)};

    static public Coordinates Direction(int direction,Coordinates coordinates)
    {
        HexCalculations.coordinates = coordinates;
        return directions[direction];
    }


    static public Coordinates Neighbor(int direction, Coordinates coordinates)
    {
        HexCalculations.coordinates = coordinates;
        return Add(Direction(direction, coordinates));
    }

    //static public List<Hex> diagonals = new List<Hex>{new Hex(2, -1, -1), new Hex(1, -2, 1), new Hex(-1, -1, 2), new Hex(-2, 1, 1), new Hex(-1, 2, -1), new Hex(1, 1, -2)};

    //public Hex DiagonalNeighbor(int direction)
    //{
    //    return Add(Hex.diagonals[direction]);
    //}


    //public int Length()
    //{
    //    return (int)((Math.Abs(coorq) + Math.Abs(r) + Math.Abs(s)) / 2);
    //}


    //public int Distance(Hex b)
    //{
    //    return Subtract(b).Length();
    //}

}

struct FractionalHex
{
    public FractionalHex(double q, double r, double s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
        if (Math.Round(q + r + s) != 0) throw new ArgumentException("q + r + s must be 0");
    }
    public readonly double q;
    public readonly double r;
    public readonly double s;

    public Coordinates[] HexRound()
    {
        int qi = (int)(Math.Round(q));
        int ri = (int)(Math.Round(r));
        int si = (int)(Math.Round(s));
        qi = (int)(Math.Ceiling(q));
        ri = (int)(Math.Round(r));
        si = -qi - ri;
        //Debug.Log(" qi: " + qi + " ri: " + ri + " si: " + si);
        //Debug.Log(" =================================== ");
        Coordinates hex1 = new Coordinates(qi, ri, si);

        qi = (int)(Math.Floor(q));
        ri = (int)(Math.Round(r));
        si = -qi - ri;
        //Debug.Log(" qi: " + qi + " ri: " + ri + " si: " + si);
        //Debug.Log(" =================================== ");
        Coordinates hex2 = new Coordinates(qi, ri, si);
        if ((int)Math.Floor(r) == (int)(Math.Round(r)))
        {
            qi = (int)(Math.Floor(q));
            ri = (int)(Math.Ceiling(r));
        }
        else 
        {
            qi = (int)(Math.Ceiling(q));
            ri = (int)(Math.Floor(r));
        }
        si = -qi - ri;
        //Debug.Log(" qi: " + qi + " ri: " + ri + " si: " + si);
        //Debug.Log(" =================================== ");
        Coordinates hex3 = new Coordinates(qi, ri, si);
        Coordinates[] hexs = new Coordinates[3];
        hexs[0] = hex1;
        hexs[1] = hex2;
        hexs[2] = hex3;
        return hexs;
    }


    //public FractionalHex HexLerp(FractionalHex b, double t)
    //{
    //    return new FractionalHex(q * (1.0 - t) + b.q * t, r * (1.0 - t) + b.r * t, s * (1.0 - t) + b.s * t);
    //}


    //static public List<Hex> HexLinedraw(Hex a, Hex b)
    //{
    //    int N = a.Distance(b);
    //    FractionalHex a_nudge = new FractionalHex(a.q + 0.000001, a.r + 0.000001, a.s - 0.000002);
    //    FractionalHex b_nudge = new FractionalHex(b.q + 0.000001, b.r + 0.000001, b.s - 0.000002);
    //    List<Hex> results = new List<Hex>{};
    //    double step = 1.0 / Math.Max(N, 1);
    //    for (int i = 0; i <= N; i++)
    //    {
    //        //results.Add(a_nudge.HexLerp(b_nudge, step * i).HexRound());
    //    }
    //    return results;
    //}

}

struct OffsetCoord
{
    public OffsetCoord(int col, int row)
    {
        this.col = col;
        this.row = row;
    }
    public readonly int col;
    public readonly int row;
    static public int EVEN = 1;
    static public int ODD = -1;

    //static public OffsetCoord QoffsetFromCube(int offset, Hex h)
    //{
    //    int col = h.q;
    //    int row = h.r + (int)((h.q + offset * (h.q & 1)) / 2);
    //    return new OffsetCoord(col, row);
    //}


    //static public Hex QoffsetToCube(int offset, OffsetCoord h)
    //{
    //    int q = h.col;
    //    int r = h.row - (int)((h.col + offset * (h.col & 1)) / 2);
    //    int s = -q - r;
    //    return new Hex(q, r, s);
    //}


    //static public OffsetCoord RoffsetFromCube(int offset, Hex h)
    //{
    //    int col = h.q + (int)((h.r + offset * (h.r & 1)) / 2);
    //    int row = h.r;
    //    return new OffsetCoord(col, row);
    //}


    //static public HexCalculations RoffsetToCube(int offset, OffsetCoord h)
    //{
    //    int q = h.col - (int)((h.row + offset * (h.row & 1)) / 2);
    //    int r = h.row;
    //    int s = -q - r;
    //    return new HexCalculations(q, r, s);
    //}

}

//struct DoubledCoord
//{
//    public DoubledCoord(int col, int row)
//    {
//        this.col = col;
//        this.row = row;
//    }
//    public readonly int col;
//    public readonly int row;

//    static public DoubledCoord QdoubledFromCube(Hex h)
//    {
//        int col = h.q;
//        int row = 2 * h.r + h.q;
//        return new DoubledCoord(col, row);
//    }


//    public Hex QdoubledToCube()
//    {
//        int q = col;
//        int r = (int)((row - col) / 2);
//        int s = -q - r;
//        return new Hex(q, r, s);
//    }


//    static public DoubledCoord RdoubledFromCube(Hex h)
//    {
//        int col = 2 * h.q + h.r;
//        int row = h.r;
//        return new DoubledCoord(col, row);
//    }


//    public Hex RdoubledToCube()
//    {
//        int q = (int)((col - row) / 2);
//        int r = row;
//        int s = -q - r;
//        return new Hex(q, r, s);
//    }

//}

struct Orientation
{
    public Orientation(double f0, double f1, double f2, double f3, double b0, double b1, double b2, double b3, double start_angle)
    {
        this.f0 = f0;
        this.f1 = f1;
        this.f2 = f2;
        this.f3 = f3;
        this.b0 = b0;
        this.b1 = b1;
        this.b2 = b2;
        this.b3 = b3;
        this.start_angle = start_angle;
    }
    public readonly double f0;
    public readonly double f1;
    public readonly double f2;
    public readonly double f3;
    public readonly double b0;
    public readonly double b1;
    public readonly double b2;
    public readonly double b3;
    public readonly double start_angle;
}
public enum OrientationType
{
    PointyTop = 0,
    FlatTop = 30
}

class Layout
{
    public static Layout Instance;
    public Layout(OrientationType orientation, Point size, Point origin)
    {
        switch (orientation)
        {
            case OrientationType.PointyTop:
                this.orientation = pointy;
                break;
            case OrientationType.FlatTop:
                this.orientation = flat;
                break;
            default:
                break;
        }
        this.size = size;
        this.origin = origin;
        Instance = this;
    }
    public readonly Orientation orientation;
    public readonly Point size;
    public readonly Point origin;
    static public Orientation pointy = new Orientation(
          Math.Sqrt(3.0)
        , Math.Sqrt(3.0) / 2.0
        , 0.0
        , 3.0 / 2.0
        , Math.Sqrt(3.0) / 3.0
        , -1.0 / 3.0
        , 0.0
        , 2.0 / 3.0
        , 0.5);
    static public Orientation flat = new Orientation(3.0 / 2.0, 0.0, Math.Sqrt(3.0) / 2.0, Math.Sqrt(3.0), 2.0 / 3.0, 0.0, -1.0 / 3.0, Math.Sqrt(3.0) / 3.0, 0.0);

    public Point HexToPixel(Coordinates h)
    {
        Orientation M = orientation;
        double x = (M.f0 * h.q + M.f1 * h.r) * size.x;
        double y = (M.f2 * h.q + M.f3 * h.r) * size.y;
        //Debug.Log(new Point(x + origin.x, y + origin.y).ToString());
        return new Point(x + origin.x, y + origin.y);
    }


    public FractionalHex PixelToHex(Point p)
    {
        Orientation M = orientation;
        Point pt = new Point((p.x - origin.x) / size.x, (p.y - origin.y) / size.y);
        double q = M.b0 * pt.x + M.b1 * pt.y;
        double r = M.b2 * pt.x + M.b3 * pt.y;
        return new FractionalHex(q, r, -q - r);
    }


    public Point HexCornerOffset(int corner)
    {
        Orientation M = orientation;
        double angle = 2.0 * Math.PI * (M.start_angle - corner) / 6.0;
        return new Point(size.x * Math.Cos(angle), size.y * Math.Sin(angle));
    }


    public List<Point> PolygonCorners(Coordinates h)
    {
        List<Point> corners = new List<Point>{};
        Point center = HexToPixel(h);
        for (int i = 0; i < 6; i++)
        {
            Point offset = HexCornerOffset(i);
            corners.Add(new Point(center.x + offset.x, center.y + offset.y));
        }
        return corners;
    }

}