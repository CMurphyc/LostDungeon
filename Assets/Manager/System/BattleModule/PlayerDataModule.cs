using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataModule
{
    BattleManager _parentManager;

    public Dictionary<int, PlayerInGameData> playerToPlayer = new Dictionary<int, PlayerInGameData>();   // 玩家编号对应玩家信息

    public List<BattleInput> frameInfo;
    public List<BulletUnion> bulletList = new List<BulletUnion>();

    int AttackInterval = 5;

    public PlayerDataModule(BattleManager parent)
    {
        _parentManager = parent;
    }
    public void UpdateLogic(int frame)//更新某一帧逻辑
    {
        for (int i = 0; i < frameInfo.Count;i++)//更新操作
        {
            if (playerToPlayer.ContainsKey(frameInfo[i].Uid))
            {
                PlayerInGameData Input = playerToPlayer[frameInfo[i].Uid];
                Vector2 MoveVec = new Vector2(frameInfo[i].MoveDirectionX/10000f, frameInfo[i].MoveDirectionY/10000f).normalized * Global.FrameRate/1000f*5f ;
                FixVector2 Pos = Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition();
                if (_parentManager._terrain.IsMovable(new FixVector2((Fix64)(MoveVec.x+Pos.x),(Fix64)(MoveVec.y+Pos.y)),Input.RoomID))
                {
                    Input.obj.GetComponent<PlayerModel_Component>().Move( new FixVector2((Fix64)MoveVec.x,(Fix64)MoveVec.y));
                }
                else
                {
                    if (_parentManager._terrain.IsMovable(new FixVector2((Fix64)(MoveVec.x + Pos.x), (Fix64)(Pos.y)), Input.RoomID))
                    {
                        Input.obj.GetComponent<PlayerModel_Component>().Move(new FixVector2((Fix64)MoveVec.x, (Fix64)0));
                    }
                    else if(_parentManager._terrain.IsMovable(new FixVector2((Fix64)(Pos.x), (Fix64)(MoveVec.y + Pos.y)), Input.RoomID))
                    {
                        Input.obj.GetComponent<PlayerModel_Component>().Move(new FixVector2((Fix64)0, (Fix64)MoveVec.y));
                    }
                }


                switch (frameInfo[i].AttackType)
                {
                    case (int)AttackType.BasicAttack:
                        {
                            
                            if ((Mathf.Abs(frameInfo[i].AttackDirectionX / 10000f) >= 0.01f 
                                && Mathf.Abs(frameInfo[i].AttackDirectionY / 10000f) >= 0.01f))
                            {
                                //Debug.Log("aaaaaaaa");
                                if (frame >= Input.NextAttackFrame)
                                {
                                    List<int> list = new List<int>();
                                    BulletUnion bu = new BulletUnion(_parentManager);
                                    bu.BulletInit("Player", new FixVector2((Fix64)Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().x,
                                                                        (Fix64)Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().y),
                                                                        new FixVector2((Fix64)frameInfo[i].AttackDirectionX / 10000f,
                                                                        (Fix64)frameInfo[i].AttackDirectionY / 10000f),
                                                                        (Fix64)0.2, (Fix64)2, Input.RoomID,
                                                                        _parentManager.sys._battle._skill.enginerBase.bulletObj

                                                                        , list);

                                    bulletList.Add(bu);

                                    Input.NextAttackFrame = frame + AttackInterval;

                                    AudioManager.instance.PlayAudio(AudioName.Gunshot1, false);
                                }
                            }
                            break;
                        }
                    case (int)AttackType.Skill1:
                        {
                            //Debug.Log("bbbbbbbbbb");
                            CharacterType PlayerType = _parentManager.sys._model._RoomModule.GetCharacterType(frameInfo[i].Uid);

                            List<int> tmp = new List<int>();
                            tmp.Add(1);
                            tmp.Add(1);
                            tmp.Add(1);
                            tmp.Add(1);
                            tmp.Add(1);

                            switch (PlayerType)
                            {
                                case CharacterType.Enginner:
                                    {
                                        _parentManager._skill.enginerBase.Skill1Logic(frame,
                                            _parentManager._player.playerToPlayer[frameInfo[i].Uid].RoomID, tmp,
                                            new Vector2((float)Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().x,
                                            (float)Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().y),
                                            new Vector2((float)frameInfo[i].AttackDirectionX / 10000f,
                                            (float)frameInfo[i].AttackDirectionY / 10000f
                                            )
                                            );
                                        break;
                                    }
                                case CharacterType.Magician:
                                    {
                                        _parentManager._skill.magicianBase.Skill1Logic(frame,
                                            _parentManager._player.playerToPlayer[frameInfo[i].Uid].RoomID, tmp,
                                            new Vector2((float)frameInfo[i].AttackDirectionX / 10000f,
                                            (float)frameInfo[i].AttackDirectionY / 10000f
                                            )
                                            );
                                        break;
                                    }
                                default:
                                    break;
                            }
                            break;
                        }
                    case (int)AttackType.Skill2:
                        {
                            //Debug.Log("ccccccccc");
                            int PlayerUID = frameInfo[i].Uid;
                            CharacterType PlayerType = _parentManager.sys._model._RoomModule.GetCharacterType(PlayerUID);

                            List<int> tmp = new List<int>();
                            tmp.Add(1);
                            tmp.Add(1);
                            tmp.Add(1);
                            tmp.Add(1);
                            tmp.Add(1);

                            switch (PlayerType)
                            {
                                case CharacterType.Enginner:
                                    {
                                        //Debug.Log(frameInfo[i].AttackDirectionX / 10000f);
                                        //Debug.Log(frameInfo[i].AttackDirectionY / 10000f);
                                        _parentManager._skill.enginerBase.Skill2Logic(frame,
                                            _parentManager._player.playerToPlayer[frameInfo[i].Uid].RoomID, tmp,
                                            new Vector2((float)Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().x,
                                            (float)Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().y),
                                            new Vector2((float)frameInfo[i].AttackDirectionX / 10000f,
                                            (float)frameInfo[i].AttackDirectionY / 10000f
                                            )
                                            );
                                        break;
                                    }
                                case CharacterType.Magician:
                                    {
                                        _parentManager._skill.magicianBase.Skill2Logic(frame,
                                            _parentManager._player.playerToPlayer[frameInfo[i].Uid].RoomID, tmp,
                                            new Vector2((float)frameInfo[i].AttackDirectionX / 10000f,
                                            (float)frameInfo[i].AttackDirectionY / 10000f
                                            )
                                            );
                                        break;
                                    }
                                default:
                                    break;
                            }
                            break;
                        }
                    default:
                        break;


                }
                
                foreach(var it in bulletList)
                {
                    it.LogicUpdate();
                    it.ViewUpdate();
                }

            }
        }

        foreach(KeyValuePair<int,PlayerInGameData> x in playerToPlayer)//buff 
        {

        }
    }

    public void UpdateView()//更新视图
    {
        for (int i = 0; i < frameInfo.Count; i++)
        {
            if (playerToPlayer.ContainsKey(frameInfo[i].Uid))
            {
                PlayerInGameData Input = playerToPlayer[frameInfo[i].Uid];

                Vector2 MoveVec = new Vector2(frameInfo[i].MoveDirectionX / 10000f, frameInfo[i].MoveDirectionY / 10000f);
                Input.obj.GetComponent<PlayerView_Component>().RefreshView();
            }

        }
    }

    public HashSet<int> GetLiveRoom()
    {
        HashSet<int> RoomList  = new HashSet<int>();
        foreach (var item in playerToPlayer)
        {
            //Debug.Log("Live Room Number: " + item.Value.RoomID);

            RoomList.Add(item.Value.RoomID);
        }
        return RoomList;
    }
    public List<PlayerInGameData>  FindPlayerInRoom(int RoomID)
    {
        List<PlayerInGameData> PlayerUIDList = new List<PlayerInGameData>();
        foreach (var item in playerToPlayer)
        {
            if (item.Value.RoomID == RoomID)
                PlayerUIDList.Add(item.Value);
        }

        return PlayerUIDList;
    }

    public GameObject FindPlayerObjByUID(int uid)
    {
        if (playerToPlayer.ContainsKey(uid))
        {
            return playerToPlayer[uid].obj;
        }
        return null;
    }


    public int FindCurrentPlayerUID()
    {
        return _parentManager.sys._model._PlayerModule.uid;
    }
   


}
