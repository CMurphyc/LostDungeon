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

        Debug.Log("Send Length: " +data.Length);
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
        Send(pack.ToByteArray(), GeneralType.UserRegister);

    }
    public void LoginC2S(string UserName, string Passward)
    {
        LoginC2S pack = new LoginC2S();
        pack.UserName = UserName;
        pack.Password = Passward;
        Send(pack.ToByteArray(), GeneralType.UserLogin);
    }

    public void CreateRoomC2S()
    {
        CreateRoomC2S pack = new CreateRoomC2S();
        Send(pack.ToByteArray(), GeneralType.CreateGame);
    }

    public void ChangeCharacter( CharacterType Type)
    {
        Debug.Log("Chagne Charcter");
        //Change
        ChangeRoleC2S pack = new ChangeRoleC2S();
        pack.Role = (Role)Type;
        Send(pack.ToByteArray(), GeneralType.ChangeCharacter);

    }
    public void GetRoomList()
    {
        GetRoomListC2S pack = new GetRoomListC2S();
        Send(pack.ToByteArray(), GeneralType.GetRoomList);


    }
    public void PlayerReady()
    {
        PlayerReadyC2S pack = new PlayerReadyC2S();

        Send(pack.ToByteArray(), GeneralType.PlayerReady);

    }

    public void PlayerStartGame()
    {
        StartGameC2S pack = new StartGameC2S();
        Send(pack.ToByteArray(), GeneralType.StartGame);
    }

    public void PlayerExitRoom()
    {
        LeaveRoomC2S pack = new LeaveRoomC2S();
        Send(pack.ToByteArray(), GeneralType.PlayerLeaveRoom);
    }

    public void PlayerEnterRoom(int RoomID)
    {
        EnterRoomC2S pack = new EnterRoomC2S();
        pack.RoomId = RoomID;
        Send(pack.ToByteArray(), GeneralType.EnterRoom);
    }
}
