using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using Google.Protobuf;

public class ClientListen : MonoBehaviour
{
    GameObject main;
    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.FindWithTag("GameEntry");
    }

    // Update is called once per frame
    void Update()
    {
        EventDispatcher.Instance().OnTick();
    }
  
    TcpClient remoteServer;
    byte[] MemoryData = new byte[10000000];
    int Memory_Length = 0;
    //EventDispatcher.Instance().RegistEventListener("UpdateFrame", UpdateFrame);
    //EventDispatcher.Instance().DispatchEvent("ReFrame", 1);
    //EventDispatcher.Instance().OnTick();
    public ClientListen(TcpClient sock)
    {
        remoteServer = sock;
        main = GameObject.FindWithTag("GameEntry");
    }
    public void Received()
    {
        while (true)
        {
            if (remoteServer.Connected)
            {
                NetworkStream ns = remoteServer.GetStream();

                if (ns.CanRead && ns.DataAvailable)
                {
                    byte[] buffer = new byte[remoteServer.Available];

                    ns.Read(buffer, 0, buffer.Length);


                    int total_length = buffer.Length + Memory_Length;
                    int current_index = 0;
                    byte[] new_buffer = new byte[total_length];

                    for (int i = 0; i < Memory_Length; i++)
                    {
                        new_buffer[i] = MemoryData[i];

                    }
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        new_buffer[i + Memory_Length] = buffer[i];

                    }

                    while (current_index < total_length)
                    {
                        if ((total_length - current_index) < 8)
                        {
                            Memory_Length = total_length - current_index;
                            for (int i = 0; i < Memory_Length; i++)
                            {
                                MemoryData[i] = new_buffer[current_index + i];


                            }
                            //退出
                            break;
                        }

                        byte[] tag = new byte[4];

                        for (int i = 0; i < tag.Length; i++)
                        {
                            tag[i] = new_buffer[current_index + i];
                        }

                        int tagInt = PackConverter.bytesToInt(tag);

                        byte[] pack_sz = new byte[4];

                        for (int i = 0; i < pack_sz.Length; i++)
                        {
                            pack_sz[i] = new_buffer[current_index + 4 + i];
                        }

                        int pack_size = BitConverter.ToInt32(pack_sz, 0);

                        if (current_index + 8 + pack_size < total_length)
                        {
                            //有残余

                            byte[] CompeletePack = new byte[8 + pack_size];

                            for (int i = 0; i < CompeletePack.Length; i++)
                            {
                                CompeletePack[i] = new_buffer[i + current_index];

                            }
                            HandleMsg(tagInt, CompeletePack, pack_size);


                            current_index += (8 + pack_size);
                        }
                        else if (current_index + 8 + pack_size == total_length)
                        {
                            byte[] CompeletePack = new byte[8 + pack_size];

                            for (int i = 0; i < CompeletePack.Length; i++)
                            {
                                CompeletePack[i] = new_buffer[i + current_index];

                            }
                            HandleMsg(tagInt, CompeletePack, pack_size);
                            //正好相等 

                            Memory_Length = 0;
                            current_index += (8 + pack_size);
                        }
                        else
                        {
                            //不够解析
                            Memory_Length = total_length - current_index;

                            for (int i = 0; i < Memory_Length; i++)
                            {
                                MemoryData[i] = new_buffer[current_index + i];

                            }



                            //退出
                            break;


                        }

                    }


                }
            }
        }
    }

    void HandleMsg(int tag, byte[] pack , int pack_size)
    {
        //Debug.Log("Tag: " + tag);
        //Debug.Log("PackSize: "+pack_size);
        if (tag == GeneralType.UserLoginS2C)
        {
            IMessage IMPlayersPack = new LoginS2C();
            LoginS2C synPack = new LoginS2C();
            synPack = (LoginS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
            EventDispatcher.Instance().DispatchEvent(EventMessageType.UserLogin, synPack);
        }

        else if (tag == GeneralType.CreateRoomS2C)
        {
            IMessage IMPlayersPack = new CreateRoomS2C();
            CreateRoomS2C synPack = new CreateRoomS2C();
            synPack = (CreateRoomS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
            EventDispatcher.Instance().DispatchEvent(EventMessageType.CreateGame, synPack);
        }
        else if (tag == GeneralType.GetRoomInfoS2C)
        {
            IMessage IMPlayersPack = new GetRoomInfoS2C();
            GetRoomInfoS2C synPack = new GetRoomInfoS2C();
            synPack = (GetRoomInfoS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
            EventDispatcher.Instance().DispatchEvent(EventMessageType.GetRoomInfo, synPack);
        }
        else if (tag == GeneralType.GetRoomListS2C)
        {
            IMessage IMPlayersPack = new GetRoomListS2C();
            GetRoomListS2C synPack = new GetRoomListS2C();
            synPack = (GetRoomListS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
            EventDispatcher.Instance().DispatchEvent(EventMessageType.GetRoomList, synPack);
        }
        else if (tag == GeneralType.PlayerLeaveRoomS2C)
        {
            IMessage IMPlayersPack = new LeaveRoomS2C();
            LeaveRoomS2C synPack = new LeaveRoomS2C();
            synPack = (LeaveRoomS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
            EventDispatcher.Instance().DispatchEvent(EventMessageType.LeaveRoom, synPack);
        }
        else if (tag == GeneralType.StartGameS2C)
        {
            IMessage IMPlayersPack = new StartGameS2C();
            StartGameS2C synPack = new StartGameS2C();
            synPack = (StartGameS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
            EventDispatcher.Instance().DispatchEvent(EventMessageType.StartGame, synPack);
        }
        else if (tag == GeneralType.BattleSyncS2C)
        {
            IMessage IMPlayersPack = new BattleFrame();
            BattleFrame synPack = new BattleFrame();
            synPack = (BattleFrame)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
            EventDispatcher.Instance().DispatchEvent(EventMessageType.BattleSyn, synPack);
        }
        else if (tag == GeneralType.StartSyncS2C)
        {
            IMessage IMPlayersPack = new StartSyncS2C();
            StartSyncS2C synPack = new StartSyncS2C();
            synPack = (StartSyncS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
            EventDispatcher.Instance().DispatchEvent(EventMessageType.StartSync, synPack);
        }
        else if(tag == GeneralType.NextFloorS2C)
        {
            IMessage IMPlayersPack = new NextFloorS2C();
            NextFloorS2C synPack = new NextFloorS2C();
            synPack = (NextFloorS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
            EventDispatcher.Instance().DispatchEvent(EventMessageType.NextFloor, synPack);
        }
        else if(tag == GeneralType.GameOverS2C)
        {
            IMessage IMPlayersPack = new GameOverS2C();
            GameOverS2C synPack = new GameOverS2C();
            synPack = (GameOverS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
            EventDispatcher.Instance().DispatchEvent(EventMessageType.GameOver, synPack);
        }


    }


}
