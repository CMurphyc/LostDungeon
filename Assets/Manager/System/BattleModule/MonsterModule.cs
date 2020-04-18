using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLiveFrame
{
    public int RemainingFrame;
    public GameObject obj;


}
public class MonsterModule
{
    BattleManager _parentManager;

    //房间号-怪物列表
    public Dictionary<int, List<GameObject>> RoomToMonster = new Dictionary<int, List<GameObject>>();


    private Dictionary<GameObject, int> RemoveCounter = new Dictionary<GameObject, int>();

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
                        Monster.GetComponent<AIDestinationSetter>().Target = new Vector3((float)Target.GetComponent<PlayerModel_Component>().GetPlayerPosition().x,
                            (float)Target.GetComponent<PlayerModel_Component>().GetPlayerPosition().y, (float)0);
                        Monster.GetComponent<AIDestinationSetter>().AI_Switch = true;
                        Monster.GetComponent<EnemyAI>().InitMonster(frame);
                    }
                    else
                    {
                        Monster.GetComponent<AIDestinationSetter>().AI_Switch = false;
                    }
                }
            }
        }

        //销毁
        List<GameObject> tempTrash = new List<GameObject>();


        List<MonsterLiveFrame> LiveMonster = new List<MonsterLiveFrame>();


        foreach(var item in RemoveCounter)
        {
            int LeftFrame = item.Value - 1;
            if (LeftFrame > 0)
            {
                MonsterLiveFrame temp= new MonsterLiveFrame();
                temp.obj = item.Key;
                temp.RemainingFrame = LeftFrame;
                LiveMonster.Add(temp);
            }
            else
            {
                tempTrash.Add(item.Key);
            }
        }

        for(int i = 0; i < LiveMonster.Count;i++)
        {
            RemoveCounter[LiveMonster[i].obj] = LiveMonster[i].RemainingFrame;
        }


        for (int i = 0; i < tempTrash.Count;i++)
        {
            if (RemoveCounter.ContainsKey(tempTrash[i]))
            {
                Object.Destroy(tempTrash[i]);
                RemoveCounter.Remove(tempTrash[i]);
            }
        }
        tempTrash.RemoveAll(it=> tempTrash.Contains(it));
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


    //obj = 受击OBJECT , dmg = 伤害
    public void BeAttacked(GameObject obj, float dmg, int roomid)
    {
        Fix64 hp = obj.GetComponent<MonsterModel_Component>().HP - (Fix64)dmg;
        if (hp> Fix64.Zero)
        {
            obj.GetComponent<MonsterModel_Component>().HP = hp;
        }
        else
        {
            obj.GetComponent<MonsterModel_Component>().HP = Fix64.Zero;

            if (RoomToMonster[roomid].Contains(obj))
            {
                RoomToMonster[roomid].Remove(obj);

                int LeftFrameFromDestory = obj.GetComponent<MonsterModel_Component>().FrameLeftFromDestroy;
                RemoveCounter.Add(obj, LeftFrameFromDestory);
            }

        }
        //Debug.Log("MONSTER HP: "+ obj.GetComponent<MonsterModel_Component>().HP);
    }


    public int GetMonsterNumber( int roomID)
    {
        int ret = -1;
        if (RoomToMonster.ContainsKey(roomID))
        {
            return RoomToMonster[roomID].Count;

        }
        return ret;
    }

}
