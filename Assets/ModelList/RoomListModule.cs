using UnityEngine;
using System.Collections;

public class RoomListModule
{

    public GetRoomListS2C RoomListPack;
    public bool NeedUpdate = false;
    public RoomType roomType;

    public void ResetRoomList()
    {
        RoomListPack.RoomsInfo.Clear();
    }
}
