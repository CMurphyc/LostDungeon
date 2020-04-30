using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideDetecter
{
    public bool CircleCollideRect(Circle circle, Rectangle rect)
    {
        FixVector2 upperLeft = rect.anchor;
        FixVector2 upperRight = rect.anchor;
        FixVector2 bottomLeft = rect.anchor;
        FixVector2 bottomRight = rect.anchor;

        GetRectPoint(ref upperLeft, ref upperRight, ref bottomLeft, ref bottomRight, rect);

        Line line1 = new Line(upperLeft, upperRight);
        Line line2 = new Line(upperRight, bottomRight);
        Line line3 = new Line(bottomLeft, bottomRight);
        Line line4 = new Line(upperLeft, bottomLeft);

        //情况一：圆与矩形边相交（本质上相当于点到边小于圆的半径或者圆包含矩形）
        if (CircleCollideLine(circle, line1) == true ||
            CircleCollideLine(circle, line2) == true ||
            CircleCollideLine(circle, line3) == true ||
            CircleCollideLine(circle, line4) == true)
            return true;

        //情况二：矩形包含圆（本质相当于圆心在矩形内）
        if (PointInRectangle(circle.anchor, rect) == true) return true;

        return false;
    }

    public bool CircleCollideCircle(Circle circleA, Circle circleB)
    {
        if (circleA.radius + circleB.radius <= FixVector2.Distance(circleA.anchor, circleB.anchor)) return true;
        return false;
    }

    public bool RectangleCollideRectangle(Rectangle rectA, Rectangle rectB)
    {
        FixVector2 upperLeft = rectA.anchor;
        FixVector2 upperRight = rectA.anchor;
        FixVector2 bottomLeft = rectA.anchor;
        FixVector2 bottomRight = rectA.anchor;

        GetRectPoint(ref upperLeft, ref upperRight, ref bottomLeft, ref bottomRight, rectA);

        Line line1 = new Line(upperLeft, upperRight);
        Line line2 = new Line(upperRight, bottomRight);
        Line line3 = new Line(bottomLeft, bottomRight);
        Line line4 = new Line(upperLeft, bottomLeft);

        //情况一：点在矩形中or矩形包含
        if (PointInRectangle(upperLeft, rectB) == true ||
           PointInRectangle(upperRight, rectB) == true ||
           PointInRectangle(bottomLeft, rectB) == true ||
           PointInRectangle(bottomRight, rectB) == true)
            return true;

        //情况二：边相交
        if (LineCollideRectangle(line1, rectB) == true ||
           LineCollideRectangle(line2, rectB) == true ||
           LineCollideRectangle(line3, rectB) == true ||
           LineCollideRectangle(line4, rectB) == true)
            return true;

        return false;
    }

    public bool CircleCollideLine(Circle circle, Line line)
    {
        Fix64 dist = PointToLine(circle.anchor, line);
        if (dist <= circle.radius) return true;
        else return false;
    }

    public bool LineCollideRectangle(Line line, Rectangle rect)
    {
        FixVector2 upperLeft = rect.anchor;
        FixVector2 upperRight = rect.anchor;
        FixVector2 bottomLeft = rect.anchor;
        FixVector2 bottomRight = rect.anchor;

        GetRectPoint(ref upperLeft, ref upperRight, ref bottomLeft, ref bottomRight, rect);

        Line line1 = new Line(upperLeft, upperRight);
        Line line2 = new Line(upperRight, bottomRight);
        Line line3 = new Line(bottomLeft, bottomRight);
        Line line4 = new Line(upperLeft, bottomLeft);

        if (LineCollideLine(line, line1) == true ||
           LineCollideLine(line, line2) == true ||
           LineCollideLine(line, line3) == true ||
           LineCollideLine(line, line4) == true)
            return true;
        return false;
    }

    public bool PointInRectangle(FixVector2 point, Rectangle rect)
    {
        FixVector2 upperLeft = rect.anchor;
        FixVector2 upperRight = rect.anchor;
        FixVector2 bottomLeft = rect.anchor;
        FixVector2 bottomRight = rect.anchor;

        GetRectPoint(ref upperLeft, ref upperRight, ref bottomLeft, ref bottomRight, rect);

        Line line1 = new Line(upperLeft, upperRight);
        Line line2 = new Line(upperRight, bottomRight);
        Line line3 = new Line(bottomLeft, bottomRight);
        Line line4 = new Line(upperLeft, bottomLeft);

        if (PointToLine(point, line1) +
            PointToLine(point, line2) +
            PointToLine(point, line3) +
            PointToLine(point, line4) <= (rect.horizon + rect.vertical + (Fix64)0.01f))
        {
            //Debug.Log("IN");
            //Debug.Log("anchor is + " + rect.anchor);
            //Debug.Log("Distance * 4 = " + (PointToLine(point, line1) +
            //PointToLine(point, line2) +
            //PointToLine(point, line3) +
            //PointToLine(point, line4) + "周长/2 = " + (rect.horizon + rect.vertical)));
            return true;
        }
        return false;
    }

    public Vector2 Vector2ToFixVector2(FixVector2 v)
    {
        Vector2 newv = new Vector2((float)v.x, (float)v.y);
        return newv;
    }
    public bool LineCollideLine(Line line1, Line line2)
    {
        //var crossA = Fix64.Sign((Fix64)Vector3.Cross(Vector2ToFixVector2(line2.p2) - Vector2ToFixVector2(line2.p1), Vector2ToFixVector2(line1.p1) - Vector2ToFixVector2(line2.p1)).y);
        //var crossB = Fix64.Sign((Fix64)Vector3.Cross(Vector2ToFixVector2(line2.p2) - Vector2ToFixVector2(line2.p1), Vector2ToFixVector2(line1.p2) - Vector2ToFixVector2(line2.p1)).y);

        //if (Mathf.Approximately(crossA, crossB)) return false;

        //var crossC = Fix64.Sign((Fix64)Vector3.Cross(Vector2ToFixVector2(line1.p2) - Vector2ToFixVector2(line1.p1), Vector2ToFixVector2(line2.p1) - Vector2ToFixVector2(line1.p1)).y);
        //var crossD = Fix64.Sign((Fix64)Vector3.Cross(Vector2ToFixVector2(line1.p2) - Vector2ToFixVector2(line1.p1), Vector2ToFixVector2(line2.p2) - Vector2ToFixVector2(line1.p1)).y);


        //if (Mathf.Approximately(crossC, crossD)) return false;

        var crossA = Fix64.Sign(FixVector3.Cross(PackConverter.FixVector2ToFixVector3(line2.p2 - line2.p1), PackConverter.FixVector2ToFixVector3(line1.p1 - line2.p1)).y);
        var crossB = Fix64.Sign(FixVector3.Cross(PackConverter.FixVector2ToFixVector3(line2.p2 - line2.p1), PackConverter.FixVector2ToFixVector3(line1.p2 - line2.p1)).y);

        if (Mathf.Approximately(crossA, crossB)) return false;

        var crossC = Fix64.Sign(FixVector3.Cross(PackConverter.FixVector2ToFixVector3(line1.p2 - line1.p1), PackConverter.FixVector2ToFixVector3(line2.p1 - line1.p1)).y);
        var crossD = Fix64.Sign(FixVector3.Cross(PackConverter.FixVector2ToFixVector3(line1.p2 - line1.p1), PackConverter.FixVector2ToFixVector3(line2.p2 - line1.p1)).y);


        if (Mathf.Approximately(crossC, crossD)) return false;

        return true;
    }
    public Fix64 Dot(FixVector2 v1, FixVector2 v2)
    {
        return v1.x * v2.x + v1.y * v2.y;
    }
    public Fix64 Cross(FixVector2 v1, FixVector2 v2)
    {
        return v1.x * v2.y - v1.y * v2.x;
    }
    public Fix64 Length(FixVector2 v)
    {
        return Fix64.Sqrt(v.x * v.x + v.y * v.y);
    }
    public Fix64 PointToLine(FixVector2 point, Line line)
    {
        if (line.p1 == line.p2)
        {
            FixVector2 newVector = point - line.p1;
            return Fix64.Sqrt(newVector.x * newVector.x + newVector.y * newVector.y);
        }
        FixVector2 v1 = line.p2 - line.p1;
        FixVector2 v2 = point - line.p1;
        FixVector2 v3 = point - line.p2;
        if (Dot(v1, v2) < Fix64.Zero) return Length(v2);
        else if (Dot(v1, v3) > Fix64.Zero) return Length(v3);
        else
        {
            Fix64 dis = Cross(v1, v2) / Length(v1);
            if (dis < Fix64.Zero) dis *= (Fix64)(-1);
            return dis;
        }
    }
    public void PointRotate(FixVector2 center, ref FixVector2 p1, Fix64 angle)
    {
        Fix64 x1 = (Fix64)((p1.x - center.x) * Fix64.Cos(angle) + (p1.y - center.y) * Fix64.Sin(angle) + center.x);
        Fix64 y1 = (Fix64)(-(p1.x - center.x) * Fix64.Sin(angle) + (p1.y - center.y) * Fix64.Cos(angle) + center.y);
        p1.x = x1;
        p1.y = y1;
    }
    public void GetRectPoint(ref FixVector2 upperLeft, ref FixVector2 upperRight, ref FixVector2 bottomLeft, ref FixVector2 bottomRight, Rectangle Rect)
    {
        upperLeft.x = Rect.anchor.x - Rect.horizon / (Fix64)2f;
        upperLeft.y = Rect.anchor.y + Rect.vertical / (Fix64)2f;

        upperRight.x = Rect.anchor.x + Rect.horizon / (Fix64)2f;
        upperRight.y = Rect.anchor.y + Rect.vertical / (Fix64)2f;

        bottomLeft.x = Rect.anchor.x - Rect.horizon / (Fix64)2f;
        bottomLeft.y = Rect.anchor.y - Rect.vertical / (Fix64)2f;

        bottomRight.x = Rect.anchor.x + Rect.horizon / (Fix64)2f;
        bottomRight.y = Rect.anchor.y - Rect.vertical / (Fix64)2f;
        /*
        Debug.Log("upperleft is " + upperLeft.x + " " + upperLeft.y);
        Debug.Log("upperright is " + upperRight.x + " " + upperRight.y);
        Debug.Log("bottomleft is " + bottomLeft.x + " " + bottomLeft.y);
        Debug.Log("bottomright is " + bottomRight.x + " " + bottomRight.y);
        */
        //根据朝向旋转矩形，如果之后扩展到多边形的话也可以用
        /*Fix64 angle;
        if (Rect.toward.x == (Fix64)0) angle = (Fix64)90f;
        else
        {
            Debug.Log("?????????");
            angle = Fix64.Atan(Rect.toward.y / Rect.toward.x);
        }*/
        //PointRotate(Rect.anchor, ref upperLeft, angle);
        //PointRotate(Rect.anchor, ref upperRight, angle);
        //PointRotate(Rect.anchor, ref bottomLeft, angle);
        //PointRotate(Rect.anchor, ref bottomRight, angle);
    }
}