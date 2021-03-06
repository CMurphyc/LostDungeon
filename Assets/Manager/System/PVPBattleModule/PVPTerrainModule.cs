﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPTerrainModule : MonoBehaviour
{
    PVPBattleManager _pvp;


    public Dictionary<int, List<GameObject>> roomToStone = new Dictionary<int, List<GameObject>>();     // 房间号对应的石头列表
    public Dictionary<int, List<int>> roomToDoor = new Dictionary<int, List<int>>();                    // 房间号对应的门列表
    public Dictionary<int, GameObject> doornumToDoor = new Dictionary<int, GameObject>();              // 门号对应门的实体
    public Dictionary<int, DoorData> doorToDoor = new Dictionary<int, DoorData>();                     // 一个编号的门传送到的另一个门的编号
    public Dictionary<int, int> doorToRoom = new Dictionary<int, int>();                               // 一个编号的门对应的房间编号
    CollideDetecter collideDetecter = new CollideDetecter();

    public FixVector2 BossSpawningPoint = new FixVector2();

    bool NextFloorInit = false;
    private GameObject NextFloor_Instance;

    bool InGateRange = false;
    bool GameOverInit = false;


    public PVPTerrainModule(PVPBattleManager parent)
    {
        _pvp = parent;
    }
    public void Free()
    {
        roomToStone.Clear();
        roomToDoor.Clear();
        doornumToDoor.Clear();
        doorToDoor.Clear();
        doorToRoom.Clear();
        NextFloorInit = false;
        InGateRange = false;
        GameOverInit = false;
    }

    public bool IsMovable(Polygon poly, int RoomId)
    {

        foreach (GameObject stone in roomToStone[RoomId])
        {
            BoxCollider2D collider = stone.GetComponent<BoxCollider2D>();
            Rectangle rect = new Rectangle(new FixVector2((Fix64)stone.transform.position.x + (Fix64)collider.offset.x,
                                                          (Fix64)stone.transform.position.y + (Fix64)collider.offset.y),
                new FixVector2((Fix64)stone.transform.rotation.x, (Fix64)stone.transform.rotation.y),
                (Fix64)(stone.GetComponent<BoxCollider2D>().size.x),
                (Fix64)(stone.GetComponent<BoxCollider2D>().size.y)
                );
            //Debug.Log("anchor is " + rect.anchor + "offsetX is " + rect.horizon + "offsety is " + rect.vertical );

            switch (poly.type)
            {
                case PolygonType.Point:
                    {
                        if (collideDetecter.PointInRectangle(poly.Point, rect))
                        {
                            return false;
                        }
                        break;
                    }
                case PolygonType.Rectangle:
                    {
                        if (collideDetecter.RectangleCollideRectangle(poly.Rect, rect))
                        {
                            return false;
                        }
                        break;
                    }

                case PolygonType.Circle:
                    {

                        if (collideDetecter.CircleCollideRect(poly.circle, rect))
                        {
                            return false;
                        }
                        break;
                    }
            }

            //if (collideDetecter.PointInRectangle(pos, rect))
            //{
            //    //Debug.Log("mememe pos is " + pos + " " + "collide anchor is " + rect.anchor + " " + "horizon is " + rect.horizon + " " + "vertical is " + rect.vertical);
            //    return false;
            //}
            //else
            //{
            //    //Debug.Log("mememe pos is " + pos + " " + "dis anchor is " + rect.anchor + " " + "horizon is " + rect.horizon + " " + "vertical is " + rect.vertical);
            //}
        }
        return true;

    }



    public void UpdateLogic(int frame)
    {
        DoorTeleport();
        updateCurtain();
        ToPVPResult();
    }
    /*
    public void getTreasure()
    {
        int PlayerUID = _parentManager.sys._model._PlayerModule.uid;
        GameObject p = _parentManager.sys._battle._player.playerToPlayer[PlayerUID].obj;
        foreach
    }
    */
    public void updateCurtain()
    {
        /*
        GameObject t = GameObject.Find("RoomCreate");
        foreach (var x in _pvp._pvpplayer.playerToPlayer)
        {
            int PlayerUID = x.Key;
            int roomID = _pvp._pvpplayer.playerToPlayer[PlayerUID].RoomID;
            if (t != null)
            {
                foreach (var px in t.GetComponent<RoomCreate>().roomToCurtain)
                {
                    if (px.Key == roomID) px.Value.SetActive(false);
                }
            }
        }
        */
    }


    public void UpdateView()
    {



    }

    void ToPVPResult()
    {
        
    }

    void DoorTeleport()
    {
        foreach (KeyValuePair<int, PlayerInGameData> x in _pvp._pvpplayer.playerToPlayer)//判断传送门
        {
            int RoomId = x.Value.RoomID;
            //Debug.Log("RoomId = " + RoomId);
            foreach (int p in roomToDoor[RoomId])
            {
                FixVector2 doorAnchor = new FixVector2((Fix64)doornumToDoor[p].transform.position.x,
                                                        (Fix64)doornumToDoor[p].transform.position.y);
                FixVector2 pos = x.Value.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition();
                /*
                Debug.Log("-------------------");
                Debug.Log(FixVector2.Distance(pos, doorAnchor));
                Debug.Log("-------------------");
                */
                if (FixVector2.Distance(pos, doorAnchor) <= (Fix64)1.2f)//这里加一个monstermodule的是否没怪
                {
                    //&& ( _parentManager._monster.GetMonsterNumber(RoomId))==0

                    // && _parentManager._monster.GetMonsterNumber()==0
                    DoorData To = doorToDoor[p];//编号 位置

                    FixVector2 telPos = new FixVector2((Fix64)To.transferPos.x, (Fix64)To.transferPos.y);
                    //Debug.Log(RoomId);
                    //x.Value.obj.GetComponent<PlayerModel_Component>().Move(telPos-x.Value.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition());
                    x.Value.obj.GetComponent<PlayerModel_Component>().SetPosition(telPos);
                    _pvp._pvpplayer.playerToPlayer[x.Key].ChangeRoomID(doorToRoom[To.doorNum]);
                    /*
                    if (GameObject.Find("Canvas").GetComponent<MeleeSmallMap>() != null&&_pvp._pvpplayer.FindPlayerTeamByUID(x.Key)==_pvp._pvpplayer.FindCurrentPlayerTeam())
                    {
                        GameObject.Find("Canvas").GetComponent<MeleeSmallMap>().ChangeRoom(RoomId, doorToRoom[To.doorNum]);
                    }
                    */
                    break;
                }

            }
        }


    }
}
