using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceControl : MonoBehaviour
{
    FixVector2 pos;
    FixVector2 dir;
    Fix64 radius;
    BattleManager _parentManager;
    int roomID;
    int damage;
    int userID;
    int cnt;

    public void init(FixVector2 Pos, FixVector2 Dir, Fix64 Radius, int roomid, int Damage, int userid, BattleManager p)
    {
        cnt = 0;
        pos = Pos;
        dir = Dir;
        _parentManager = p;
        roomID = roomid;
        damage = Damage;
        radius = Radius;
        userID = userid;
        if (Dir.x >= Fix64.Zero)
        {
            this.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan((float)(dir.y / dir.x)) * 180 / Mathf.PI);
        }
        else
        {
            this.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan((float)(dir.y / dir.x)) * 180 / Mathf.PI + 180);
        }
    }

    public bool updateLogic(int frame)
    {
        cnt++;
        if(cnt==200/Global.FrameRate)
        {
            cnt = 0;
            SkillBase p = new SkillBase(0,damage,pos,radius,200/Global.FrameRate,frame+1,userID);
            _parentManager.sys._battle._skill.Add(p,roomID);
        }

        pos += dir;
        this.gameObject.transform.position = new Vector3((float)pos.x, (float)pos.y, 0);

        Polygon polygon = new Polygon(PolygonType.Circle);
        polygon.InitCircle(pos, (Fix64)0.1f);

        if (!_parentManager._terrain.IsMovable(polygon, roomID))
        {
            SkillBase tmp = new SkillBase(0, damage, pos, radius, 0, frame + 1, userID);
            _parentManager._skill.Add(tmp, roomID);
            return true;
        }
        return false;
    }
}
