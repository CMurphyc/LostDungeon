using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPSkillModule : MonoBehaviour
{
    PVPBattleManager _pvp;

    //房间号-抛射物
    public Dictionary<int, List<SkillBase>> RoomToProjectile = new Dictionary<int, List<SkillBase>>();

    //特效-持续帧数
    public List<KeyValuePair<GameObject, int>> Effects = new List<KeyValuePair<GameObject, int>>();

    public EngineerBase enginerBase;
    public MagicianBase magicianBase;

    public PVPSkillModule(PVPBattleManager parent)
    {

        _pvp = parent;
        enginerBase = new EngineerBase(parent.sys);
        magicianBase = new MagicianBase(parent.sys);
    }
    public void Free()
    {
        RoomToProjectile.Clear();
        Effects.Clear();
        enginerBase.Free();
        magicianBase.Free();
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
                if (x.frame == frame)
                {
                    List<GameObject> btt = new List<GameObject>();
                    foreach (var pler in _pvp._pvpplayer.FindPlayerInRoom(p.Key))
                    {
                        if(_pvp._pvpplayer.FindPlayerTeamByUID(x.DmgSrcPlayerUID)== _pvp._pvpplayer.FindPlayerTeamByGameObject(pler.obj))
                        {
                            //Debug.LogError(x.DmgSrcPlayerUID+"  "+ _pvp._pvpplayer.FindPlayerUIDbyObject(pler.obj));
                            continue;
                        }
                        Fix64 dist = FixVector2.Distance(new FixVector2(pler.obj.GetComponent<PlayerModel_Component>().playerPosition.x,
                            pler.obj.GetComponent<PlayerModel_Component>().playerPosition.y), x.center
                            );

                        if (dist <= x.radius)
                        {
                            btt.Add(pler.obj);

                        }
                    }
                    //destroy skillbase
                    foreach (var pler in btt)
                    {
                        Debug.Log("damage:" + x.damage);
                        _pvp._pvpplayer.BeAttacked(x.DmgSrcPlayerUID,pler, x.damage, p.Key);
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
