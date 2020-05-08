using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextJumpModule
{
    List<GameObject> CointextList;
    List<int> CointextTime;

    List<GameObject> DamagetextList;
    List<int> DamagetextTime;
    public TextJumpModule()
    {
        CointextList = new List<GameObject>();
        CointextTime = new List<int>();
    }
    public void Free()
    {
        CointextList.Clear();
        CointextTime.Clear();
    }
    public void UpdateView()
    {
        UpdateCoinText();
    }
    void UpdateCoinText()
    {
        for (int i = CointextList.Count - 1; i >= 0; i--)
        {
            if (CointextTime[i] >= 41)
            {
                Object.Destroy(CointextList[i]);
                CointextList.RemoveAt(i);
                CointextTime.RemoveAt(i);
                continue;
            }
            if (CointextTime[i] >= 5 && CointextTime[i] <= 20)
            {
                CointextList[i].transform.position += new Vector3(0, 3, 0);
                Color now = CointextList[i].GetComponent<Text>().color;
                now.a = (20 - CointextTime[i]) * 1.0f / 15.0f;
                CointextList[i].GetComponent<Text>().color = now;
                CointextList[i].GetComponent<Text>().fontSize++;
            }
            CointextTime[i]++;
        }
    }

    /// <summary>
    /// 添加金币获得跳字
    /// </summary>
    /// <param name="Pos">人物位置</param>
    /// <param name="val">金币跳字值</param>
    public void AddCoinText(Vector2 Pos,int val)
    {
        Pos += new Vector2(0, 0.6f);//跳字头顶偏移值

        GameObject text = Resources.Load("UI/UIPrefabs/JumpText",typeof(GameObject)) as GameObject;
        text.GetComponent<Text>().color = new Color(1,249.0f/255.0f,0,1);
        text.GetComponent<Text>().text = "+" + val.ToString();
        CointextList.Add(Object.Instantiate(text, Camera.main.WorldToScreenPoint(Pos), Quaternion.identity, GameObject.Find("Canvas").transform));
        CointextTime.Add(0);
    }
    /// <summary>
    /// 添加受击伤害跳字
    /// </summary>
    public void AddBeAttackText(Vector2 pos,int val)
    {

    }
}
