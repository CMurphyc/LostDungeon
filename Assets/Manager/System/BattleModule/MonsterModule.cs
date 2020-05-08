using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

public class AliasMonsterPack
{
    public int RemainingFrame;
    public GameObject obj;

}

public class MonsterModule
{
    BattleManager _parentManager;

    //房间号-敌方怪物列表
    public Dictionary<int, List<GameObject>> RoomToMonster = new Dictionary<int, List<GameObject>>();
    public int BossRoom;

    //房间号-己方怪物列表
    public Dictionary<int, List<AliasMonsterPack>> RoomToAliasUnit = new Dictionary<int, List<AliasMonsterPack>>();

    /// <summary>
    /// 敌方单位
    /// </summary>
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
    //Boss持续伤害技能
    public List<GameObject> BossSkill = new List<GameObject>();



    /// <summary>
    /// 己方单位
    /// </summary>
    //死亡延迟销毁尸体
    private Dictionary<GameObject, int> AliasRemoveCounter = new Dictionary<GameObject, int>();
    //死亡延迟从容器剔除
    private Dictionary<GameObject, MonsterPack> AliasRemoveWaitFrame = new Dictionary<GameObject, MonsterPack>();

 


    private Color Attacked = new Color(255f/255f, 93f / 255f, 93f / 255f);
    private Color Normal = new Color(255f / 255f, 255f / 255f, 255f / 255f);

    private bool BossUIInited = false;
    public MonsterModule(BattleManager parent)
    {
        _parentManager = parent;
      
    }
    public void Free()
    {
        RoomToMonster.Clear();
        RoomToAliasUnit.Clear();
        RemoveCounter.Clear();
        RemoveWaitFrame.Clear();
        StatusCounter.Clear();
        BossMove.Clear();
        bulletList.Clear();
        bulletEvent.Clear();
        BossSkill.Clear();
        AliasRemoveCounter.Clear();
        AliasRemoveWaitFrame.Clear();
        BossUIInited = false;
    }

