using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterModule
{
    BattleManager _parentManager;

    //房间号-怪物列表
    public Dictionary<int, List<GameObject>> RoomToMonster = new Dictionary<int, List<GameObject>>();

    public MonsterModule(BattleManager parent)
    {
        _parentManager = parent;
    }


    public void UpdateLogic(int frame)
    {
        foreach (int RoomID in _parentManager._player.GetLiveRoom())
        {
            if (RoomToMonster.ContainsKey(RoomID))
            {
                List<GameObject> MonsterList = RoomToMonster[RoomID];
                for (int i = 0; i < MonsterList.Count; i++)
                {
                    GameObject Monster = MonsterList[i];

                    Vector2 MonsterPos = new Vector2((float)Monster.GetComponent<MonsterModel_Component>().position.x, (float)Monster.GetComponent<MonsterModel_Component>().position.y);
                    GameObject Target = FindClosePlayer(MonsterPos, RoomID);
                   
                    Monster.GetComponent<EnemyAI>().UpdateLogic(Target, frame);
                    if (Target != null)
                    {
                        Monster.GetComponent<AIDestinationSetter>().Target = new Vector3((float)Target.GetComponent<PlayerModel_Component>().GetPlayerPosition().x, (float)Target.GetComponent<PlayerModel_Component>().GetPlayerPosition().y, (float)Target.GetComponent<PlayerModel_Component>().GetPlayerPosition().z);
                        Monster.GetComponent<AIDestinationSetter>().AI_Switch = true;
                    }
                    else
                    {
                        Monster.GetComponent<AIDestinationSetter>().AI_Switch = false;
                    }
                }
            }
        }
    }

    public void UpdateView()
    {
        foreach (int RoomID in _parentManager._player.GetLiveRoom())
        {
            if (RoomToMonster.ContainsKey(RoomID))
            {
                List<GameObject> MonsterList = RoomToMonster[RoomID];
                for (int i = 0; i < MonsterList.Count; i++)
                {
                    GameObject Monster = MonsterList[i];

                    Monster.GetComponent<EnemyAI>().UpdateView();
                }
            }
        }
    }

    //obj = 受击OBJECT , dmg = 伤害
    public void BeAttacked( GameObject obj,float dmg)
    {


    }


    GameObject FindClosePlayer(Vector2 MonsterPos, int RoomID )
    {
        GameObject ret = null;
        float Min_Distance = 99999;

     
        List<PlayerInGameData> PlayerInRoomList = _parentManager._player.FindPlayerInRoom(RoomID);
        for (int i = 0; i < PlayerInRoomList.Count; i++)
        {
            Vector2 PlayerPos = new Vector2 ((float)PlayerInRoomList[i].obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().x, (float)PlayerInRoomList[i].obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().y);
            float distance = Vector2.Distance(PlayerPos, MonsterPos);
            if (distance< Min_Distance)
            {
                Min_Distance = distance;
                ret = PlayerInRoomList[i].obj;
            }
        }
        return ret;
    }

}
