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
    public void BuildRectangleCollider(Vector2 anchor, Vector2 toward, float horizon, float vertical)
    {
        rectangle = new Rectangle(anchor, toward, horizon, vertical);
    }

    public void BuildCircleCollider(Vector2 anchor, Vector2 toward, float radius)
    {
        circle = new Circle(anchor, radius);
    }
    
}
