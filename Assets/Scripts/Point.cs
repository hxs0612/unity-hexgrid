using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : IEquatable<Point>
{
    public int x;
    public int y;

    public int PointId { get; set; }

    public Point(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public override string ToString() {
        return "x: " + x + ", y: " + y;
    }

    public override bool Equals(object obj) {
        if (obj == null) {
            return false;
        }
        Point point = obj as Point;
        if (point == null) {
            return false;
        } else {
            return Equals(point);
        }
    }

    public bool Equals(Point point) {
        if (point is null) {
            return false;
        }
        return x == point.x && y == point.y;
    }

    public override int GetHashCode()
    {
        return PointId;
    }
}
