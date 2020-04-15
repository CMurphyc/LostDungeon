using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainModule : MonoBehaviour
{
    BattleManager _parentManager;


    //房间号-障碍物
    public Dictionary<int, List<GameObject>> RoomToStone;
    //房间号-传送门列表
    public Dictionary<int, List<int>> RoomToDoor;
    //门号-门号实体
    public Dictionary<int, GameObject> DoorToDoorObj;
    //门号-传送门号
    public Dictionary<int, int> doorToDoor;
    //门号-房间号
    public Dictionary<int, int> DoorToRoom;


    public TerrainModule(BattleManager parent)
    {
        _parentManager = parent;
    }

    public void UpdateLogic(int frame)
    {



    }
    public void UpdateView()
    {



    }
}
