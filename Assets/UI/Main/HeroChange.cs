using System.Collections;
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
    private int NowHeroIndex=0;
    private Vector3 NowHeroPosition;
    private GameObject NowHeroObj;
    private Text HeroName;
    private void Awake()
    {
        HeroName = transform.Find("Text").GetComponent<Text>();
        LeftPosition = transform.Find("LeftPosition").position;
        RightPosition = transform.Find("RightPosition").position;
        btnLeft = transform.Find("btnLeft").GetComponent<Button>();
        btnRight = transform.Find("btnRight").GetComponent<Button>();
        btnLeft.onClick.AddListener(OnbtnLeft);
        btnRight.onClick.AddListener(OnbtnRight);
    }

    private void Update()
    {
        NowHeroPosition = Vector3.Lerp(NowHeroPosition,transform.position,0.05f);
        NowHeroObj.transform.position = NowHeroPosition;
    }

    void OnbtnLeft()
    {
        NowHeroIndex=(NowHeroIndex+1+Hero.Length)%Hero.Length;
        NowHeroObj = Hero[NowHeroIndex];
        NowHeroPosition = RightPosition;
        NowHeroObj.transform.position = NowHeroPosition;
        HeroName.text = NowHeroObj.name;
    }
    void OnbtnRight()
    {
        NowHeroIndex = (NowHeroIndex - 1 + Hero.Length) % Hero.Length;
        NowHeroObj = Hero[NowHeroIndex];
        NowHeroPosition = LeftPosition;
        NowHeroObj.transform.position = NowHeroPosition;
        HeroName.text = NowHeroObj.name;
    }
}
