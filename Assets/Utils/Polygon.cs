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