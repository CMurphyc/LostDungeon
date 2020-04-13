using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideDetecter
{
    public bool CircleCollideRect(Circle circle, Rectangle rect)
    {
        Vector2 upperLeft = rect.anchor;
        Vector2 upperRight = rect.anchor;
        Vector2 bottomLeft = rect.anchor;
        Vector2 bottomRight = rect.anchor;

        GetRectPoint(ref upperLeft, ref upperRight, ref bottomLeft, ref bottomRight, rect);

        Line line1 = new Line(upperLeft, upperRight);
        Line line2 = new Line(upperRight, bottomRight);
        Line line3 = new Line(bottomLeft, bottomRight);
        Line line4 = new Line(upperLeft, bottomLeft);

        //情况一：圆与矩形边相交（本质上相当于点到边小于圆的半径或者圆包含矩形）
        if( CircleCollideLine(circle, line1) == true ||
            CircleCollideLine(circle, line2) == true ||
            CircleCollideLine(circle, line3) == true ||
            CircleCollideLine(circle, line4) == true)
            return true;

        //情况二：矩形包含圆（本质相当于圆心在矩形内）
        if(PointInRectangle(circle.anchor, rect) == true) return true;

        return false;
    }

    public bool CircleCollideCircle(Circle circleA, Circle circleB)
    {
        if(circleA.radius + circleB.radius <= Vector2.Distance(circleA.anchor, circleB.anchor)) return true;
        return false;
    }

    public bool RectangleCollideRectangle(Rectangle rectA, Rectangle rectB)
    {
        Vector2 upperLeft = rectA.anchor;
        Vector2 upperRight = rectA.anchor;
        Vector2 bottomLeft = rectA.anchor;
        Vector2 bottomRight = rectA.anchor;

        GetRectPoint(ref upperLeft, ref upperRight, ref bottomLeft, ref bottomRight, rectA);

        Line line1 = new Line(upperLeft, upperRight);
        Line line2 = new Line(upperRight, bottomRight);
        Line line3 = new Line(bottomLeft, bottomRight);
        Line line4 = new Line(upperLeft, bottomLeft);

        //情况一：点在矩形中or矩形包含
        if(PointInRectangle(upperLeft, rectB) == true  || 
           PointInRectangle(upperRight, rectB) == true ||
           PointInRectangle(bottomLeft, rectB) == true ||
           PointInRectangle(bottomRight, rectB) == true)
           return true;

        //情况二：边相交
        if(LineCollideRectangle(line1, rectB) == true ||
           LineCollideRectangle(line2, rectB) == true ||
           LineCollideRectangle(line3, rectB) == true ||
           LineCollideRectangle(line4, rectB) == true)
           return true;
        
        return false;
    }

    public bool CircleCollideLine(Circle circle, Line line)
    {
        float dist = PointToLine(circle.anchor, line);
        if(dist <= circle.radius) return true;
        else return false;
    }

    private bool LineCollideRectangle(Line line, Rectangle rect)
    {
        Vector2 upperLeft = rect.anchor;
        Vector2 upperRight = rect.anchor;
        Vector2 bottomLeft = rect.anchor;
        Vector2 bottomRight = rect.anchor;

        GetRectPoint(ref upperLeft, ref upperRight, ref bottomLeft, ref bottomRight, rect);

        Line line1 = new Line(upperLeft, upperRight);
        Line line2 = new Line(upperRight, bottomRight);
        Line line3 = new Line(bottomLeft, bottomRight);
        Line line4 = new Line(upperLeft, bottomLeft);

        if(LineCollideLine(line, line1) == true ||
           LineCollideLine(line, line2) == true ||
           LineCollideLine(line, line3) == true ||
           LineCollideLine(line, line4) == true)
           return true;
        return false;
    }

    private bool PointInRectangle(Vector2 point, Rectangle rect)
    {
        Vector2 upperLeft = rect.anchor;
        Vector2 upperRight = rect.anchor;
        Vector2 bottomLeft = rect.anchor;
        Vector2 bottomRight = rect.anchor;

        GetRectPoint(ref upperLeft, ref upperRight, ref bottomLeft, ref bottomRight, rect);

        Line line1 = new Line(upperLeft, upperRight);
        Line line2 = new Line(upperRight, bottomRight);
        Line line3 = new Line(bottomLeft, bottomRight);
        Line line4 = new Line(upperLeft, bottomLeft);

        if( PointToLine(point, line1) +
            PointToLine(point, line2) +
            PointToLine(point, line3) +
            PointToLine(point, line4) <= (rect.horizon + rect.vertical) * 2)
            return true;
        return false;
    }
    private bool LineCollideLine(Line line1, Line line2)
    {
        var crossA = Mathf.Sign(Vector3.Cross(line2.p2 - line2.p1, line1.p1 - line2.p1).y);
        var crossB = Mathf.Sign(Vector3.Cross(line2.p2 - line2.p1, line1.p2 - line2.p1).y);

        if(Mathf.Approximately(crossA, crossB)) return false;

        var crossC = Mathf.Sign(Vector3.Cross(line1.p2 - line1.p1, line2.p1 - line1.p1).y);
        var crossD = Mathf.Sign(Vector3.Cross(line1.p2 - line1.p1, line2.p2 - line1.p1).y);

        if(Mathf.Approximately(crossC, crossD)) return false;

        return true;
    }
    private float PointToLine(Vector2 point, Line line)
    {
        float x = point.x, y = point.y;
        float x1 = line.p1.x, y1 = line.p1.y;
        float x2 = line.p2.x, y2 = line.p2.y;

        float cross = (x2 - x1) * (x - x1) + (y2 - y1) * (y - y1);

        if (cross <= 0) return Mathf.Sqrt((x - x1) * (x - x1) + (y - y1) * (y - y1));

        float d2 = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
        if (cross >= d2) return Mathf.Sqrt((x - x2) * (x - x2) + (y - y2) * (y - y2));

        float r = cross / d2;
        float px = x1 + (x2 - x1) * r;
        float py = y1 + (y2 - y1) * r;
        return Mathf.Sqrt((x - px) * (x - px) + (py - y1) * (py - y1));
    }
    private void PointRotate(Vector2 center, ref Vector2 p1, float angle)
    {
        float x1 = (float)((p1.x - center.x) * Mathf.Cos(angle) + (p1.y - center.y) * Mathf.Sin(angle) + center.x);
        float y1 = (float)(-(p1.x - center.x) * Mathf.Sin(angle) + (p1.y - center.y) * Mathf.Cos(angle) + center.y);
        p1.x = x1;
        p1.y = y1;
    }
    private void GetRectPoint(ref Vector2 upperLeft, ref Vector2 upperRight, ref Vector2 bottomLeft, ref Vector2 bottomRight, Rectangle Rect)
    {
        upperLeft.x = Rect.anchor.x - Rect.horizon / 2f;
        upperLeft.y = Rect.anchor.y + Rect.vertical / 2f;

        upperRight.x = Rect.anchor.x + Rect.horizon / 2f;
        upperRight.y = Rect.anchor.y + Rect.vertical / 2f;

        bottomLeft.x = Rect.anchor.x - Rect.horizon / 2f;
        bottomLeft.y = Rect.anchor.y - Rect.vertical / 2f;

        bottomRight.x = Rect.anchor.x + Rect.horizon / 2f;
        bottomRight.y = Rect.anchor.y - Rect.vertical / 2f;

        //根据朝向旋转矩形，如果之后扩展到多边形的话也可以用
        float angle = Mathf.Atan(Rect.toward.y / Rect.toward.x);
        PointRotate(Rect.anchor, ref upperLeft, angle);
        PointRotate(Rect.anchor, ref upperRight, angle);
        PointRotate(Rect.anchor, ref bottomLeft, angle);
        PointRotate(Rect.anchor, ref bottomRight, angle);
    }
}
