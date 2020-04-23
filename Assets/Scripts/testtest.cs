using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testtest : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject a;
    GameObject p;
    int x = 0;
    MagicianBase eb;
    List<int> tl = new List<int>();

    public int did = 0;
    public float xx,yy;
    GameObject tss2, en,tss1;

    BattleManager _parentManager;

    void Start()
    {
        eb = new MagicianBase(_parentManager);
        tl.Add(1);
        tl.Add(1);
        tl.Add(1);
        tl.Add(1);
        tl.Add(1);
        

         tss2 = GameObject.Find("SkillStickUI2");
         tss1 = GameObject.Find("SkillStickUI1");
         en = GameObject.Find("Magician");

        tss1.GetComponent<SkillIndiactor>().Init(2,0.5f,en);//4 2
        tss2.GetComponent<SkillIndiactor>().Init(2,0.5f,en);//4 2

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        x++;
        eb.updateLogic(x);

        if (did != 0)
        {
            if (did == 1)
            {
                eb.Skill1Logic(x, 1, tl,new Vector3(xx, yy, 0));
                did = 0;
            }
            else
            {
                eb.Skill2Logic(x, 1, tl,new Vector3(xx, yy, 0));
                did = 0;
            }
        }
    }
}
