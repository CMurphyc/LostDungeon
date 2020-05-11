using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillModule
{
    public BattleManager _parentManager;

    //房间号-抛射物
    public Dictionary<int, List<SkillBase>> RoomToProjectile = new Dictionary<int, List<SkillBase>>();

    //特效-持续帧数
    public List<KeyValuePair<GameObject, int>> Effects = new List<KeyValuePair<GameObject, int>>();

    public EngineerBase enginerBase;
    public MagicianBase magicianBase;

    public GhostBase ghostBase;


    public GuardianBase guardianBase;

    public SkillModule(BattleManager parent)
    {

        _parentManager = parent;

        enginerBase = new EngineerBase(parent.sys);
        magicianBase = new MagicianBase(parent.sys);


        ghostBase = new GhostBase(parent.sys);

        guardianBase = new GuardianBase(parent.sys);


    }
    public void Free()
    {
        RoomToProjectile.Clear();
        Effects.Clear();
        enginerBase.Free();
        magicianBase.Free();


        ghostBase.Free();

        guardianBase.Free();

    }
    public void Add(SkillBase x, int roomID)
    {
        if (!RoomToProjectile.ContainsKey(roomID))
        {
            RoomToProjectile.Add(roomID, new List<SkillBase>());
        }

        RoomToProjectile[roomID].Add(x);
    }

    public void UpdateLogic(int frame)
    {
        foreach (var p in RoomToProjectile)
        {
            List<SkillBase> t = new List<SkillBase>();
            foreach (SkillBase x in p.Value)
            {
                switch (x.tag)
                {
                    case 0:
                        break;
                    case 1:
                        foreach (var pler in _parentManager._player.FindPlayerInRoom(p.Key))
                        {
                            FixVector2 plerpos = pler.obj.transform.GetComponent<PlayerModel_Component>().GetPlayerPosition();
                            Fix64 dist = FixVector2.Distance(new FixVector2(plerpos.x, plerpos.y), x.center);
                            if (dist <= x.radius)
                            {
                            }
                        }
                        break;
                }
                if (x.frame == frame)
                {
                    List<GameObject> btt = new List<GameObject>();
                    foreach (var mon in _parentManager._monster.RoomToMonster[p.Key])
                    {
                        Fix64 dist = FixVector2.Distance(new FixVector2(mon.GetComponent<MonsterModel_Component>().position.x,
                            mon.GetComponent<MonsterModel_Component>().position.y), x.center
                            );

                        if (dist <= x.radius)
                        {
                            btt.Add(mon);

                        }
                    }
                    //destroy skillbase
                    foreach (var mon in btt)
                    {
                        Debug.Log("damage:" + x.damage);
                        _parentManager._monster.BeAttacked(mon, x.damage, p.Key,x.DmgSrcPlayerUID);
                        Debug.LogError("11111111111111111111");
                    }
                    btt.Clear();

                }
                else
                {
                    t.Add(x);
                }
            }
            p.Value.Clear();
            foreach (SkillBase x in t)
            {
                p.Value.Add(x);
            }
            t.Clear();
        }
        enginerBase.updateLogic(frame);
        magicianBase.updateLogic(frame);

        ghostBase.updateLogic(frame);

        guardianBase.updateLogic(frame);


    }
    public void UpdateView()
    {
        for (int i = Effects.Count - 1; i >= 0; i--)
        {
            Effects[i] = new KeyValuePair<GameObject, int>(Effects[i].Key, Effects[i].Value - 1);
            if (Effects[i].Value == 0)
            {
                Object.Destroy(Effects[i].Key);
                Effects.RemoveAt(i);
            }
        }


    }
}
