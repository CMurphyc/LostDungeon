using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterPack
{
    public int RemainingFrame;
    public int RoomID;
}

public class MonsterEventFrame
{
    public int RemainingFrame;
    public GameObject obj;
}


public class MonsterModule
{
    BattleManager _parentManager;

    //房间号-怪物列表
    public Dictionary<int, List<GameObject>> RoomToMonster = new Dictionary<int, List<GameObject>>();
    public int BossRoom;

    //死亡延迟销毁尸体
    private Dictionary<GameObject, int> RemoveCounter = new Dictionary<GameObject, int>();
    //死亡延迟从容器剔除
    private Dictionary<GameObject, MonsterPack> RemoveWaitFrame = new Dictionary<GameObject, MonsterPack>();
    //受击状态
    private Dictionary<GameObject, int> StatusCounter = new Dictionary<GameObject, int>();
    //强制怪物移动
    public Dictionary<GameObject, int> BossMove = new Dictionary<GameObject, int>();


 
    //Boss子弹
    public List<BulletUnion> bulletList = new List<BulletUnion>();
    //Boss子弹触发帧
    public Dictionary<int, List<FakeBulletUnion>> bulletEvent = new Dictionary<int, List<FakeBulletUnion>>();


    private Color Attacked = new Color(255f/255f, 93f / 255f, 93f / 255f);
    private Color Normal = new Color(255f / 255f, 255f / 255f, 255f / 255f);
    
    public MonsterModule(BattleManager parent)
    {
        _parentManager = parent;
      
    }


    public void UpdateLogic(int frame)
    {
        MonsterAILogic(frame);
        MonsterBeAttackHandler(frame);
        MonsterDeadHandler();
        UpdateBullet(frame);
    }
    void UpdateBullet(int frame)
    {
        if (bulletEvent.ContainsKey(frame))
        {
            for (int i = 0; i < bulletEvent[frame].Count; i++)
            {
                FakeBulletUnion temp = bulletEvent[frame][i];
                BulletUnion bu = new BulletUnion(_parentManager);
                bu.BulletInit(temp.tag,temp.anchor,temp.toward,temp.speed,temp.damage,temp.roomid,temp.bulletPrefab,temp.itemList);
                bulletList.Add(bu);
            }
            bulletEvent.Remove(frame);
        }
        foreach (var it in bulletList)
        {
            it.LogicUpdate();
            it.ViewUpdate();
        }
    }
    void MonsterBeAttackHandler(int frame)
    {
        List<GameObject> UnderAttackList = new List<GameObject>();
        List<MonsterEventFrame> LiveEvent = new List<MonsterEventFrame>();


        foreach (var item in StatusCounter)
        {
            int LeftFrame = item.Value - 1;
            if (LeftFrame > 0)
            {
                MonsterEventFrame temp = new MonsterEventFrame();
                temp.obj = item.Key;
                temp.RemainingFrame = LeftFrame;
                LiveEvent.Add(temp);
            }
            else
            {
                UnderAttackList.Add(item.Key);
                item.Key.GetComponent<MonsterModel_Component>().UnderAttack = false;
            }
        }

        for (int i = 0; i < LiveEvent.Count; i++)
        {
            StatusCounter[LiveEvent[i].obj] = LiveEvent[i].RemainingFrame;
        }

        for (int i = 0; i < UnderAttackList.Count; i++)
        {
            if (StatusCounter.ContainsKey(UnderAttackList[i]))
            {
                StatusCounter.Remove(UnderAttackList[i]);
            }
        }
        UnderAttackList.RemoveAll(it => UnderAttackList.Contains(it));
        LiveEvent.RemoveAll(it => LiveEvent.Contains(it));
       

    }


    //obj = 受击OBJECT , dmg = 伤害
    public void BeAttacked(GameObject obj, float dmg, int roomid)
    {
      
       
        int AttackedTime = 10;
        Fix64 hp = obj.GetComponent<MonsterModel_Component>().HP - (Fix64)dmg;
        if (hp > Fix64.Zero)
        {
            obj.GetComponent<MonsterModel_Component>().HP = hp;

            //怪物受击状态时间
            if (!StatusCounter.ContainsKey(obj))
            {
                StatusCounter.Add(obj, AttackedTime);
            }
            else
            {
                StatusCounter[obj] = AttackedTime;

            }

        }
        else
        {
            obj.GetComponent<MonsterModel_Component>().HP = Fix64.Zero;

            if (RoomToMonster[roomid].Contains(obj))
            {
                MonsterPack temp = new MonsterPack();
                temp.RemainingFrame = 2;
                temp.RoomID = roomid;

                RemoveWaitFrame.Add(obj, temp);
                //RoomToMonster[roomid].Remove(obj);
                int LeftFrameFromDestory = obj.GetComponent<MonsterModel_Component>().FrameLeftFromDestroy;
                RemoveCounter.Add(obj, LeftFrameFromDestory);



                if (AttackedTime <= LeftFrameFromDestory)
                {
                    //怪物受击状态时间
                    if (!StatusCounter.ContainsKey(obj))
                    {
                        StatusCounter.Add(obj, AttackedTime);
                    }
                    else
                    {
                        StatusCounter[obj] = AttackedTime;

                    }
                }
                else
                {
                    if (!StatusCounter.ContainsKey(obj))
                    {
                        StatusCounter.Add(obj, LeftFrameFromDestory);
                    }
                    else
                    {
                        StatusCounter[obj] = LeftFrameFromDestory;

                    }

                }


            }

        }

        obj.GetComponent<MonsterModel_Component>().UnderAttack = true;

        Debug.Log("MONSTER HP: " + obj.GetComponent<MonsterModel_Component>().HP);
                
              




       
    }
    void MonsterAILogic(int frame)
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

