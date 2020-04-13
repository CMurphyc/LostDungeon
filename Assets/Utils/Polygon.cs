using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Line
{
    public Vector2 p1;
    public Vector2 p2;
    public Line() {}
    public Line(Vector2 p1, Vector2 p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }
}
public class Rectangle
{
    public Vector2 anchor;
    public Vector2 toward;
    public float horizon;
    public float vertical;
    public Rectangle() {}
    public Rectangle(Vector2 anchor, Vector2 toward, float horizon, float vertical)
    {
        this.anchor = anchor;
        this.toward = toward;
        this.horizon = horizon;
        this.vertical = vertical;
    }
}

public class Circle
{
    public Vector2 anchor;
    public float radius;
    public Circle() {}
    public Circle(Vector2 anchor, float radius)
    {
        this.anchor = anchor;
        this.radius = radius;
    }
}
