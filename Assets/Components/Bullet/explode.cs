using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class explode
{
    private GameObject centerCircle;
    private GameObject allaroundRect;

    //测试帧率默认20帧
    private int survivalTime = 0;
    private float circleA;
    private int rectNum;
    private bool overCircle = false;
    private bool overRect = false;
    private FixVector2 anchor;

    //rect个数为随机数，暂定5-7个
    private List<GameObject> rect;
    private List<float> rectA;
    private enum TimeNode
    {
        centerStart = 0,
        aroundStart = 3
    }

    public explode(FixVector2 anchor)
    {
        this.centerCircle = Resources.Load("Effects/Prefab/circle") as GameObject;
        this.allaroundRect = Resources.Load("Effects/Prefab/rect") as GameObject;
        this.anchor = anchor;
        this.circleA = 1;

        rect = new List<GameObject>();
        rectA = new List<float>();

        GetRectNum();

        for (int i = 0; i < rectNum; ++i) rectA.Add(1f);
    }

    private void GetRectNum()
    {
        System.Random rd = new System.Random();
        rectNum = rd.Next(9, 12);
    }

    private void GetCenterInstance()
    {
        centerCircle = GameObject.Instantiate(centerCircle, Converter.FixVector2ToVector2(anchor), centerCircle.transform.rotation);
    }

    private void GetAroundInstance()
    {
        RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();
        byte[] byteCsp = new byte[10];
        //生成的锚点位置是随机的，如果有重叠的话就重新随机
        for (int i = 0; i < rectNum; ++i)
        {
            csp.GetBytes(byteCsp);
            System.BitConverter.ToInt32(byteCsp, 0);

            //固定锚点，随机偏移量
            csp.GetBytes(byteCsp);
            float offsetX = System.BitConverter.ToInt32(byteCsp, 0) % 3 * 0.05f;
            csp.GetBytes(byteCsp);
            float offsetY = System.BitConverter.ToInt32(byteCsp, 0) % 3 * 0.05f;

            //Debug.Log("offsetX " + offsetX);
            //Debug.Log("offsetY " + offsetY);

            //随机rect的scale
            float scale = 0.2f;

            //随机偏移量的符号
            csp.GetBytes(byteCsp);
            if (System.BitConverter.ToInt32(byteCsp, 0) % 2 == 1) offsetX *= 1;
            else offsetX *= -1;

            csp.GetBytes(byteCsp);
            if (System.BitConverter.ToInt32(byteCsp, 0) % 2 == 1) offsetY *= 1;
            else offsetY *= -1;

            Vector2 rectAnchor = Converter.FixVector2ToVector2(anchor);
            rectAnchor.x += offsetX;
            rectAnchor.y += offsetY;

            GameObject obj = GameObject.Instantiate(allaroundRect, rectAnchor, allaroundRect.transform.rotation);
            obj.transform.localScale = new Vector3(scale, scale, scale);

            rect.Add(obj);
        }

    }

    private void SurvivalTimeUpdate()
    {
        if (survivalTime <= 3) survivalTime++;
        else survivalTime = 4;
    }

    private void CircleViewUpdate()
    {
        if (overCircle == true) return;

        if (centerCircle.GetComponent<Renderer>().material.color.a <= 1 && centerCircle.GetComponent<Renderer>().material.color.a > 0)
        {
            centerCircle.GetComponent<Renderer>().material.color = new Color
            (
                centerCircle.GetComponent<Renderer>().material.color.r,
                centerCircle.GetComponent<Renderer>().material.color.g,
                centerCircle.GetComponent<Renderer>().material.color.b,
                circleA
            );
        }

        if (circleA <= 0)
        {
            Object.Destroy(centerCircle);
            overCircle = true;
            return;
        }
    }

    private void RectViewUpdate()
    {
        if (survivalTime < (int)TimeNode.aroundStart || overRect == true) return;

        for (int i = 0; i < rectNum; ++i)
        {
            if (rect[i].GetComponent<Renderer>().material.color.a <= 1 && rect[i].GetComponent<Renderer>().material.color.a > 0)
            {
                rect[i].GetComponent<Renderer>().material.color = new Color
                (
                    rect[i].GetComponent<Renderer>().material.color.r,
                    rect[i].GetComponent<Renderer>().material.color.g,
                    rect[i].GetComponent<Renderer>().material.color.b,
                    rectA[i]
                );
            }

            if (rectA[i] <= 0) overRect = true;
        }

        if (overRect == true)
        {
            for (int i = 0; i < rectNum; ++i) Object.Destroy(rect[i]);
        }
    }

    private void CircleAndRectViewUpdate()
    {
        CircleViewUpdate();
        if (survivalTime >= (int)TimeNode.aroundStart) RectViewUpdate();
    }

    private void CircleLogicUpdate()
    {
        if (circleA > 0) circleA -= 0.15f;
    }

    private void RectLogicUpdate()
    {
        for (int i = 0; i < rectNum; ++i)
        {
            if (rectA[i] > 0) rectA[i] -= 0.15f;
        }
    }

    private void CircleAndRectLogicUpdate()
    {
        CircleLogicUpdate();
        if (survivalTime >= (int)TimeNode.aroundStart) RectLogicUpdate();
    }

    public void LogicUpdate()
    {
        if (overCircle == true && overRect == true) return;

        switch (survivalTime)
        {
            case (int)TimeNode.centerStart:
                CircleLogicUpdate();
                break;
            case (int)TimeNode.aroundStart:
                RectLogicUpdate();
                break;
            default:
                CircleAndRectLogicUpdate();
                break;
        }
    }

    public void ViewUpdate()
    {
        if (overCircle == true && overRect == true) return;

        switch (survivalTime)
        {
            case (int)TimeNode.centerStart:
                GetCenterInstance();
                break;
            case (int)TimeNode.aroundStart:
                GetAroundInstance();
                break;
            default:
                CircleAndRectViewUpdate();
                break;
        }

        SurvivalTimeUpdate();
    }

}