﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataModule
{
    BattleManager _parentManager;

    public Dictionary<int, PlayerInGameData> playerToPlayer = new Dictionary<int, PlayerInGameData>();   // 玩家编号对应玩家信息

    public List<BattleInput> frameInfo;
    public List<BulletUnion> bulletList = new List<BulletUnion>();

    int AttackInterval = 5;

    public GameObject CD1=null;
    public GameObject CD2 = null;


    public Dictionary<int, GameObject> playerToRevival = new Dictionary<int, GameObject>();   // 玩家编号对应复活框
    Vector3 Revival_Offset = new Vector3(0, 0.8f, 0);
   

    public PlayerDataModule(BattleManager parent)
    {
        _parentManager = parent;
    }
    public void Free()
    {
        playerToPlayer.Clear();
        if (frameInfo != null)
        {
            frameInfo.Clear();
        }
        bulletList.Clear();
    }
    void UpdateBuff()
    {
        foreach (KeyValuePair<int, PlayerInGameData> x in playerToPlayer)//buff 
        {
            PlayerModel_Component PlayerComp = x.Value.obj.GetComponent<PlayerModel_Component>();
            if (PlayerComp.debuff.Poison && PlayerComp.debuff.PoisonRemainingFrame>0)
            {
               

                if (PlayerComp.debuff.PoisonRemainingFrame % 20 ==0)
                {
                    Debug.Log("触发毒BUFF伤害：" );
                    BeAttacked(x.Value.obj, 1, x.Value.RoomID);
                }

                PlayerComp.debuff.PoisonRemainingFrame--;
            }
            else
            {
                PlayerComp.debuff.Poison = false;
            }
        }





    }
    public void UpdateLogic(int frame)//更新某一帧逻辑
    {
        UpdateMovement(frame);
        foreach(var p in playerToPlayer)
        {
            //Debug.Log(p.Value.obj.GetComponent<PlayerModel_Component>().healthPoint);
            p.Value.obj.GetComponent<PlayerModel_Component>().UpdateLogic();
            int PlayerUID =_parentManager.sys._model._PlayerModule.uid;
            if (p.Key== PlayerUID)
            {
                if(CD1==null)
                {
                    CD1 = GameObject.Find("SkillStickUI1").transform.GetChild(2).gameObject;
                    CD1.GetComponent<Slider>().maxValue = 0;
                }

                int kp = p.Value.obj.GetComponent<PlayerModel_Component>().GetCountDown1();
                if(kp>CD1.GetComponent<Slider>().maxValue)
                {
                    CD1.GetComponent<Slider>().maxValue = kp;
                    CD1.SetActive(true);
                }
                if(kp==0)
                {
                    CD1.GetComponent<Slider>().maxValue = kp;
                    CD1.SetActive(false);
                }
                CD1.GetComponent<Slider>().value = kp;



                if (CD2 == null)
                {
                    CD2 = GameObject.Find("SkillStickUI2").transform.GetChild(2).gameObject;
                    CD2.GetComponent<Slider>().maxValue = 0;
                }

                kp = p.Value.obj.GetComponent<PlayerModel_Component>().GetCountDown2();
                if (kp > CD2.GetComponent<Slider>().maxValue)
                {
                    CD2.GetComponent<Slider>().maxValue = kp;
                    CD2.SetActive(true);
                }
                if (kp == 0)
                {
                    CD2.GetComponent<Slider>().maxValue = kp;
                    CD2.SetActive(false);
                }
                CD2.GetComponent<Slider>().value = kp;

            }
        }
        CheckRevial();
    }

    void CheckRevial()
    {
        foreach(var item in playerToRevival)
        {
            GameObject PlayerObj = FindPlayerObjByUID(item.Key);
            FixVector2 Body_Pos = PlayerObj.GetComponent<PlayerModel_Component>().GetPlayerPosition();
            bool InSave = false;
            foreach (var player in playerToPlayer)
            {
                //不是同个玩家
                if (player.Key!=item.Key)
                {
                    FixVector2 OtherPos = player.Value.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition();
                    Fix64 P2P = FixVector2.Distance(Body_Pos, OtherPos);

                    //救人判定距离
                    Fix64 SaveDistance =(Fix64)1.3;
                    if (P2P<= SaveDistance)
                    {
                        InSave = true;
                        Debug.Log("正在复活玩家： " + item.Key);
                        FindPlayerObjByUID(item.Key).GetComponent<PlayerModel_Component>().revival++;
                    }
                }
            }
            //无人则重置复活条
            if (!InSave)
            {
                FindPlayerObjByUID(item.Key).GetComponent<PlayerModel_Component>().revival=0;
            }
        }
    }
    void UpdateRevivalBar()
    {
        List<int> DeleteList = new List<int>();

        //更新血条位置
        foreach(var item in playerToRevival)
        {
            GameObject player = FindPlayerObjByUID(item.Key);
            if (player != null)
            {
                int HP = player.GetComponent<PlayerModel_Component>().GetHealthPoint();

                if (HP <= 0)
                {
                    //Vector3 PlayerPos = PackConverter.FixVector2ToVector2(player.GetComponent<PlayerModel_Component>().GetPlayerPosition());
                    Vector3 PlayerPos = player.transform.position;

                    Vector3 ScreenPos = Camera.main.WorldToScreenPoint(PlayerPos + Revival_Offset);
                    item.Value.transform.position = ScreenPos;
                }
                else
                {
                    DeleteList.Add(item.Key);
                }
                //更新复活条数值
                int Revival = player.GetComponent<PlayerModel_Component>().revival;
                item.Value.GetComponent<Slider>().value = (float)Revival / (float)player.GetComponent<PlayerModel_Component>().MaxRevival;

            }
        }
    

        //销毁
        foreach(int item in DeleteList)
        {
            Object.Destroy(playerToRevival[item]);
            playerToRevival.Remove(item);
        }

    }
    public void UpdateMovement(int frame)
    {
        for (int i = 0; i < frameInfo.Count; i++)//更新操作
        {
            if (playerToPlayer.ContainsKey(frameInfo[i].Uid))//有玩家
            {

                PlayerInGameData Input = playerToPlayer[frameInfo[i].Uid];

                if (Input.obj.GetComponent<PlayerModel_Component>().GetDead() == 1) continue;

                Vector2 MoveVec = new Vector2(frameInfo[i].MoveDirectionX / 10000f, frameInfo[i].MoveDirectionY / 10000f).normalized * Global.FrameRate / 1000f * 5f;

                //FixVector2 MoveVec = new FixVector2(frameInfo[i].MoveDirectionX / (Fix64)100, frameInfo[i].MoveDirectionY / (Fix64)100);

                //MoveVec = MoveVec.GetNormalized() * (Fix64)Global.FrameRate / (Fix64)1000 * (Fix64)5;

                FixVector2 Pos = Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition();
                if (_parentManager._terrain.IsMovable(new FixVector2((Fix64)(MoveVec.x + Pos.x), (Fix64)(MoveVec.y + Pos.y)), Input.RoomID))
                {
                    Input.obj.GetComponent<PlayerModel_Component>().Move(new FixVector2((Fix64)MoveVec.x, (Fix64)MoveVec.y));
                }
                else
                {
                    if (_parentManager._terrain.IsMovable(new FixVector2((Fix64)(MoveVec.x + Pos.x), (Fix64)(Pos.y)), Input.RoomID))
                    {
                        Input.obj.GetComponent<PlayerModel_Component>().Move(new FixVector2((Fix64)MoveVec.x, (Fix64)0));
                    }
                    else if (_parentManager._terrain.IsMovable(new FixVector2((Fix64)(Pos.x), (Fix64)(MoveVec.y + Pos.y)), Input.RoomID))
                    {
                        Input.obj.GetComponent<PlayerModel_Component>().Move(new FixVector2((Fix64)0, (Fix64)MoveVec.y));
                    }
                }


                switch (frameInfo[i].AttackType)
                {
                    case (int)AttackType.BasicAttack:
                        {

                            Fix64 AttackDirectionX = (Fix64)(frameInfo[i].AttackDirectionX / (Fix64)100);
                            Fix64 AttackDirectionY = (Fix64)(frameInfo[i].AttackDirectionY / (Fix64)100);

                            //Debug.Log(AttackDirectionX);
                            //Debug.Log(AttackDirectionY);

                            FixVector2 AttackVec = new FixVector2(AttackDirectionX, AttackDirectionY).GetNormalized();


                            if (AttackVec.x != Fix64.Zero
                                || AttackVec.y != Fix64.Zero)
                            //if ((Fix64.Abs(AttackDirectionX) >= (Fix64)0.01f 
                            //&& Fix64.Abs(AttackDirectionY) >= (Fix64)0.01f))
                            {
                                //Debug.Log("aaaaaaaa");
                                if (frame >= Input.NextAttackFrame)
                                {
                                    List<int> list = new List<int>();
                                    BulletUnion bu = new BulletUnion(_parentManager);


                                    CharacterType PlayerType = _parentManager.sys._model._RoomModule.GetCharacterType(frameInfo[i].Uid);
                                    switch (PlayerType)
                                    {
                                        case CharacterType.Enginner:
                                            {
                                                bu.BulletInit("Player", new FixVector2((Fix64)Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().x,
                                                                        (Fix64)Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().y),
                                                                        AttackVec,
                                                                        (Fix64)0.2, (Fix64)2, Input.RoomID,
                                                                        _parentManager.sys._battle._skill.enginerBase.bulletObj

                                                                        , list, frameInfo[i].Uid);
                                                break;
                                            }
                                        case CharacterType.Magician:
                                            {
                                                bu.BulletInit("Player", new FixVector2((Fix64)Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().x,
                                                                        (Fix64)Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().y),
                                                                        AttackVec,
                                                                        (Fix64)0.2, (Fix64)2, Input.RoomID,
                                                                        _parentManager.sys._battle._skill.magicianBase.bulletObj

                                                                        , list, frameInfo[i].Uid);
                                                break;
                                            }
                                    }

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
                                        int cd=_parentManager._skill.enginerBase.Skill1Logic(frame,
                                            _parentManager._player.playerToPlayer[frameInfo[i].Uid].RoomID, tmp,
                                            new Vector2((float)Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().x,
                                            (float)Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().y),
                                            new Vector2((float)frameInfo[i].AttackDirectionX / 10000f,
                                            (float)frameInfo[i].AttackDirectionY / 10000f
                                            ),frameInfo[i].Uid
                                            );
                                        Input.obj.GetComponent<PlayerModel_Component>().SetCountDown1(cd);
                                        break;
                                    }
                                case CharacterType.Magician:
                                    {
                                        int cd=_parentManager._skill.magicianBase.Skill1Logic(frame,
                                            _parentManager._player.playerToPlayer[frameInfo[i].Uid].RoomID, tmp,
                                            new Vector2((float)frameInfo[i].AttackDirectionX / 10000f,
                                            (float)frameInfo[i].AttackDirectionY / 10000f
                                            ),frameInfo[i].Uid
                                            );
                                        Input.obj.GetComponent<PlayerModel_Component>().SetCountDown1(cd);
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
                                        int cd=_parentManager._skill.enginerBase.Skill2Logic(frame,
                                            _parentManager._player.playerToPlayer[frameInfo[i].Uid].RoomID, tmp,
                                            new Vector2((float)Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().x,
                                            (float)Input.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().y),
                                            new Vector2((float)frameInfo[i].AttackDirectionX / 10000f,
                                            (float)frameInfo[i].AttackDirectionY / 10000f
                                            ), frameInfo[i].Uid
                                            );
                                        Debug.Log("aaaaaa" + cd);
                                        Input.obj.GetComponent<PlayerModel_Component>().SetCountDown2(cd);
                                        break;
                                    }
                                case CharacterType.Magician:
                                    {
                                        int cd=_parentManager._skill.magicianBase.Skill2Logic(frame,
                                            _parentManager._player.playerToPlayer[frameInfo[i].Uid].RoomID, tmp,
                                            new Vector2((float)frameInfo[i].AttackDirectionX / 10000f,
                                            (float)frameInfo[i].AttackDirectionY / 10000f
                                            ), frameInfo[i].Uid
                                            );
                                        Input.obj.GetComponent<PlayerModel_Component>().SetCountDown2(cd);
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

                foreach (var it in bulletList)
                {
                    it.LogicUpdate();
                }

            }
        }



        UpdateBuff();

    }


    //obj = 受击OBJECT , dmg = 伤害
    public void BeAttacked(GameObject obj, int dmg,int roomid)
    {

        if (obj.GetComponent<PlayerModel_Component>().GetHealthPoint()<=0)
        {
            return;
        }
        int LeftHealthPoint = obj.GetComponent<PlayerModel_Component>().GetHealthPoint() - dmg;
        if (LeftHealthPoint<=0)
        {
            obj.GetComponent<PlayerModel_Component>().SetHealthPoint(0);
          
            Vector3 PlayerPos = PackConverter.FixVector2ToVector2( obj.GetComponent<PlayerModel_Component>().GetPlayerPosition());
            Vector3 ScreenPos = Camera.main.WorldToScreenPoint(PlayerPos+ Revival_Offset);
            Debug.Log("ScreenPos: " + ScreenPos);
            GameObject Revival_Prefab = (GameObject)Resources.Load("UI/UIPrefabs/Revival");
            GameObject Canvas = GameObject.Find("Canvas");
            if (Canvas != null)
            {
                GameObject Revival_Instance = Object.Instantiate(Revival_Prefab, Canvas.transform);
                Revival_Instance.transform.position = ScreenPos;
                if (FindPlayerUIDbyObject(obj)!=-1)
                {
                    int uid = FindPlayerUIDbyObject(obj);
                    if (!playerToRevival.ContainsKey(uid))
                    {
                        playerToRevival.Add(uid, Revival_Instance);
                    }
                    else
                    {
                        playerToRevival[uid] = Revival_Instance;
                    }
                }

            }
        }
        else
        {
            obj.GetComponent<PlayerModel_Component>().SetHealthPoint(LeftHealthPoint);
        }
 
        int PlayerUID = _parentManager.sys._model._PlayerModule.uid;

        foreach(var x in playerToPlayer)
        {
            if(x.Value.obj==obj)
            {
                if(x.Key==PlayerUID)
                {
                    misc.ScreenFlash();
                }
            }
        }
    }

    public void UpdateView()//更新视图
    {
        for (int i = 0; i < frameInfo.Count; i++)
        {
            if (playerToPlayer.ContainsKey(frameInfo[i].Uid))
            {
                PlayerInGameData Input = playerToPlayer[frameInfo[i].Uid];
                Input.obj.GetComponent<PlayerView_Component>().RefreshView();
            }

        }
        foreach (var it in bulletList)
        {
            it.ViewUpdate();

        }

        //更新复活框位置
        UpdateRevivalBar();
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
   
    int FindPlayerUIDbyObject(GameObject obj)
    {
        int ret = -1;

        foreach(var item in playerToPlayer)
        {
            if (item.Value.obj== obj)
            {
                return item.Key;
            }
        }
        return ret;
    }

}
