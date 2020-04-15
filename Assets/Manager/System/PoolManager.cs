using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public struct PlayerObjInfo
{
    int uid;
    GameObject obj;
}

 public class PoolManager 
{

    //------------------战斗中所有资源的生成与销毁写这里-------------------------------

    //当前所有玩家所在的房间
    public HashSet<int> LiveRoom;

    public Dictionary<int, List<PlayerObjInfo>> RoomToPlayer;
    //房间号-怪物列表
    public Dictionary<int, List<GameObject>> RoomToMonster;

    //房间号-抛射物
    public Dictionary<int, List<GameObject>> RoomToProjectile;

    //-------------------------不可修改-------------------

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


    public PoolManager()
    {
   
        LiveRoom = new HashSet<int>();
        RoomToMonster = new Dictionary<int, List<GameObject>>();
        RoomToProjectile = new Dictionary<int, List<GameObject>>();
        RoomToStone = new Dictionary<int, List<GameObject>>();
        RoomToDoor = new Dictionary<int, List<int>>();
        DoorToDoorObj = new Dictionary<int, GameObject>();
        doorToDoor = new Dictionary<int, int>();
        DoorToRoom = new Dictionary<int, int>();
    }

}
