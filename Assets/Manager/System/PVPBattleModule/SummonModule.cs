using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonModule 
{
    PVPBattleManager _pvp;
    public Dictionary<int, List<AliasMonsterPack>> RedTeamRoomToAliasUnit = new Dictionary<int, List<AliasMonsterPack>>();
    public Dictionary<int, List<AliasMonsterPack>> BlueTeamRoomToAliasUnit = new Dictionary<int, List<AliasMonsterPack>>();

    //死亡延迟销毁尸体
    private Dictionary<GameObject, int> AliasRemoveCounter = new Dictionary<GameObject, int>();
    public List<PVPBulletUnion> bulletList = new List<PVPBulletUnion>();
    public SummonModule(PVPBattleManager pvp)
    {
        _pvp = pvp;
    }
    public void Free()
    {
        AliasRemoveCounter.Clear();
        RedTeamRoomToAliasUnit.Clear();
        BlueTeamRoomToAliasUnit.Clear();
        bulletList.Clear();
    }
    public void UpdateLogic(int frame)
    {
        MonsterAILogic(frame);
        MonsterDeadHandler();
        UpdateBullet(frame);
    }
    void UpdateBullet(int frame)
    {
        foreach (var it in bulletList)
        {
            it.LogicUpdate();
            it.ViewUpdate();
        }
    }
    public void MonsterAILogic(int frame)
    {
        foreach (int RoomID in _pvp._pvpplayer.GetLiveRoom())
        {
            if (RedTeamRoomToAliasUnit.ContainsKey(RoomID))
            {
                List<AliasMonsterPack> AliasMonsterList = RedTeamRoomToAliasUnit[RoomID];
                for (int i = 0; i < AliasMonsterList.Count; i++)
                {
                    if (AliasMonsterList[i].RemainingFrame > 0)
                    {
                        AliasMonsterList[i].RemainingFrame--;
                        GameObject AliasUnit = AliasMonsterList[i].obj;
                        Vector2 AliasPos = new Vector2((float)AliasUnit.GetComponent<MonsterModel_Component>().position.x, (float)AliasUnit.GetComponent<MonsterModel_Component>().position.y);
                        GameObject Target = FindCloseBlueTeamPlayer(AliasPos, RoomID);
                       
                        if (Target != null)
                        {
                            AliasUnit.GetComponent<AIDestinationSetter>().Target = new Vector3((float)Target.GetComponent<PlayerModel_Component>().playerPosition.x,
                                (float)Target.GetComponent<PlayerModel_Component>().playerPosition.y, (float)0);
                            AliasUnit.GetComponent<AIDestinationSetter>().AI_Switch = true;
                            AliasUnit.GetComponent<EnemyAI>().InitMonster(frame);
                        }
                        else
                        {
                            AliasUnit.GetComponent<AIDestinationSetter>().AI_Switch = false;
                        }
                        AliasUnit.GetComponent<EnemyAI>().UpdateLogic(Target, frame, AliasUnit, true);
                    }
                    else
                    {
                        //销毁
                        GameObject AliasUnit = AliasMonsterList[i].obj;
                        AliasUnit.GetComponent<MonsterModel_Component>().HP = Fix64.Zero;
                        if (RedTeamRoomToAliasUnit[RoomID].Contains(AliasMonsterList[i]))
                        {
                            //MonsterPack temp = new MonsterPack();
                            //temp.RemainingFrame = 2;
                            //temp.RoomID = RoomID;

                            //AliasRemoveWaitFrame.Add(AliasUnit, temp);

                            RedTeamRoomToAliasUnit[RoomID].Remove(AliasMonsterList[i]);
                            int LeftFrameFromDestory = AliasUnit.GetComponent<MonsterModel_Component>().FrameLeftFromDestroy;
                            AliasRemoveCounter.Add(AliasUnit, LeftFrameFromDestory);

                        }
                    }
                }
            }
            if (BlueTeamRoomToAliasUnit.ContainsKey(RoomID))
            {
                List<AliasMonsterPack> AliasMonsterList = BlueTeamRoomToAliasUnit[RoomID];
                for (int i = 0; i < AliasMonsterList.Count; i++)
                {
                    if (AliasMonsterList[i].RemainingFrame > 0)
                    {
                        AliasMonsterList[i].RemainingFrame--;
                        GameObject AliasUnit = AliasMonsterList[i].obj;
                        Vector2 AliasPos = new Vector2((float)AliasUnit.GetComponent<MonsterModel_Component>().position.x, (float)AliasUnit.GetComponent<MonsterModel_Component>().position.y);
                        GameObject Target = FindCloseRedTeamPlayer(AliasPos, RoomID);
                      
                        if (Target != null)
                        {
                            AliasUnit.GetComponent<AIDestinationSetter>().Target = new Vector3((float)Target.GetComponent<PlayerModel_Component>().playerPosition.x,
                                (float)Target.GetComponent<PlayerModel_Component>().playerPosition.y, (float)0);
                            AliasUnit.GetComponent<AIDestinationSetter>().AI_Switch = true;
                            AliasUnit.GetComponent<EnemyAI>().InitMonster(frame);
                        }
                        else
                        {
                            AliasUnit.GetComponent<AIDestinationSetter>().AI_Switch = false;
                        }
                        AliasUnit.GetComponent<EnemyAI>().UpdateLogic(Target, frame, AliasUnit, true);
                    }
                    else
                    {
                        //销毁
                        GameObject AliasUnit = AliasMonsterList[i].obj;
                        AliasUnit.GetComponent<MonsterModel_Component>().HP = Fix64.Zero;
                        if (BlueTeamRoomToAliasUnit[RoomID].Contains(AliasMonsterList[i]))
                        {
                            //MonsterPack temp = new MonsterPack();
                            //temp.RemainingFrame = 2;
                            //temp.RoomID = RoomID;

                            //AliasRemoveWaitFrame.Add(AliasUnit, temp);

                            BlueTeamRoomToAliasUnit[RoomID].Remove(AliasMonsterList[i]);
                            int LeftFrameFromDestory = AliasUnit.GetComponent<MonsterModel_Component>().FrameLeftFromDestroy;
                            AliasRemoveCounter.Add(AliasUnit, LeftFrameFromDestory);

                        }
                    }
                }
            }
        }
    }

    public void UpdateView()
    {

    }

    public void AddAliasUnit(int OwnerUID, int RoomID, AliasMonsterPack temp)
    {
        switch (_pvp._pvpplayer.FindPlayerTeamByUID(OwnerUID))
        {
            case "RedTeam":
                if (!RedTeamRoomToAliasUnit.ContainsKey(RoomID))
                {
                    List<AliasMonsterPack> ListAlias = new List<AliasMonsterPack>();
                    ListAlias.Add(temp);
                    RedTeamRoomToAliasUnit.Add(RoomID, ListAlias);
                }
                else
                {
                    RedTeamRoomToAliasUnit[RoomID].Add(temp);
                }
                break;
            case "BlueTeam":
                Debug.LogError("??????");
                if (!BlueTeamRoomToAliasUnit.ContainsKey(RoomID))
                {
                    List<AliasMonsterPack> ListAlias = new List<AliasMonsterPack>();
                    ListAlias.Add(temp);
                    BlueTeamRoomToAliasUnit.Add(RoomID, ListAlias);
                }
                else
                {
                    BlueTeamRoomToAliasUnit[RoomID].Add(temp);
                }
                break;
        }
    }

    void MonsterDeadHandler()
    {
        List<GameObject> tempTrash4 = new List<GameObject>();
        List<MonsterEventFrame> LiveMonster4 = new List<MonsterEventFrame>();


        foreach (var item in AliasRemoveCounter)
        {
            int LeftFrame = item.Value - 1;
            if (LeftFrame > 0)
            {
                MonsterEventFrame temp = new MonsterEventFrame();
                temp.obj = item.Key;
                temp.RemainingFrame = LeftFrame;
                LiveMonster4.Add(temp);
            }
            else
            {
                tempTrash4.Add(item.Key);
            }
        }

        for (int i = 0; i < LiveMonster4.Count; i++)
        {
            AliasRemoveCounter[LiveMonster4[i].obj] = LiveMonster4[i].RemainingFrame;
        }


        for (int i = 0; i < tempTrash4.Count; i++)
        {
            if (AliasRemoveCounter.ContainsKey(tempTrash4[i]))
            {
                Object.Destroy(tempTrash4[i]);
                AliasRemoveCounter.Remove(tempTrash4[i]);

            }
        }
        tempTrash4.RemoveAll(it => tempTrash4.Contains(it));
    }

    GameObject FindCloseBlueTeamPlayer(Vector2 AliasPos,int RoomID)
    {
        GameObject ret = null;
        Fix64 Min_Distance = (Fix64)99999;


        for (int i = 0; i < _pvp._pvpplayer.BlueTeam.Count; i++)
        {
            //Vector2 PlayerPos = new Vector2 ((float)PlayerInRoomList[i].obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().x, (float)PlayerInRoomList[i].obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().y);
            int uid = _pvp._pvpplayer.BlueTeam[i];
            if(_pvp._pvpplayer.playerToPlayer[uid].RoomID!=RoomID)
            {
                continue;
            }
            if (_pvp._pvpplayer.playerToPlayer[uid].obj.GetComponent<PlayerModel_Component>().dead == 1 || _pvp._pvpplayer.playerToPlayer[uid].obj.GetComponent<PlayerModel_Component>().buff.Invisible)
            {
                continue;
            }
            FixVector2 PlayerPos = _pvp._pvpplayer.playerToPlayer[uid].obj.GetComponent<PlayerModel_Component>().GetPlayerPosition();
            Fix64 distance = FixVector2.Distance(PlayerPos,new FixVector2((Fix64)AliasPos.x,(Fix64)AliasPos.y));
            if (distance < Min_Distance)
            {
                Min_Distance = distance;
                ret = _pvp._pvpplayer.playerToPlayer[uid].obj;
            }
        }
        return ret;
    }
    GameObject FindCloseRedTeamPlayer(Vector2 AliasPos, int RoomID)
    {
        GameObject ret = null;
        Fix64 Min_Distance = (Fix64)99999;


        for (int i = 0; i < _pvp._pvpplayer.RedTeam.Count; i++)
        {
            //Vector2 PlayerPos = new Vector2 ((float)PlayerInRoomList[i].obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().x, (float)PlayerInRoomList[i].obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().y);
            int uid = _pvp._pvpplayer.RedTeam[i];
            if (_pvp._pvpplayer.playerToPlayer[uid].RoomID != RoomID)
            {
                continue;
            }
            if (_pvp._pvpplayer.playerToPlayer[uid].obj.GetComponent<PlayerModel_Component>().dead == 1 || _pvp._pvpplayer.playerToPlayer[uid].obj.GetComponent<PlayerModel_Component>().buff.Invisible)
            {
                continue;
            }
            FixVector2 PlayerPos = _pvp._pvpplayer.playerToPlayer[uid].obj.GetComponent<PlayerModel_Component>().GetPlayerPosition();
            Fix64 distance = FixVector2.Distance(PlayerPos, new FixVector2((Fix64)AliasPos.x, (Fix64)AliasPos.y));
            if (distance < Min_Distance)
            {
                Min_Distance = distance;
                ret = _pvp._pvpplayer.playerToPlayer[uid].obj;
            }
        }
        return ret;
    }
}
