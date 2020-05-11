using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Line
{
    public FixVector2 p1;
    public FixVector2 p2;
    public Line() { }
    public Line(FixVector2 p1, FixVector2 p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }
}
public class Rectangle
{
    public FixVector2 anchor;
    public FixVector2 toward;
    public Fix64 horizon;
    public Fix64 vertical;
    public Rectangle() { }
    public Rectangle(FixVector2 anchor, FixVector2 toward, Fix64 horizon, Fix64 vertical)
    {
        this.anchor = anchor;
        this.toward = toward;
        this.horizon = horizon;
        this.vertical = vertical;
    }
}

public class Circle
{
    public FixVector2 anchor;
    public Fix64 radius;
    public Circle() { }
    public Circle(FixVector2 anchor, Fix64 radius)
    {
        this.anchor = anchor;
        this.radius = radius;
    }
}


public class Polygon
{
    public PolygonType type;
    public Line line;
    public Rectangle Rect;
    public Circle circle;
    public FixVector2 Point;

    public Polygon(PolygonType Polygontype)
    {
        type = Polygontype;
    }

    public void InitRectangle(FixVector2 anchor, FixVector2 toward, Fix64 horizon, Fix64 vertical)
    {
        Rect = new Rectangle(anchor, toward, horizon, vertical);
    }
    public void InitCircle(FixVector2 anchor, Fix64 radius)
    {
        circle = new Circle(anchor, radius);
    }
    public void InitLine(FixVector2 p1, FixVector2 p2)
    {
        line = new Line(p1, p2);
    }
    public void InitPoint(FixVector2 p)
    {
        Point = p;
    }
}

public enum PolygonType
{
    Circle,
    Rectangle,
    Line,
    Point
}