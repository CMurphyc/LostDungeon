using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using Google.Protobuf;

public class ClientSend 
{
    TcpClient remoteServer;
    public ClientSend(TcpClient sock)
    {
        remoteServer = sock;
    }
    //void Send(byte[] data)
    //{
    //    if (remoteServer.Connected)
    //    {

    //        NetworkStream ns = remoteServer.GetStream();
    //        if (ns.CanWrite)
    //        {
    //            ns.Write(data, 0, data.Length);
    //        }
    //    }
    //}

    void Send(byte[] data, int ProtoType)
    {
        byte[] init = PackConverter.intToBytes(ProtoType);
        byte[] databytes = data;
        byte[] length_pack = PackConverter.intToBytes(data.Length);

        //Debug.Log("Send Length: " +data.Length);
        byte[] final = new byte[databytes.Length + init.Length + length_pack.Length];
        //pack.CalculateSize()

        init.CopyTo(final, 0);
        length_pack.CopyTo(final, init.Length);
        databytes.CopyTo(final, init.Length + length_pack.Length);

        if (remoteServer.Connected)
        {

            NetworkStream ns = remoteServer.GetStream();
            if (ns.CanWrite)
            {
                ns.Write(final, 0, final.Length);
            }
        }

    }
    public void RegisterC2S(string UserName, string Passward, string NickName)
    {

        LoginC2S pack = new LoginC2S();
       
        pack.UserName = UserName;
        pack.Password = Passward;
        //pack.Nickname = NickName;
        Send(pack.ToByteArray(), GeneralType.UserRegisterC2S);

    }
    public void LoginC2S(string UserName, string Passward)
    {
        LoginC2S pack = new LoginC2S();
        pack.UserName = UserName;
        pack.Password = Passward;
        Send(pack.ToByteArray(), GeneralType.UserLoginC2S);
    }

    public void CreateRoomC2S(RoomType roomType)
    {
        CreateRoomC2S pack = new CreateRoomC2S();
        pack.RoomType = roomType;
        Send(pack.ToByteArray(), GeneralType.CreateRoomC2S);
        Debug.Log("send CreateRoom pack: " + pack.ToString());
    }

    public void ChangeCharacter(CharacterType Type)
    {
        Debug.Log("Chagne Charcter");
        //Change
        ChangeRoleC2S pack = new ChangeRoleC2S();
        pack.Role = (Role)Type;
        Send(pack.ToByteArray(), GeneralType.ChangeRoleC2S);

    }

    public void ChangeRunes(int runes)
    {
        Debug.Log("Change runes");
        ChangeRunesC2S pack = new ChangeRunesC2S();
        pack.Runes = runes;
        Send(pack.ToByteArray(), GeneralType.ChangeRunesC2S);
    }

    public void ChangeFaction(int faction)
    {
        Debug.Log("Change faction");
        ChangeFactionC2S pack = new ChangeFactionC2S();
        pack.Faction = faction;
        Send(pack.ToByteArray(), GeneralType.ChangeFactionC2S);
    }

    public void GetRoomList(RoomType roomType)
    {
        GetRoomListC2S pack = new GetRoomListC2S();
        pack.RoomType = roomType;
        Send(pack.ToByteArray(), GeneralType.GetRoomListC2S);
        // Debug.Log("send GetRoomList pack: " + pack.ToString());
    }
    public void PlayerReady()
    {
        PlayerReadyC2S pack = new PlayerReadyC2S();

        Send(pack.ToByteArray(), GeneralType.PlayerReadyC2S);

    }

    public void PlayerStartGame()
    {
        StartGameC2S pack = new StartGameC2S();
        Send(pack.ToByteArray(), GeneralType.StartGameC2S);
    }

    public void PlayerExitRoom()
    {
        LeaveRoomC2S pack = new LeaveRoomC2S();
        Send(pack.ToByteArray(), GeneralType.PlayerLeaveRoomC2S);
    }

    public void PlayerEnterRoom(int RoomID)
    {
        EnterRoomC2S pack = new EnterRoomC2S();
        pack.RoomId = RoomID;
        Send(pack.ToByteArray(), GeneralType.EnterRoomC2S);
    }

    public void BattleSynC2S(Vector3 Left, Vector3 Right, AttackType type)
    {
        BattleInput pack = new BattleInput();
        //导入信息 To Do

        pack.MoveDirectionX = (int)(Left.x *10000);
        pack.MoveDirectionY = (int)(Left.y *10000);


        pack.AttackDirectionX = (int)(Right.x * 10000);
        pack.AttackDirectionY = (int)(Right.y * 10000);

        pack.AttackType = (int)type;

      /* 
        Debug.Log("AttackDirX  " + pack.AttackDirectionX);
        Debug.Log("AttackDirY  " + pack.AttackDirectionY);
        Debug.Log("Attack Type  " + pack.AttackType);
     */
        //if (Right.x == 0 && Right.y == 0)
        //{
        //    pack.AttackType = 0;
        //}
        //else
        //{
        //    pack.AttackType = 1;
        //    pack.AttackDirectionX = (int)(Right.x * 10000);
        //    pack.AttackDirectionY = (int)(Right.y * 10000);
        //}
        //pack.AttackType = ;
        Send(pack.ToByteArray(), GeneralType.BattleSyncC2S);
    }

    public void StartSync()
    {
        StartGameC2S pack = new StartGameC2S();
        Send(pack.ToByteArray(), GeneralType.StartSyncC2S);
        Debug.Log("Send StartSync pack");
    }

    public void NextFloor(int floorNumber)
    {
        NextFloorC2S pack = new NextFloorC2S();
        pack.FloorNumber = floorNumber;
        Send(pack.ToByteArray(), GeneralType.NextFloorC2S);
        Debug.Log("Send NextFloor pack");
    }

    public void GameOver()
    {
        GameOverC2S pack = new GameOverC2S();
        Send(pack.ToByteArray(), GeneralType.GameOverC2S);
        Debug.Log("Send GameOver pack");
    }
}
