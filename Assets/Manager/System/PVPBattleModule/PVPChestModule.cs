using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PVPChestModule
{
    PVPBattleManager _parentManager;

    public Dictionary<int, List<TreasureData>> roomToTreasure;   // 房间号对应宝物列表
    public Dictionary<int, PropData> propToProperty;   // 根据道具名称找到对应道具属性


    public PVPChestModule(PVPBattleManager _parentManager)
    {
        this._parentManager = _parentManager;
        roomToTreasure = new Dictionary<int, List<TreasureData>>();
        propToProperty = new Dictionary<int, PropData>();
    }
    public void Free()
    {
        roomToTreasure.Clear();
        propToProperty.Clear();
    }
    public void UpdateLogic(int Frame)
    {
        foreach(var x in roomToTreasure)
        {
            foreach(var y in x.Value)
            {
                if (y.active == false) continue;
                if(y.frame==Global.FrameRate*30)
                {
                    int treasureId = Frame % propToProperty.Count;
                    GameObject tmp =GameObject.Instantiate(propToProperty[treasureId].propObject, 
                        y.treasureTable.transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);
                    y.Change(treasureId, propToProperty[treasureId].propType, y.treasureTable, tmp, false);
                }
                else
                {
                    y.AddFrame();
                }
            }
        }

    }
    public void UpdateView()
    {
        
        GameObject tobj = GameObject.Find("AttackStickUI");
        bool has = false;
        int PlayerUID = _parentManager.sys._model._PlayerModule.uid;
        foreach (var x in roomToTreasure[_parentManager.sys._pvpbattle._pvpplayer.playerToPlayer[PlayerUID].RoomID])
        {
            if (x.active) continue;
            FixVector2 tmp = new FixVector2((Fix64)x.treasureTable.transform.position.x, (Fix64)x.treasureTable.transform.position.y);
            if (FixVector2.Distance(tmp,
                _parentManager.sys._pvpbattle._pvpplayer.playerToPlayer[PlayerUID].obj.GetComponent<PlayerModel_Component>().GetPlayerPosition()) <= (Fix64)1.4f)
            {
                has = true;
                tobj.GetComponent<VirtualJoystick2>().pickIcon();
                break;
            }
        }
        if (!has)
        {
            tobj.GetComponent<VirtualJoystick2>().attackIcon();
        }
    }

}
