﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroChange : MonoBehaviour
{
    Button btnLeft;
    Button btnRight;
    Vector3 LeftPosition;
    Vector3 RightPosition;
    public GameObject[] Hero;
    private List<GameObject> NewHero=new List<GameObject>();
    private int NowHeroIndex=0;
    private Vector3 NowHeroPosition;
    private GameObject NowHeroObj;
    private Text HeroName;
    private void Awake()
    {
        for(int i=0;i<4;i++)
        {
            if (Hero[i] == null) Debug.LogError("??");
            NewHero.Add(Object.Instantiate(Hero[i],new Vector3(0,0,0),Quaternion.identity,transform) as GameObject);
            NewHero[i].transform.localScale = new Vector3(400, 400, 0);

            for(int j = 0; j < NewHero[i].transform.childCount; ++j) NewHero[i].transform.GetChild(j).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            NewHero[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
        HeroName = transform.Find("Text").GetComponent<Text>();
        LeftPosition = transform.Find("LeftPosition").position;
        RightPosition = transform.Find("RightPosition").position;
        btnLeft = transform.Find("btnLeft").GetComponent<Button>();
        btnRight = transform.Find("btnRight").GetComponent<Button>();
        btnLeft.onClick.AddListener(OnbtnLeft);
        btnRight.onClick.AddListener(OnbtnRight);

        NowHeroObj = NewHero[NowHeroIndex];
    }

    private void Update()
    {
        NowHeroPosition = Vector3.Lerp(NowHeroPosition,transform.position,0.2f);
        NowHeroObj.transform.position = NowHeroPosition;
        NowHeroObj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - Mathf.Abs(NowHeroPosition.x - transform.position.x));
        for (int j = 0; j < NowHeroObj.transform.childCount; ++j) NowHeroObj.transform.GetChild(j).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - Mathf.Abs(NowHeroPosition.x - transform.position.x));
    }

    private void ClearColorAlpha(GameObject obj)
    {
        for (int j = 0; j < obj.transform.childCount; ++j) obj.transform.GetChild(j).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
    void OnbtnLeft()
    {
        ClearColorAlpha(NowHeroObj);
        NowHeroIndex=(NowHeroIndex+1+Hero.Length)%Hero.Length;
        NowHeroObj = NewHero[NowHeroIndex];
        NowHeroPosition = RightPosition;
        NowHeroObj.transform.position = NowHeroPosition;
        HeroName.text = NowHeroObj.name;
    }
    void OnbtnRight()
    {
        ClearColorAlpha(NowHeroObj);
        NowHeroIndex = (NowHeroIndex - 1 + Hero.Length) % Hero.Length;
        NowHeroObj = NewHero[NowHeroIndex];
        NowHeroPosition = LeftPosition;
        NowHeroObj.transform.position = NowHeroPosition;
        HeroName.text = NowHeroObj.name;
    }
}