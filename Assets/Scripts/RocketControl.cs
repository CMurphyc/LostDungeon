using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketControl : MonoBehaviour
{
    FixVector2 pos;
    FixVector2 dir;
    Fix64 radius;
    BattleManager _parentManager;
    int roomID;
    int damage;
    int userID;

    public void init( FixVector2 Pos,FixVector2 Dir,Fix64 Radius,int roomid,int Damage,int userid,BattleManager p)
    {
        pos = Pos;
        dir = Dir;
        _parentManager = p;
        roomID = roomid;
        damage = Damage;
        radius = Radius;
        userID = userid;
    }

    public bool updateLogic(int frame)
    {
        pos += dir;
        this.gameObject.transform.position = new Vector3((float)pos.x,(float)pos.y,0);

        if(!_parentManager._terrain.IsMovable(pos, roomID))
        {
            SkillBase tmp = new SkillBase(0, damage, pos, radius,0 ,frame+1, userID);
            _parentManager._skill.Add(tmp, roomID);
            return true;
        }
        foreach(var x in _parentManager._monster.RoomToMonster[roomID])
        {
            Fix64 dist = FixVector2.Distance(pos, 
                new FixVector2(x.GetComponent<MonsterModel_Component>().position.x, x.GetComponent<MonsterModel_Component>().position.y));
            if(dist<=(Fix64)0.3f)
            {
                SkillBase tmp = new SkillBase(0, damage, pos, radius, 0, frame + 1, userID);
                _parentManager._skill.Add(tmp, roomID);
                return true;
            }
        }
        return false;
    }

}
