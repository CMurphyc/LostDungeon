using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using Login;
using Room;
public class ClientSend 
{
    TcpClient remoteServer;
    public ClientSend(TcpClient sock)
    {
        remoteServer = sock;
    }
    void Send(byte[] data)
    {
        if (remoteServer.Connected)
        {

            NetworkStream ns = remoteServer.GetStream();
            if (ns.CanWrite)
            {
                ns.Write(data, 0, data.Length);
            }
        }
    }
    public void RegisterC2S(string UserName, string Passward, string NickName)
    {

        LoginC2S pack = new LoginC2S();
        pack.Username = UserName;
        pack.Password = Passward;
        pack.Nickname = NickName;
        byte[] databytes = pack.ToByteArray();
        byte[] init = PackConverter.intToBytes(GeneralType.UserRegister);
        byte[] final = new byte[databytes.Length + init.Length];
        init.CopyTo(final, 0);
        databytes.CopyTo(final, init.Length);
        Send(final);

    }
    public void LoginC2S(string UserName, string Passward)
    {
        LoginC2S pack = new LoginC2S();
        pack.Username = UserName;
        pack.Password = Passward;
        byte[] databytes = pack.ToByteArray();
        byte[] init = PackConverter.intToBytes(GeneralType.UserLogin);
        byte[] final = new byte[databytes.Length + init.Length];
        init.CopyTo(final, 0);
        databytes.CopyTo(final, init.Length);
        Send(final);
    }
    public void CreateRoomC2S(string username,int roomsize)
    {
        CreateRoomC2S pack = new CreateRoomC2S();
        pack.Owner = username;
        pack.Roomsize = roomsize;
        byte[] databytes = pack.ToByteArray();
        byte[] init = PackConverter.intToBytes(GeneralType.CreateGame);
        byte[] final = new byte[databytes.Length + init.Length];
        init.CopyTo(final, 0);
        databytes.CopyTo(final, init.Length);
        Send(final);
    }
    public void GetRoomListC2S()
    {
        GetRoomListC2S pack = new GetRoomListC2S();
        byte[] databytes = pack.ToByteArray();
        byte[] init = PackConverter.intToBytes(GeneralType.GetRoomList);
        byte[] final = new byte[databytes.Length + init.Length];
        init.CopyTo(final, 0);
        databytes.CopyTo(final, init.Length);
        Send(final);

    }
    public void EnterRoomC2S(string username, int roomid)
    {
        EnterRoomC2S pack = new EnterRoomC2S();
        pack.Username = username;
        pack.Roomid = roomid;
        byte[] databytes = pack.ToByteArray();
        byte[] init = PackConverter.intToBytes(GeneralType.EnterRoom);
        byte[] final = new byte[databytes.Length + init.Length];
        init.CopyTo(final, 0);
        databytes.CopyTo(final, init.Length);
        Send(final);
    }

    public void PlayerReadyC2S(string username , int status)
    {
        PlayerReadyC2S pack = new PlayerReadyC2S();
        pack.Username = username;
        pack.Status = status;
        byte[] databytes = pack.ToByteArray();
        byte[] init = PackConverter.intToBytes(GeneralType.PlayerReady);
        byte[] final = new byte[databytes.Length + init.Length];
        init.CopyTo(final, 0);
        databytes.CopyTo(final, init.Length);
        Send(final);
    }

    public void RoomOwnerStartC2S(string username)
    {
        RoomOwnerStartC2S pack = new RoomOwnerStartC2S();
        pack.Ownerid = username;
        
        byte[] databytes = pack.ToByteArray();
        byte[] init = PackConverter.intToBytes(GeneralType.RoomOwnerStart);
        byte[] final = new byte[databytes.Length + init.Length];
        init.CopyTo(final, 0);
        databytes.CopyTo(final, init.Length);
        Send(final);
    }

}