                    //Debug.Log("HP:　"+ Monster.GetComponent<MonsterModel_Component>().HP);

                    Monster.GetComponent<EnemyAI>().UpdateLogic(Target, frame, Monster);
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

        //销毁List缓存
        List<GameObject> tempRemove = new List<GameObject>();
        //备份更新
        List<MonsterEventFrame> LiveEvent = new List<MonsterEventFrame>();
        //Boss 位移
        foreach (var item in BossMove)
        {
            int LeftFrame = item.Value - 1;
            if (LeftFrame>0)
            {
                MonsterEventFrame temp = new MonsterEventFrame();
                temp.obj = item.Key;
                temp.RemainingFrame = LeftFrame;
                LiveEvent.Add(temp);
                GameObject Boss = item.Key;
                Vector3 MonsterPos = new Vector3((float)Boss.GetComponent<MonsterModel_Component>().position.x, (float)Boss.GetComponent<MonsterModel_Component>().position.y);
                Boss.GetComponent<AIPath>().InitConfig(MonsterPos, Boss.GetComponent<MonsterModel_Component>().Rotation, new Vector3(1.5f, 1.5f, 1.5f), Global.FrameRate);
                //获取当前帧位置
                Vector3 Pos;
                Quaternion Rot;
                Boss.GetComponent<AIPath>().GetFramePosAndRotation(out Pos, out Rot);

                FixVector3 FixMonsterPos = new FixVector3((Fix64)Pos.x, (Fix64)Pos.y, (Fix64)Pos.z);
                Boss.GetComponent<MonsterModel_Component>().position = FixMonsterPos;
                Boss.GetComponent<MonsterModel_Component>().Rotation = Rot;
            }
            else
            {
                tempRemove.Add(item.Key);
            }
        }

        for (int i = 0; i < LiveEvent.Count; i++)
        {
            if (BossMove.ContainsKey(LiveEvent[i].obj))
            {
                BossMove[LiveEvent[i].obj] = LiveEvent[i].RemainingFrame;
            }
        }


        for (int i = 0; i < tempRemove.Count;i++)
        {
            if (BossMove.ContainsKey(tempRemove[i]))
            {
                BossMove.Remove(tempRemove[i]);
            }
        }
        tempRemove.RemoveAll(it => tempRemove.Contains(it));
    }

    void MonsterDeadHandler()
    {

        //销毁
        List<GameObject> tempTrash2 = new List<GameObject>();
        List<MonsterEventFrame> LiveMonster2 = new List<MonsterEventFrame>();

        foreach (var item in RemoveWaitFrame)
        {
            int LeftFrame = item.Value.RemainingFrame - 1;
            if (LeftFrame > 0)
            {
                MonsterEventFrame temp = new MonsterEventFrame();
                temp.obj = item.Key;
                temp.RemainingFrame = LeftFrame;
                LiveMonster2.Add(temp);
            }
            else
            {
                tempTrash2.Add(item.Key);
            }
        }
        for (int i = 0; i < LiveMonster2.Count; i++)
        {
            RemoveWaitFrame[LiveMonster2[i].obj].RemainingFrame = LiveMonster2[i].RemainingFrame;
        }
        for (int i = 0; i < tempTrash2.Count; i++)
        {
            if (RemoveWaitFrame.ContainsKey(tempTrash2[i]))
            {
               
                if (RoomToMonster[RemoveWaitFrame[tempTrash2[i]].RoomID].Contains(tempTrash2[i]))
                {
                    RoomToMonster[RemoveWaitFrame[tempTrash2[i]].RoomID].Remove(tempTrash2[i]);
                 }
                RemoveWaitFrame.Remove(tempTrash2[i]);

            }
        }
        tempTrash2.RemoveAll(it => tempTrash2.Contains(it));






        //销毁
        List<GameObject> tempTrash = new List<GameObject>();
        List<MonsterEventFrame> LiveMonster = new List<MonsterEventFrame>();


        foreach (var item in RemoveCounter)
        {
            int LeftFrame = item.Value - 1;
            if (LeftFrame > 0)
            {
                MonsterEventFrame temp = new MonsterEventFrame();
                temp.obj = item.Key;
                temp.RemainingFrame = LeftFrame;
                LiveMonster.Add(temp);
            }
            else
            {
                tempTrash.Add(item.Key);
            }
        }

        for (int i = 0; i < LiveMonster.Count; i++)
        {
            RemoveCounter[LiveMonster[i].obj] = LiveMonster[i].RemainingFrame;
        }


        for (int i = 0; i < tempTrash.Count; i++)
        {
            if (RemoveCounter.ContainsKey(tempTrash[i]))
            {
                Object.Destroy(tempTrash[i]);
                RemoveCounter.Remove(tempTrash[i]);

            }
        }
        tempTrash.RemoveAll(it => tempTrash.Contains(it));
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

                    Monster.GetComponent<EnemyAI>().UpdateView(Monster);
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
