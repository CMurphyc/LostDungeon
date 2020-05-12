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
    public HashSet<int> ActPlayer;
    public ChestInfo(GameObject thing,ThingsType type,int ActFrame,FixVector2 Position)
    {
        this.thing = thing;
        this.type = type;
        this.ActFrame = ActFrame;
        this.Position = Position;
        ActPlayer = new HashSet<int>();
    }
}

public class ChestModule
{
    BattleManager _parentManager;

    public HashSet<int> Room;

    public List<ChestInfo> Chest;

    //开过的宝箱
    public List<GameObject> OpenedChests;
    //被拾取的金币
    public List<KeyValuePair<GameObject,float> > HandledCoins;

    public int CoinValue = 0;
    public int LastTime = 0;


    public Dictionary<int, List<TreasureData>> roomToTreasure;   // 房间号对应宝物列表
    public Dictionary<int, PropData> propToProperty;   // 根据道具名称找到对应道具属性


    public ChestModule(BattleManager _parentManager)
    {
        this._parentManager = _parentManager;
        Room = new HashSet<int>();
        Chest = new List<ChestInfo>();
        OpenedChests = new List<GameObject>();
        HandledCoins = new List<KeyValuePair<GameObject, float>>();
        roomToTreasure = new Dictionary<int, List<TreasureData>>();
        propToProperty = new Dictionary<int, PropData>();
    }
    public void Free()
    {
        Room.Clear();
        Chest.Clear();
        OpenedChests.Clear();
        HandledCoins.Clear();
        roomToTreasure.Clear();
        propToProperty.Clear();
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
                            GameObject it =Object.Instantiate(Resources.Load("UI/UIPrefabs/Openedchest"), Chest[i].thing.transform.position, Chest[i].thing.transform.rotation) as GameObject;
                            OpenedChests.Add(it);
                            SCCoins(Chest[i].Position, Frame);
                            Object.Destroy(Chest[i].thing);
                            Chest.RemoveAt(i);
                        }
                    }
                    break;
                case ThingsType.Coin:
                    FixVector2 coinspos = Chest[i].Position;
                    foreach (var pler in _parentManager._player.playerToPlayer)
                    {
                        if (FixVector2.Distance(pler.Value.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition(), coinspos) > (Fix64)4) continue;
                        if (Chest[i].ActPlayer.Contains(pler.Key)) continue;
                        if(pler.Key==_parentManager._player.FindCurrentPlayerUID())
                        {
                            HandledCoins.Add(new KeyValuePair<GameObject, float>(Chest[i].thing, 0.025f));
                        }
                        if (_parentManager.sys._model._RoomModule.PVEResult.ContainsKey(pler.Key) == false)
                        {
                            _parentManager.sys._model._RoomModule.PVEResult.Add(pler.Key, new PVEData());
                        }
                        _parentManager.sys._model._RoomModule.PVEResult[pler.Key].coins++;
                    }
                    break;
            }
        }

    }
    public void UpdateView()
    {
        GameObject pler = _parentManager._player.FindPlayerObjByUID(_parentManager._player.FindCurrentPlayerUID());
        LastTime++;
        for (int i=HandledCoins.Count-1;i>=0;i--)
        {
            GameObject coin = HandledCoins[i].Key;
            if (Vector2.Distance(coin.transform.position,pler.transform.position)<=0.8f)
            {
                Object.Destroy(coin);
                HandledCoins.RemoveAt(i);
                CoinValue++;
                LastTime = 0;
            }
            else
            {
                coin.transform.position=Vector2.Lerp(coin.transform.position, pler.transform.position, HandledCoins[i].Value);
                HandledCoins[i] = new KeyValuePair<GameObject, float>(coin, HandledCoins[i].Value+Time.deltaTime/2);
            }
        }
        if (LastTime>=6&&CoinValue>0)
        {
            _parentManager._textjump.AddCoinText(pler.transform.position, CoinValue);
            CoinValue = 0;
            LastTime = 0;
        }


        GameObject tobj = GameObject.Find("AttackStickUI");
        bool has = false;
        int PlayerUID = _parentManager.sys._model._PlayerModule.uid;
        foreach(var x in roomToTreasure[_parentManager.sys._battle._player.playerToPlayer[PlayerUID].RoomID])
        {
            if (x.active) continue;
            FixVector2 tmp = new FixVector2( (Fix64)x.treasureTable.transform.position.x, (Fix64)x.treasureTable.transform.position.y);
            if(FixVector2.Distance(tmp,
                _parentManager.sys._battle._player.playerToPlayer[PlayerUID].obj.GetComponent<PlayerModel_Component>().GetPlayerPosition())<=(Fix64)1.4f)
            {
                has = true;
                tobj.GetComponent<VirtualJoystick2>().pickIcon();
                break;
            }
        }
        if(!has)
        {
            tobj.GetComponent<VirtualJoystick2>().attackIcon();
        }
    }

    /// <summary>
    /// 生成宝箱
    /// </summary>
    void SCChest(int Frame,int RoomID)
    {
        GameObject it= Object.Instantiate(Resources.Load("UI/UIPrefabs/chest")) as GameObject;
        foreach (var i in _parentManager._player.FindPlayerInRoom(RoomID))
        {
            it.transform.position = i.obj.transform.position;
            Chest.Add(new ChestInfo(it, ThingsType.Chest, Frame + 40,i.obj.GetComponent<PlayerModel_Component>().playerPosition));
            break;
        }
    }
    /// <summary>
    /// 生成金币
    /// </summary>
    void SCCoins(FixVector2 chestpos,int Frame)
    {
        int cnt = Random.Range(3, 7);
        for (int i = 0; i < cnt; i++)
        {
            GameObject it = Object.Instantiate(Resources.Load("UI/Scene'sPictures/MapCreat/Prefabs/coin")) as GameObject;
            it.transform.position = new Vector2((float)chestpos.x, (float)chestpos.y) + Random.insideUnitCircle;
            Chest.Add(new ChestInfo(it, ThingsType.Coin, Frame + 20, new FixVector2((Fix64)it.transform.position.x, (Fix64)it.transform.position.y)));
        }
        
    }

    /// <summary>
    /// 玩家通关房间
    /// </summary>
    /// <param name="RoomID">房间ID</param>
    void PassRoom(int Frame)
    {
        foreach(var i in _parentManager._monster.RoomToMonster)
        {
            if (i.Value.Count > 0)
            {
                Room.Add(i.Key);
            }
        }
        foreach(var i in _parentManager._player.playerToPlayer)
        {
            if(Room.Contains(i.Value.RoomID)&&_parentManager._monster.GetMonsterNumber(i.Value.RoomID)==0)
            {
                SCChest(Frame,i.Value.RoomID);
                Room.Remove(i.Value.RoomID);
            }
        }
    }
}
