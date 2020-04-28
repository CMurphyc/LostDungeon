﻿using UnityEngine;
using UnityEditor;


enum AI_BehaviorType
{
    Idle,
    Run,
    Attack,
    Dead,
    UnderAttack,
    Dash,
    BossSkill1,
    BossSkill2

}

[System.Serializable]
public class Monster
{
    public GameObject monsterGameObject;
    public AI_Type type;
    public int MonsterID;
}



[System.Serializable]
public enum AI_Type
{
    Normal_Melee,
    Nomral_Range,
    Boss_Rabit,
    Boss_Rabit_Egg,
    Engineer_TerretTower,
    Boss_Wizard,
    Boss_DarkKnight,
    Boss_DarkKnightSword
}


public enum StateType
{
    Not_Ready,
    Ready
}

public enum ObjectType
{

    Player, //玩家
    NPC,    //非玩家控制单位
    Projectile, //抛射物
    Portol,      //传送门
};

public class EventMessageType
{
    public static string UserLogin = "Login";
    public static string CreateGame = "CreateGame";
    public static string GetRoomInfo = "GetRoomInfo";
    public static string RefreshHeroWindow = "RefreshHeroWindow";
    public static string GetRoomList = "GetRoomList";
    public static string LeaveRoom = "LeaveRoom";
    public static string StartGame = "StartGame";

    public static string BattleSyn = "BattleSyn";
}

public enum CharacterType
{
    Enginner,
    Warrior,
    Magician,

    None


}
public class PlayerInGameData
{
    public GameObject obj;
    public int RoomID;

    public int NextAttackFrame;
    public void ChangeRoomID(int val)
    {
        RoomID = val;
    }

}

public struct PlayerData
{
    public bool empty;
    public int uid;
    public bool ready;
    public string username;
    public GameObject obj;
    public CharacterType type;
    public PlayerData(bool emp = true, int id = 0, bool status = false, GameObject gameObject = null, CharacterType character = CharacterType.Enginner, string name = "")
    {
        empty = emp;
        uid = id;
        ready = status;
        obj = gameObject;
        type = character;
        username = name;
    }

}


public struct DoorData
{
    public int doorNum;
    public Vector3 transferPos;
    public DoorData(int _doorNum, Vector3 _transferPos)
    {
        doorNum = _doorNum;
        transferPos = _transferPos;
    }
}

public struct TreasureData
{
    public int treasureId;
    public TreasureType treasuretType;
    public GameObject treasureObejct;
    public TreasureData(int _treasureId, TreasureType _treasureType, GameObject _treasureObject)
    {
        treasureId = _treasureId;
        treasuretType = _treasureType;
        treasureObejct = _treasureObject;
    }
}



[System.Serializable]
public class Door
{
    public GameObject upDoor;
    public GameObject downDoor;
    public GameObject leftDoor;
    public GameObject rightDoor;
}

public enum TreasureType
{
    Buff = 1,
    Penetrate = 201,
    Sputtering = 202,
    LightningChain = 203,
    Freeze = 204,
    Poision = 205,
    Burn = 206,
    Dizziness = 207,
    Retard = 208,
    Bigger = 209,
    Smaller = 210,
    Longer = 211,
    Initiative = 3
}