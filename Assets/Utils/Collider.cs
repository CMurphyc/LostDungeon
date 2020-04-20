using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBase
{
    public Rectangle rectangle;
    public Circle circle;
}
public class CommonCollider : ColliderBase
{
    public void BuildRectangleCollider(FixVector2 anchor, FixVector2 toward, Fix64 horizon, Fix64 vertical)
    {
        rectangle = new Rectangle(anchor, toward, horizon, vertical);
    }

    public void BuildCircleCollider(FixVector2 anchor, FixVector2 toward, Fix64 radius)
    {
        circle = new Circle(anchor, radius);
    }

}