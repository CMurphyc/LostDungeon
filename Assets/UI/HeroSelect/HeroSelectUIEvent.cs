﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectUIEvent : MonoBehaviour
{
    GameObject main;
    public Sprite[] HeroImageArray;

    //private GameObject HeroPanel;
    public GameObject HeroPanel; 
    private Image HeroImage;
    private CharacterType which;
    private Text HeroName;
    private Text HeroDescription;
    private Text HeroSkills;

    // Start is called before the first frame update

    void Awake()
    {
        main = GameObject.FindWithTag("GameEntry");
        //HeroPanel = transform.Find("Hero").gameObject;
        HeroImage = HeroPanel.transform.Find("HeroImage").GetComponent<Image>();
        HeroName = HeroPanel.transform.Find("Text0").GetComponent<Text>();
        HeroDescription = HeroPanel.transform.Find("Text1").GetComponent<Text>();
        HeroSkills = HeroPanel.transform.Find("Text2").GetComponent<Text>();

        transform.GetComponent<Button>().onClick.AddListener(OnBtnHero);
        HeroPanel.transform.Find("btnEngineer").GetComponent<Toggle>().onValueChanged.AddListener(OnHeroEngineer);
        HeroPanel.transform.Find("btnWarrior").GetComponent<Toggle>().onValueChanged.AddListener(OnHeroWarrior);
        HeroPanel.transform.Find("btnMagician").GetComponent<Toggle>().onValueChanged.AddListener(OnHeroMagician);

        HeroPanel.transform.Find("btnGhost").GetComponent<Toggle>().onValueChanged.AddListener(OnHeroGhost);
        HeroPanel.transform.Find("YES").GetComponent<Button>().onClick.AddListener(OnBtnYES);
        HeroPanel.transform.Find("Exit").GetComponent<Button>().onClick.AddListener(OnBtnExitHeroPanel);

        HeroImage.sprite = HeroImageArray[0];
        which = CharacterType.Enginner;
        HeroName.text = "Engineer";
        HeroDescription.text = "HP:100 \n\nAttack:10 \n\nMoveSpeed:0.8 \n\nAttackSpeed:2.5 ";
        HeroSkills.text = "ZLP50 Self-propelled Fort:Unleash a turret and be able to automatically aim at the enemy.\n\nFD71 Missile:Release a missile, causing damage and slowing down the enemy";
        
    }
    void Start()
    {
        
    }
    public void OnHeroEngineer(bool aaa)
    {
        which = CharacterType.Enginner;
        HeroImage.sprite = HeroImageArray[0];
        HeroName.text = "Engineer";
        HeroDescription.text = "HP:100 \n\nAttack:10 \n\nMoveSpeed:0.8 \n\nAttackSpeed:2.5 ";
        HeroSkills.text = "ZLP50 Self-propelled Fort:Unleash a turret and be able to automatically aim at the enemy.\n\nFD71 Missile:Release a missile, causing damage and slowing down the enemy";
        //main.GetComponent<GameMain>().socket.sock_c2s.ChangeCharacter(CharacterType.Enginner);
    }
    public void OnHeroGhost(bool aaa)
    {
        which = CharacterType.Ghost;
        HeroImage.sprite = HeroImageArray[3];
        HeroName.text = "Ghost";
        HeroDescription.text = "HP:80 \n\nAttack:20 \n\nMoveSpeed:0.8 \n\nAttackSpeed:3 ";
        HeroSkills.text = "Shadow Dance:Dash to the Target Position\n\nSilver Bullet:Increasing Attack Damage in a short period";
        //main.GetComponent<GameMain>().socket.sock_c2s.ChangeCharacter(CharacterType.Enginner);
    }
    public void OnHeroWarrior(bool aaa)
    {
        which = CharacterType.Warrior;
        HeroImage.sprite = HeroImageArray[1];
        HeroName.text = "Warrior";
        HeroDescription.text = "HP:150 \n\nAttack:10 \n\nMoveSpeed:0.4 \n\nAttackSpeed:2 ";
        HeroSkills.text = "Heimdall's Guardian: Blocking enemy skills facing you.\n\nSkadi's Grave: do much damages enemies facing in the direction.";
        //main.GetComponent<GameMain>().socket.sock_c2s.ChangeCharacter(CharacterType.Warrior);
        

    }

    public void OnHeroMagician(bool aaa)
    {
        which = CharacterType.Magician;
        HeroImage.sprite = HeroImageArray[2];
        HeroName.text = "Magician";
        HeroDescription.text = "HP:100 \n\nAttack:10 \n\nMoveSpeed:0.4 \n\nAttackSpeed:3 ";
        HeroSkills.text = "Sigh of Jortonheim: Releasing ice on an area will freeze the enemy.\n\nShirtel's Anger: releasing fire on an area.";
        //main.GetComponent<GameMain>().socket.sock_c2s.ChangeCharacter(CharacterType.Magician);
    }
    public void OnBtnHero()
    {
        if (HeroPanel.activeSelf == false)
        {
            HeroPanel.SetActive(true);
            which = CharacterType.Enginner;
        }
        else
        {
            HeroPanel.SetActive(false);
        }
    }
    public void OnBtnExitHeroPanel()
    {
        HeroPanel.SetActive(false);
    }
    public void OnBtnYES()
    {
        if (which != CharacterType.None)
        {
            main.GetComponent<GameMain>().socket.sock_c2s.ChangeCharacter(which);
        }
        HeroPanel.SetActive(false);
    }
}
