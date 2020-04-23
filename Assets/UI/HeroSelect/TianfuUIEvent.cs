using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TianfuUIEvent : MonoBehaviour
{
    GameObject main;
    private GameObject Tianfu;
    private Button[] skillButton = new Button[10];
    private Button SaveBtn;
    private Button ExitBtn;
    private HashSet<int> skill=new HashSet<int>();
    private bool IsSave=false;
    void Awake()
    {
        main = GameObject.FindWithTag("GameEntry");
        Tianfu = transform.Find("Tianfu").gameObject;
        SaveBtn = Tianfu.transform.Find("btnSave").GetComponent<Button>();
        ExitBtn = Tianfu.transform.Find("btnExit").GetComponent<Button>();
        for (int i=0;i<10;i++)
        {
            skillButton[i] = Tianfu.transform.Find("btnSkill"+i.ToString()).GetComponent<Button>();
        }
        transform.GetComponent<Button>().onClick.AddListener(OnTianfuBtn);
        SaveBtn.onClick.AddListener(OnSaveBtn);
        ExitBtn.onClick.AddListener(OnExitBtn);
        skillButton[0].onClick.AddListener(OnSkillBtn0);
        skillButton[1].onClick.AddListener(OnSkillBtn1);
        skillButton[2].onClick.AddListener(OnSkillBtn2);
        skillButton[3].onClick.AddListener(OnSkillBtn3);
        skillButton[4].onClick.AddListener(OnSkillBtn4);
        skillButton[5].onClick.AddListener(OnSkillBtn5);
        skillButton[6].onClick.AddListener(OnSkillBtn6);
        skillButton[7].onClick.AddListener(OnSkillBtn7);
        skillButton[8].onClick.AddListener(OnSkillBtn8);
        skillButton[9].onClick.AddListener(OnSkillBtn9);
    }
    void Start()
    {
        IsSave = false;
        skill = new HashSet<int>();
    }
    public void OnTianfuBtn()
    {
        Tianfu.SetActive(true);
    }
    public void OnExitBtn()
    {
        if(IsSave==false)
        {
            skill.Clear();
        }
        Tianfu.SetActive(false);
    }
    public void OnSaveBtn()
    {
        IsSave = true;
    }
    public void OnSkillBtn0()
    {
        int t = 0;
        skill.Add(t);
        ChangeColorToBright(t);
        if (skill.Contains( (t+5)%10) )
        {
            skill.Remove((t + 5) % 10);
            ChangeColorToDark((t + 5) % 10);
        }
    }
    public void OnSkillBtn1()
    {
        int t = 1;
        skill.Add(t);
        ChangeColorToBright(t);
        if (skill.Contains((t + 5) % 10))
        {
            skill.Remove((t + 5) % 10);
            ChangeColorToDark((t + 5) % 10);
        }
    }
    public void OnSkillBtn2()
    {
        int t = 2;
        skill.Add(t);
        ChangeColorToBright(t);
        if (skill.Contains((t + 5) % 10))
        {
            skill.Remove((t + 5) % 10);
            ChangeColorToDark((t + 5) % 10);
        }
    }
    public void OnSkillBtn3()
    {
        int t = 3;
        skill.Add(t);
        ChangeColorToBright(t);
        if (skill.Contains((t + 5) % 10))
        {
            skill.Remove((t + 5) % 10);
            ChangeColorToDark((t + 5) % 10);
        }
    }
    public void OnSkillBtn4()
    {
        int t = 4;
        skill.Add(t);
        ChangeColorToBright(t);
        if (skill.Contains((t + 5) % 10))
        {
            skill.Remove((t + 5) % 10);
            ChangeColorToDark((t + 5) % 10);
        }
    }
    public void OnSkillBtn5()
    {
        int t = 5;
        skill.Add(t);
        ChangeColorToBright(t);
        if (skill.Contains((t + 5) % 10))
        {
            skill.Remove((t + 5) % 10);
            ChangeColorToDark((t + 5) % 10);
        }
    }
    public void OnSkillBtn6()
    {
        int t = 6;
        skill.Add(t);
        ChangeColorToBright(t);
        if (skill.Contains((t + 5) % 10))
        {
            skill.Remove((t + 5) % 10);
            ChangeColorToDark((t + 5) % 10);
        }
    }
    public void OnSkillBtn7()
    {
        int t = 7;
        skill.Add(t);
        ChangeColorToBright(t);
        if (skill.Contains((t + 5) % 10))
        {
            skill.Remove((t + 5) % 10);
            ChangeColorToDark((t + 5) % 10);
        }
    }
    public void OnSkillBtn8()
    {
        int t = 8;
        skill.Add(t);
        ChangeColorToBright(t);
        if (skill.Contains((t + 5) % 10))
        {
            skill.Remove((t + 5) % 10);
            ChangeColorToDark((t + 5) % 10);
        }
    }
    public void OnSkillBtn9()
    {
        int t = 9;
        skill.Add(t);
        ChangeColorToBright(t);
        if (skill.Contains((t + 5) % 10))
        {
            skill.Remove((t + 5) % 10);
            ChangeColorToDark((t + 5) % 10);
        }
    }

    void FlipColor(int index)
    {
        Color now =skillButton[index].GetComponent<Image>().color;
        if (now.a <= 0.4) now.a = 1;
        else now.a = 80.0f / 255;
        skillButton[index].GetComponent<Image>().color = now;
    }
    void ChangeColorToDark(int index)
    {
        Color now = skillButton[index].GetComponent<Image>().color;
        now.a = 80.0f / 255;
        skillButton[index].GetComponent<Image>().color = now;
    }
    void ChangeColorToBright(int index)
    {
        Color now = skillButton[index].GetComponent<Image>().color;
        now.a = 1;
        skillButton[index].GetComponent<Image>().color = now;
    }
}
