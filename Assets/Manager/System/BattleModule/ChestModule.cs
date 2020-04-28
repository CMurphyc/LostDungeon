using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ThingsType
{
    Coin,
    Chest,
}
public class ChestInfo
{
    public GameObject thing;     //物体
    public ThingsType type;      //thing类型
    public int ActFrame;         //作用帧数
    public FixVector2 Position;  //物体位置
    public ChestInfo(GameObject thing,ThingsType type,int ActFrame,FixVector2 Position)
    {
        this.thing = thing;
        this.type = type;
        this.ActFrame = ActFrame;
        this.Position = Position;
    }
}

public class ChestModule:MonoBehaviour
{
    BattleManager _parentManager;

    public HashSet<int> Room;

    public List<ChestInfo> Chest;

    //开过的宝箱
    public List<GameObject> OpenedChests;
    //被拾取的金币
    public List<GameObject> HandledCoins;
    public ChestModule(BattleManager _parentManager)
    {
        this._parentManager = _parentManager;
        Room = new HashSet<int>();
        Chest = new List<ChestInfo>();
        OpenedChests = new List<GameObject>();
        HandledCoins = new List<GameObject>();
    }
    public void UpdateLogic(int Frame)
    {
        PassRoom(Frame);
        for (int i = Chest.Count - 1; i >= 0; i--)
        {
            if (Chest[i].ActFrame > Frame) continue;
            FixVector2 playerpos;
            switch (Chest[i].type)
            {
                case ThingsType.Chest:
                    foreach (var j in _parentManager.sys._battle._player.playerToPlayer)   //打开宝箱
                    {
                        playerpos = j.Value.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition();
                        if (FixVector2.Distance(playerpos, Chest[i].Position) <= (Fix64)1)
                        {
                            GameObject it = Instantiate(Resources.Load("UI/UIPrefabs/Openedchest"), Chest[i].thing.transform.position, Chest[i].thing.transform.rotation) as GameObject;
                            OpenedChests.Add(it);
                            SCCoins(Chest[i].Position, Frame);
                            Destroy(Chest[i].thing);
                            Chest.RemoveAt(i);
                        }
                    }
                    break;
                case ThingsType.Coin:
                    GameObject pler = _parentManager._player.FindPlayerObjByUID(_parentManager._player.FindCurrentPlayerUID());
                    playerpos = pler.GetComponent<PlayerModel_Component>().GetPlayerPosition();
                    FixVector2 coinspos = Chest[i].Position;
                    if (FixVector2.Distance(playerpos, coinspos) <= (Fix64)2)
                    {
                        HandledCoins.Add(Chest[i].thing);
                        Chest.RemoveAt(i);

                    }
                    break;
            }
        }
    }
    public void UpdateView(int Frame)
    {
        for(int i=HandledCoins.Count-1;i>=0;i--)
        {
            GameObject pler = _parentManager._player.FindPlayerObjByUID(_parentManager._player.FindCurrentPlayerUID());
            GameObject coin = HandledCoins[i];
            if (Vector2.Distance(coin.transform.position,pler.transform.position)<=0.1)
            {
                Destroy(coin);
                HandledCoins.RemoveAt(i);
            }
            else
            {
                coin.transform.position=Vector2.Lerp(coin.transform.position, pler.transform.position,0.2f);
            }
        }
    }

    /// <summary>
    /// 生成宝箱
    /// </summary>
    void SCChest(int Frame)
    {
        GameObject it=Instantiate(Resources.Load("UI/UIPrefabs/chest")) as GameObject;
        it.transform.position = _parentManager._player.FindPlayerObjByUID(_parentManager._player.FindCurrentPlayerUID()).transform.position;
        Chest.Add(new ChestInfo(it,ThingsType.Chest,Frame+20,
            new FixVector2((Fix64)it.transform.position.x,(Fix64)it.transform.position.y)));
    }
    /// <summary>
    /// 生成金币
    /// </summary>
    void SCCoins(FixVector2 chestpos,int Frame)
    {
        for(int i=1;i<=5;i++)
        {
            GameObject it=Instantiate(Resources.Load("UI/Scene'sPictures/MapCreat/Prefabs/coin")) as GameObject;
            it.transform.position = new Vector2((float)chestpos.x,(float)chestpos.y)+Random.insideUnitCircle;
            Chest.Add(new ChestInfo(it,ThingsType.Coin,Frame+10,new FixVector2((Fix64)it.transform.position.x,(Fix64)it.transform.position.y)));
        }
    }

    /// <summary>
    /// 玩家通关房间
    /// </summary>
    /// <param name="RoomID">房间ID</param>
    void PassRoom(int Frame)
    {
        int CurrentRoomID = _parentManager._player.playerToPlayer[_parentManager._player.FindCurrentPlayerUID()].RoomID;
        if(_parentManager._monster.GetMonsterNumber(CurrentRoomID)==0&&Room.Contains(CurrentRoomID)==false)
        {
            SCChest(Frame);
            Room.Add(CurrentRoomID);
        }
    }
}