    public void UpdateLogic(int frame)
    {
        MonsterAILogic(frame);
        MonsterBeAttackHandler(frame);
        MonsterDeadHandler();
        UpdateBossSkill();
        UpdateBullet(frame);
        UpdateBossHP();
        UpdateBuff();
        UpdateBossIcon();
    }
    //目前只针对Boss
    void UpdateBuff()
    {
        //无敌BUFF
        if (RoomToMonster.ContainsKey(BossRoom))
        {
            for (int i = 0; i < RoomToMonster[BossRoom].Count;i++ )
            {
                GameObject boss = RoomToMonster[BossRoom][i];
                if (boss.tag == "Boss")
                {
                    //Debug.Log("Tag: " + boss.tag);
                    //Debug.Log("Name: " + boss.name);
                    if (boss.GetComponent<MonsterModel_Component>().buff.Undefeadted && 
                        boss.GetComponent<MonsterModel_Component>().buff.Undefeadted_RemainingFrame>0)
                    {
                        boss.GetComponent<MonsterModel_Component>().buff.Undefeadted_RemainingFrame--;
                    }
                    else
                    {
                        boss.GetComponent<MonsterModel_Component>().buff.Undefeadted = false;
                    }
                }
            }
        }
        //冰冻buff
        foreach (int roomid in _parentManager._player.GetLiveRoom())
        {
            if (RoomToMonster.ContainsKey(roomid))
            {
                for (int i = 0; i < RoomToMonster[roomid].Count;i++)
                {
                    if (RoomToMonster[roomid][i].GetComponent<MonsterModel_Component>().Debuff.Freeze.isFreeze 
                        && RoomToMonster[roomid][i].GetComponent<MonsterModel_Component>().Debuff.Freeze.RemainingFrame>0)
                    {
                        RoomToMonster[roomid][i].GetComponent<MonsterModel_Component>().Debuff.Freeze.RemainingFrame--;

                    }
                    else
                    {
                        RoomToMonster[roomid][i].GetComponent<MonsterModel_Component>().Debuff.Freeze.isFreeze = false;
                    }
                }
            }
        }
    }
    void UpdateBossSkill()
    {
        List<GameObject> DeleteQueue = new List<GameObject>();

        for (int i = 0; i < BossSkill.Count;i++)
        {
            GameObject Skill = BossSkill[i];
            SkillType type = Skill.GetComponent<Skill_Component>().SkillType;
            FixVector2 BufPos = Skill.GetComponent<Skill_Component>().Position;
            Fix64 Radius = Skill.GetComponent<Skill_Component>().Radius;
            int RemainingFrame = Skill.GetComponent<Skill_Component>().RemainingFrame;
            if (RemainingFrame <= 0)
            {
                DeleteQueue.Add(Skill);
            }
            else
            {
                switch (type)
                {
                    case SkillType.BossPoison:
                        {
                            // to do 判断是否有人在毒里

                            foreach(var item in _parentManager._player.playerToPlayer)
                            {
                                FixVector2 PlayerPos = item.Value.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition();

                                Fix64 D2P = FixVector2.Distance(BufPos, PlayerPos);
                                if (D2P<= Radius)
                                {
                                    //刷新 毒Debuff

                                    if (!item.Value.obj.GetComponent<PlayerModel_Component>().debuff.Poison)
                                    {
                                        Debug.Log("刷新毒BUFF");
                                        item.Value.obj.GetComponent<PlayerModel_Component>().debuff.Poison = true;
                                        item.Value.obj.GetComponent<PlayerModel_Component>().debuff.PoisonRemainingFrame = item.Value.obj.GetComponent<PlayerModel_Component>().debuff.PoisonFrameDuration;

                                    }

                                }
                            }


                            break;
                        }
                }


                Skill.GetComponent<Skill_Component>().RemainingFrame--;
            }
        }

        for (int i = 0; i < DeleteQueue.Count;i++)
        {
            GameObject temp = DeleteQueue[i];
            if (BossSkill.Contains(temp))
            {
                BossSkill.Remove(temp);
                Object.Destroy(temp);
            }
        }
        DeleteQueue.Clear();
    }

