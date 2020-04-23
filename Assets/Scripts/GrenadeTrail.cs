using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeTrail : MonoBehaviour
{
    public int startFrame;
    public int endFrame;
    public int cost;
    public float kx;
    public Vector3 s,t;
    public Vector3 dir;
    public void init(int frame,int c,Vector3 x,Vector3 y)
    {
        startFrame = frame;
        endFrame = frame + c;
        cost = c;
        s = x;
        t = y;
        this.transform.position = x;
        dir = y - x;
        dir.Normalize();
        
        float tmp = dir.x;
        dir.x = -dir.y;
        dir.y = tmp;

        kx = Vector3.Distance(x, y)/400;    //这里可以调节飞行高度
    }


    public void updateLogic(int frame)
    {
        Vector3 ts= this.transform.position + (t - s) / cost;

        this.transform.Rotate(new Vector3(0,0,-10));    //这里可以调节旋转速度
        
        if (2 * (frame - startFrame) <= cost)
        {
            ts = ts + kx * dir * (startFrame + cost / 2 - frame);
        }
        else
        {
            ts = ts - kx * dir * (frame - startFrame - cost / 2);
        }
        this.transform.position = ts;
    }
}
