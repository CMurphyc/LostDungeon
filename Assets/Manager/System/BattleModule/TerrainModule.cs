using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainModule 
{
    BattleManager _parentManager;


    public Dictionary<int, List<GameObject>> roomToStone = new Dictionary<int, List<GameObject>>();     // 房间号对应的石头列表
    public Dictionary<int, List<int>> roomToDoor = new Dictionary<int, List<int>>();                    // 房间号对应的门列表
    public Dictionary<int, GameObject> doornumToDoor = new Dictionary<int, GameObject>() ;              // 门号对应门的实体
    public Dictionary<int, DoorData> doorToDoor = new Dictionary<int, DoorData>() ;                     // 一个编号的门传送到的另一个门的编号
    public Dictionary<int, int> doorToRoom = new Dictionary<int, int>() ;                               // 一个编号的门对应的房间编号
    CollideDetecter collideDetecter = new CollideDetecter();
    public TerrainModule(BattleManager parent)
    {
        _parentManager = parent;
    }


    public bool IsMovable(FixVector2 pos,int RoomId)
    {

        foreach (GameObject stone in roomToStone[RoomId])
        {
            BoxCollider2D collider = stone.GetComponent<BoxCollider2D>();
            Rectangle rect = new Rectangle(new FixVector2((Fix64)(stone.transform.position.x + collider.offset.x), 
                                                          (Fix64)(stone.transform.position.y + collider.offset.y)),
                new FixVector2((Fix64)stone.transform.rotation.x, (Fix64)stone.transform.rotation.y),
                (Fix64)(stone.GetComponent<BoxCollider2D>().size.x),
                (Fix64)(stone.GetComponent<BoxCollider2D>().size.y)
                );
            //Debug.Log("anchor is " + rect.anchor + "offsetX is " + rect.horizon + "offsety is " + rect.vertical );
            if (collideDetecter.PointInRectangle(pos, rect))
            {
                //Debug.Log("mememe pos is " + pos + " " + "collide anchor is " + rect.anchor + " " + "horizon is " + rect.horizon + " " + "vertical is " + rect.vertical);
                return false;
            }
            else
            {
                //Debug.Log("mememe pos is " + pos + " " + "dis anchor is " + rect.anchor + " " + "horizon is " + rect.horizon + " " + "vertical is " + rect.vertical);
            }
        }
        return true;

    }


    public void UpdateLogic(int frame)
    {
        foreach(KeyValuePair<int, PlayerInGameData> x in _parentManager._player.playerToPlayer)//判断传送门
        {
            int RoomId = x.Value.RoomID;
            //Debug.Log("RoomId = " + RoomId);
            foreach(int p in roomToDoor[RoomId])
            {
                FixVector2 doorAnchor = new FixVector2((Fix64)doornumToDoor[p].transform.position.x, 
                                                        (Fix64)doornumToDoor[p].transform.position.y);
                FixVector2 pos = x.Value.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition();
                /*
                Debug.Log("-------------------");
                Debug.Log(FixVector2.Distance(pos, doorAnchor));
                Debug.Log("-------------------");
                */
                if (FixVector2.Distance(pos, doorAnchor) <= (Fix64)0.8f &&  _parentManager._monster.GetMonsterNumber(RoomId)==0)//这里加一个monstermodule的是否没怪
                {
                    //&& ( _parentManager._monster.GetMonsterNumber(RoomId))==0
                  
                    // && _parentManager._monster.GetMonsterNumber()==0
                    DoorData To = doorToDoor[p];//编号 位置
                    
                    FixVector2 telPos = new FixVector2((Fix64)To.transferPos.x, (Fix64)To.transferPos.y);
                    //Debug.Log(RoomId);
                    //x.Value.obj.GetComponent<PlayerModel_Component>().Move(telPos-x.Value.obj.GetComponent<PlayerModel_Component>().GetPlayerPosition());
                    x.Value.obj.GetComponent<PlayerModel_Component>().SetPosition(telPos);
                    _parentManager._player.playerToPlayer[x.Key].ChangeRoomID(doorToRoom[To.doorNum]);
                 
                    break;
                }
       
            }
        }

    }
    public void UpdateView()
    {



    }
}