    void UpdateBullet(int frame)
    {
        if (bulletEvent.ContainsKey(frame))
        {
            for (int i = 0; i < bulletEvent[frame].Count; i++)
            {
                FakeBulletUnion temp = bulletEvent[frame][i];
                BulletUnion bu = new BulletUnion(_parentManager);

                if (temp.boss != null)
                {
                    FixVector2 MonsterPos = PackConverter.FixVector3ToFixVector2(temp.boss.GetComponent<MonsterModel_Component>().position);

                    bu.BulletInit(temp.tag, MonsterPos, temp.toward, temp.speed, temp.damage, temp.roomid, temp.bulletPrefab, temp.itemList,0);
                    bulletList.Add(bu);
                }
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
            if(item.Key==null)
            {
                continue;
            }
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

    //UI 
    void UpdateBossIcon()
    {

        //刷新BOSS ICon 名字
        BattleUIUpdate BattleUI = GameObject.Find("Canvas").GetComponent<BattleUIUpdate>();
        GameObject BossUI = BattleUI.BossUI;
        if (!BossUIInited)
        {
            switch (_parentManager.sys._model._RoomModule.MapFloorNumber)
            {
                case 1:
                    {

                        break;
                    }

                case 2:
                    {
                        BossUI.transform.Find("head_frame/head_img").gameObject.GetComponent<Image>().sprite =
                        BattleUI.boss2;
                        BossUI.transform.Find("BossName").gameObject.GetComponent<Text>().text =
                       "Dark Wizard";

                        break;
                    }
                case 3:
                    {
                        BossUI.transform.Find("head_frame/head_img").gameObject.GetComponent<Image>().sprite =
                         BattleUI.boss3;
                        BossUI.transform.Find("BossName").gameObject.GetComponent<Text>().text =
                      "Black Knight";
                        break;
                    }
            }

            BossUIInited = true;
        }
    }



    public void UpdateBossHP()
    {

        if (GameObject.Find("Canvas") != null)
        {
            BattleUIUpdate BattleUI = GameObject.Find("Canvas").GetComponent<BattleUIUpdate>();
            if (BattleUI != null)
            {
                GameObject BossUI = BattleUI.BossUI;

                if (BossUI != null)
                {
                    int CurrnetUID = _parentManager._player.FindCurrentPlayerUID();
                    int CurrnetRoomID = _parentManager._player.playerToPlayer[CurrnetUID].RoomID;
                    if (CurrnetRoomID == BossRoom)
                    {
                        if (!BossUI.activeSelf)
                        {
                            BossUI.SetActive(true);
                        }
                        else
                        {
                            List<GameObject> ListObj = _parentManager._monster.RoomToMonster[BossRoom];
                            for (int i = 0; i < ListObj.Count; i++)
                            {
                                if (ListObj[i].tag == "Boss")
                                {
                                    bl_ProgressBar BossHP = GameObject.Find("Canvas/BossHint/HP/Mask/Slider").GetComponent<bl_ProgressBar>();
                                    BossHP.MaxValue = (float)ListObj[i].GetComponent<MonsterModel_Component>().MaxHP;
                                    BossHP.Value = (float)ListObj[i].GetComponent<MonsterModel_Component>().HP;
                                    break;
                                }
                            }
                        }
                    }


                    Text MonsterNum = GameObject.Find("Canvas/MonsterLeft/monsternum").GetComponent<Text>();
                    MonsterNum.text = _parentManager._monster.RoomToMonster[CurrnetRoomID].Count.ToString();

                }
            }
        }

    }

    //obj = 受击OBJECT , dmg = 伤害
    public void BeAttacked(GameObject obj, float dmg, int roomid, int dmg_srcUID)
    {
        if (obj.GetComponent<MonsterModel_Component>().buff.Undefeadted)
        {
            return;
        }
       

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
            if (dmg_srcUID!=0)
            {
                if (!_parentManager.sys._model._RoomModule.PVEResult.ContainsKey(dmg_srcUID))
                {
                    PVEData data = new PVEData();
                    data.kills = 1;
                    _parentManager.sys._model._RoomModule.PVEResult.Add(dmg_srcUID, data);
                        }
                else
                {
                    _parentManager.sys._model._RoomModule.PVEResult[dmg_srcUID].kills++;
                }

                Debug.Log("Match OverView");
                foreach(var item in _parentManager.sys._model._RoomModule.PVEResult)
                {
                    Debug.Log("Player: " + item.Key);
                    Debug.Log("Kills: " + item.Value.kills);
                }


            }

            obj.GetComponent<MonsterModel_Component>().HP = Fix64.Zero;

            if (RoomToMonster[roomid].Contains(obj))
            {
                MonsterPack temp = new MonsterPack();
                temp.RemainingFrame = 2;
                temp.RoomID = roomid;
                if (!RemoveWaitFrame.ContainsKey(obj))
                {
                    RemoveWaitFrame.Add(obj, temp);
                }
                //RoomToMonster[roomid].Remove(obj);
                int LeftFrameFromDestory = obj.GetComponent<MonsterModel_Component>().FrameLeftFromDestroy;

                if (!RemoveCounter.ContainsKey(obj))
                {
                    RemoveCounter.Add(obj, LeftFrameFromDestory);
                }


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

        //Debug.Log("MONSTER HP: " + obj.GetComponent<MonsterModel_Component>().HP);
    }
    void MonsterAILogic(int frame)
    {

        foreach (int RoomID in _parentManager._player.GetLiveRoom())
        {
            //敌方AI
            if (RoomToMonster.ContainsKey(RoomID))
            {
                List<GameObject> MonsterList = RoomToMonster[RoomID];
                for (int i = MonsterList.Count-1; i >=0; i--)
                {
                    if(MonsterList[i]==null)
                    {
                        MonsterList.RemoveAt(i);
                        continue;
                    }
                    GameObject Monster = MonsterList[i];

                    //FixVector2 MonsterPos = new Vector2((float)Monster.GetComponent<MonsterModel_Component>().position.x, (float)Monster.GetComponent<MonsterModel_Component>().position.y);
                    FixVector2 MonsterPos =PackConverter.FixVector3ToFixVector2(Monster.GetComponent<MonsterModel_Component>().position);
                    GameObject Target = FindClosePlayer(MonsterPos, RoomID);

                    Monster.GetComponent<EnemyAI>().UpdateLogic(Target, frame, Monster,true);
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
            //己方AI
            if (RoomToAliasUnit.ContainsKey(RoomID))
            {
                List<AliasMonsterPack> AliasMonsterList = RoomToAliasUnit[RoomID];
                for (int i = 0; i < AliasMonsterList.Count;i++)
                {
                    if (AliasMonsterList[i].RemainingFrame > 0)
                    {
                        AliasMonsterList[i].RemainingFrame--;
                        GameObject AliasUnit = AliasMonsterList[i].obj;
                        Vector2 AliasPos = new Vector2((float)AliasUnit.GetComponent<MonsterModel_Component>().position.x, (float)AliasUnit.GetComponent<MonsterModel_Component>().position.y);
                        GameObject Target = FindCloseMonster(AliasPos, RoomID);
                        AliasUnit.GetComponent<EnemyAI>().UpdateLogic(Target, frame, AliasUnit, false);
                        if (Target != null)
                        {
                            AliasUnit.GetComponent<AIDestinationSetter>().Target = new Vector3((float)Target.GetComponent<MonsterModel_Component>().position.x,
                                (float)Target.GetComponent<MonsterModel_Component>().position.y, (float)0);
                            AliasUnit.GetComponent<AIDestinationSetter>().AI_Switch = true;
                            AliasUnit.GetComponent<EnemyAI>().InitMonster(frame);
                        }
                        else
                        {
                            AliasUnit.GetComponent<AIDestinationSetter>().AI_Switch = false;
                        }
                    }
                    else
                    {
                        //销毁
                        GameObject AliasUnit = AliasMonsterList[i].obj;
                        AliasUnit.GetComponent<MonsterModel_Component>().HP = Fix64.Zero;
                        if (RoomToAliasUnit[RoomID].Contains(AliasMonsterList[i]))
                        {
                            //MonsterPack temp = new MonsterPack();
                            //temp.RemainingFrame = 2;
                            //temp.RoomID = RoomID;

                            //AliasRemoveWaitFrame.Add(AliasUnit, temp);

                            RoomToAliasUnit[RoomID].Remove(AliasMonsterList[i]);
                            int LeftFrameFromDestory = AliasUnit.GetComponent<MonsterModel_Component>().FrameLeftFromDestroy; 
                            AliasRemoveCounter.Add(AliasUnit, LeftFrameFromDestory);

                        }
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

                if (Boss != null)
                {
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

        //己方单位销毁
        //销毁死亡动画
        //销毁
        /*
        List<GameObject> tempTrash3 = new List<GameObject>();
        List<MonsterEventFrame> LiveMonster3 = new List<MonsterEventFrame>();

        foreach (var item in AliasRemoveWaitFrame)
        {
            int LeftFrame = item.Value.RemainingFrame - 1;
            if (LeftFrame > 0)
            {
                MonsterEventFrame temp = new MonsterEventFrame();
                temp.obj = item.Key;
                temp.RemainingFrame = LeftFrame;
                LiveMonster3.Add(temp);
            }
            else
            {
                tempTrash3.Add(item.Key);
            }
        }
        for (int i = 0; i < LiveMonster3.Count; i++)
        {
            AliasRemoveWaitFrame[LiveMonster3[i].obj].RemainingFrame = LiveMonster3[i].RemainingFrame;
        }
        for (int i = 0; i < tempTrash3.Count; i++)
        {
            if (AliasRemoveWaitFrame.ContainsKey(tempTrash3[i]))
            {
                AliasMonsterPack temp = new AliasMonsterPack();
                bool Inited = false;
                for (int j = 0; j < RoomToAliasUnit[RemoveWaitFrame[tempTrash3[i]].RoomID].Count;j++)
                {
                   if ( RoomToAliasUnit[RemoveWaitFrame[tempTrash3[i]].RoomID][j].obj == tempTrash3[i])
                    {
                        Inited = true;
                        temp = RoomToAliasUnit[RemoveWaitFrame[tempTrash3[i]].RoomID][j];
                    }
                }
                if (Inited)
                {
                    RoomToAliasUnit[RemoveWaitFrame[tempTrash3[i]].RoomID].Remove(temp);
                }
                AliasRemoveWaitFrame.Remove(tempTrash3[i]);

            }
        }
        tempTrash3.RemoveAll(it => tempTrash3.Contains(it));
        */

        //死亡销毁
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













        ///敌方单位
        //延迟1帧踢出队列
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





        //死亡销毁
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
            //敌方单位
            if (RoomToMonster.ContainsKey(RoomID))
            {
                List<GameObject> MonsterList = RoomToMonster[RoomID];
                for (int i = 0; i < MonsterList.Count; i++)
                {
                    GameObject Monster = MonsterList[i];

                    Monster.GetComponent<EnemyAI>().UpdateView(Monster);
                }
            }
            //己方单位
            if (RoomToAliasUnit.ContainsKey(RoomID))
            {
                List<AliasMonsterPack> MonsterList = RoomToAliasUnit[RoomID];
                for (int i = 0; i < MonsterList.Count; i++)
                {
                    GameObject Monster = MonsterList[i].obj;
                    Monster.GetComponent<EnemyAI>().UpdateView(Monster);
                }
            }


        }
    }
    



    //helper functions
    GameObject FindCloseMonster(Vector2 AliasPos, int RoomID)
    {
        GameObject ret = null;
        float Min_Distance = 99999;

        if (RoomToMonster.ContainsKey(RoomID))
        {
            List<GameObject> MonsterList = RoomToMonster[RoomID];
            for (int i = 0; i < MonsterList.Count; i++)
            {
                Vector2 MonsterPos = new Vector2((float)MonsterList[i].GetComponent<MonsterModel_Component>().position.x, (float)MonsterList[i].GetComponent<MonsterModel_Component>().position.y);
                float distance = Vector2.Distance(MonsterPos, AliasPos);
                if (distance < Min_Distance)
                {
                    Min_Distance = distance;
                    ret = MonsterList[i];
                }
            }


        }
        return ret;
    }
    GameObject FindClosePlayer(FixVector2 MonsterPos, int RoomID )
    {
        GameObject ret = null;
        Fix64 Min_Distance = (Fix64)99999;

     
        List<PlayerInGameData> PlayerInRoomList = _parentManager._player.FindPlayerInRoom(RoomID);
        for (int i = 0; i < PlayerInRoomList.Count; i++)
        {
            //Vector2 PlayerPos = new Vector2 ((float)PlayerInRoomList[i].obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().x, (float)PlayerInRoomList[i].obj.GetComponent<PlayerModel_Component>().GetPlayerPosition().y);
            if (PlayerInRoomList[i].obj.GetComponent<PlayerModel_Component>().dead == 1)
            {
                continue;
            }
            FixVector2 PlayerPos = PlayerInRoomList[i].obj.GetComponent<PlayerModel_Component>().GetPlayerPosition();
            Fix64 distance = FixVector2.Distance(PlayerPos, MonsterPos);
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
    public int FindRoomIDByMonster(GameObject Monster)
    {
        foreach(var i in RoomToMonster)
        {
            foreach(var j in i.Value)
            {
                if(Monster==j)
                {
                    return i.Key;
                }
            }
        }
        return -1;
    }
}
