using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomModule 
{
    public int MapSeed ;
    public int MapFloorNumber;
    public int MaxMapFloorNumber;
    public bool isLoadingCompleted = false;
    public bool isOver = false;
    public bool NeedUpdate = false;
    ModelManager model;
    public RoomModule(ModelManager parent)
    {
        model = parent;
    }

    public int roomid;

    public int roomOwnerID;
    //public List<PlayerData> PlayerList = new List<PlayerData> { new PlayerData{ }, new PlayerData { }, new PlayerData { }, new PlayerData { } };
    public List<PlayerData> PlayerList = new List<PlayerData> { new PlayerData { }, new PlayerData { }, new PlayerData { }, new PlayerData { }, new PlayerData { }, new PlayerData { }, new PlayerData { }, new PlayerData { }, new PlayerData { }, new PlayerData { } };


    // public List<PlayerData> PVPPlayerList = new List<PlayerData> { new PlayerData { }, new PlayerData { }, new PlayerData { }, new PlayerData { }, new PlayerData { }, new PlayerData { }, new PlayerData { }, new PlayerData { }, new PlayerData { }, new PlayerData { } };

    public List<PlayerData> RedTeamPlayerList = new List<PlayerData>();
    public List<PlayerData> BlueTeamPlayerList = new List<PlayerData>();
    public int PVEGameTime;

    public List<GameObject> PlayerAnimation = new List<GameObject>();

    public Dictionary<int, PVEData> PVEResult = new Dictionary<int, PVEData>();
    public Dictionary<int, PVPData> PVPResult = new Dictionary<int, PVPData>();


    public void SortTeam()
    {
        RedTeamPlayerList.Clear();
        BlueTeamPlayerList.Clear();

        for (int i =0; i < PlayerList.Count;i++)
        {
            if (PlayerList[i].empty) continue;
            if (PlayerList[i].team == TeamSide.Red )
            {
                RedTeamPlayerList.Add(PlayerList[i]);
            }
            else
            {
                BlueTeamPlayerList.Add(PlayerList[i]);
            }
        }

        //Debug.Log("SIZE :" + RedTeamPlayerList.Count);
        //Debug.Log("SIZE :" + BlueTeamPlayerList.Count);


        FillRest();
    }
    public void FillRest()
    {
        int redCount = RedTeamPlayerList.Count;

        for (int i = redCount; i <5;i++ )
        {
            PlayerData temp = new PlayerData();
            temp.empty = true;
            RedTeamPlayerList.Add(temp);
        }
        int blueCount = BlueTeamPlayerList.Count;
        for (int i = blueCount; i < 5; i++)
        {
            PlayerData temp = new PlayerData();
            temp.empty = true;
            BlueTeamPlayerList.Add(temp);
        }


    }


    public int GetPlayerSize()
    {
        int ret = 0;
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (!PlayerList[i].empty)
            {
                ret++;
            }
        }
        return ret;

    }

    public int GetMinIndex()
    {

        for (int i = 0; i < PlayerList.Count;i++)
        {
            if (PlayerList[i].empty)
                return i;
        }
        return -1;

    }


    public int GetPlayerIndex(int uid)
    {
        int ret = -1;
        for (int i = 0; i < PlayerList.Count;i++ )
        {
            if (PlayerList[i].uid==uid)
            {
                return i;
            }

        }
        
        return ret;

    }

    public void RemoveAllPlayer()
    {
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (!PlayerList[i].empty )
            {
                PlayerData temp = PlayerList[i];
                temp.empty = true;
                temp.uid = 0;
                temp.ready = false;
                Object.Destroy(temp.obj);
                temp.obj = null;
                PlayerList[i] = temp;
               

            }
        }

        for (int i = 0; i< PlayerAnimation.Count;i++)
        {

            Object.Destroy(PlayerAnimation[i]);


        }
        PlayerAnimation.RemoveAll(it=> PlayerAnimation.Contains(it));
       
    }


    public void Add_Player(PlayerInfo playerinfo)
    {
        for (int i = 0; i < PlayerList.Count;i++)
        {
            if (PlayerList[i].empty)
            {
             
                PlayerData temp = PlayerList[i];
                temp.uid = playerinfo.PlayerId;
                temp.ready = playerinfo.IsReady;
                temp.type = (CharacterType)playerinfo.Role;
                temp.empty = false;
                temp.username = playerinfo.UserName;
                temp.team = playerinfo.Faction == 0 ? TeamSide.Blue : TeamSide.Red;
                PlayerList[i] = temp;

                if (PVEResult.ContainsKey(playerinfo.PlayerId))
                {
                    PVEResult.Remove(playerinfo.PlayerId);
                }
                PVEResult.Add(playerinfo.PlayerId, new PVEData());

                if (PVPResult.ContainsKey(playerinfo.PlayerId))
                {
                    PVPResult.Remove(playerinfo.PlayerId);
                }
                PVPResult.Add(playerinfo.PlayerId, new PVPData());

                //Debug.Log("PlayerUid: " + temp.uid);
                //Debug.Log("PlayerReady: " + temp.ready);
                //Debug.Log("PlayerType: " + temp.type);
                //UI
                //NickName
                //Character
                //Ready

                break;
            }
        }
    }

    public void Delete_Player(int uid)
    {
        int DeleteIndex= -1;
        bool DeleteFromList = false;
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (!PlayerList[i].empty && PlayerList[i].uid == uid)
            {
                PlayerData temp = PlayerList[i];
                temp.empty = true;
                temp.uid = 0;
                temp.ready = false;
                Object.Destroy(temp.obj);
                temp.obj = null;
                PlayerList[i] = temp;
                DeleteFromList = true;
                DeleteIndex = i;


                //UI
                //NickName
                //Character
                //Ready

                break;
            }
        }
        if (DeleteFromList)
        {
            PlayerAnimation.RemoveAt(DeleteIndex);

        }
    }

    public CharacterType GetCharacterType(int uid)
    {
       for (int i =0; i < PlayerList.Count; i++)
       {
            if (PlayerList[i].uid == uid)
                return PlayerList[i].type;
       }
        return CharacterType.None;
    }

    public bool IsLoadingCompleted()
    {
        if (isLoadingCompleted)
        {
            isLoadingCompleted = false;
            return true;
        }
        return false;
    }

}
