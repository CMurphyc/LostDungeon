using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataModule
{
    BattleManager _parentManager;

    public Dictionary<int, PlayerInGameData> playerToPlayer = new Dictionary<int, PlayerInGameData>();   // 玩家编号对应玩家信息

    public List<BattleInput> frameInfo;

    public PlayerDataModule(BattleManager parent)
    {
        _parentManager = parent;
    }


    public void UpdateLogic(int frame)
    {
        for (int i = 0; i < frameInfo.Count;i++)
        {
            if (playerToPlayer.ContainsKey(frameInfo[i].Uid))
            {
                PlayerInGameData Input = playerToPlayer[frameInfo[i].Uid];

                Vector2 MoveVec = new Vector2(frameInfo[i].MoveDirectionX/10000f, frameInfo[i].MoveDirectionY/10000f).normalized * Global.FrameRate/1000f;
                Input.obj.GetComponent<PlayerModel_Component>().Move(MoveVec);
            }
        }
    }
    public void UpdateView()
    {
        for (int i = 0; i < frameInfo.Count; i++)
        {
            if (playerToPlayer.ContainsKey(frameInfo[i].Uid))
            {
                PlayerInGameData Input = playerToPlayer[frameInfo[i].Uid];

                Vector2 MoveVec = new Vector2(frameInfo[i].MoveDirectionX / 10000f, frameInfo[i].MoveDirectionY / 10000f);
                Input.obj.GetComponent<PlayerView_Component>().RefreshView();
            }

        }
    }


    public HashSet<int> GetLiveRoom()
    {
        HashSet<int> LiveRoomList = new HashSet<int>();
        foreach( var item in playerToPlayer)
        {
            LiveRoomList.Add(item.Value.RoomID);
        }
        return LiveRoomList;
    }

    public List<PlayerInGameData> FindPlayerInRoom(int roomid)
    {
        List<PlayerInGameData> ret = new List<PlayerInGameData>();

        foreach(var item in playerToPlayer)
        {
            if (item.Value.RoomID == roomid)
            {
                ret.Add(item.Value);
            }

        }
        return ret;
    }
}
