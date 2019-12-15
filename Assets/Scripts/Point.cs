using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : IComparable<Point> 
{
    private float x, y;

    public Point(float x, float y) {
        this.x = x;
        this.y = y;
    }
    public float X { get => x; set => x = value; }
    public float Y { get => y; set => y = value; }

    public int CompareTo(Point point) {
        return x.CompareTo(point.x);
    }
}
