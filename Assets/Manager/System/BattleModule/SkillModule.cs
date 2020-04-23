using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillModule 
{
    BattleManager _parentManager;

    //房间号-抛射物
    public Dictionary<int, List<SkillBase>> RoomToProjectile = new Dictionary<int, List<SkillBase>>();


    public EngineerBase enginerBase;
    public MagicianBase magicianBase;

    public SkillModule(BattleManager parent)
    {
        
        _parentManager = parent;
        enginerBase = new EngineerBase(parent);
        magicianBase = new MagicianBase(parent);
    }

    public void Add(SkillBase x,int roomID)
    {
        if(!RoomToProjectile.ContainsKey(roomID))
        {
            RoomToProjectile.Add(roomID, new List<SkillBase>());
        }
        
        RoomToProjectile[roomID].Add(x);
    }

    public void UpdateLogic(int frame)
    {
        foreach(var p in RoomToProjectile)
        {
            List<SkillBase> t = new List<SkillBase>();
            foreach(SkillBase x in p.Value)
            {
                if(x.frame==frame)
                {
                    List<GameObject> btt = new List<GameObject>();
                    foreach(var mon in _parentManager._monster.RoomToMonster[p.Key])
                    {
                        Fix64 dist = FixVector2.Distance(new FixVector2(mon.GetComponent<MonsterModel_Component>().position.x,
                            mon.GetComponent<MonsterModel_Component>().position.y),x.center
                            );

                        if(dist<=x.radius)
                        {
                            btt.Add(mon);
                            
                        }
                    }
                    //destroy skillbase
                    foreach(var mon in btt)
                    {
                        Debug.Log("damage:" + x.damage);
                        _parentManager._monster.BeAttacked(mon, x.damage, p.Key);
                    }
                    btt.Clear();

                }
                else
                {
                    t.Add(x);
                }
            }
            p.Value.Clear();
            foreach(SkillBase x in t)
            {
                p.Value.Add(x);
            }
            t.Clear();
        }
        enginerBase.updateLogic(frame);
        magicianBase.updateLogic(frame);
    }
    public void UpdateView()
    {



    }
}
