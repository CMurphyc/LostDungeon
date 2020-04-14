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
        if (tag == GeneralType.UserLogin + 100)
        {
            IMessage IMPlayersPack = new LoginS2C();
            LoginS2C synPack = new LoginS2C();
            synPack = (LoginS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
            EventDispatcher.Instance().DispatchEvent(EventMessageType.UserLogin, synPack);
        }

        else if (tag == GeneralType.CreateGame + 100)
        {
            //IMessage IMPlayersPack = new CreateRoomS2C();
            //CreateRoomS2C synPack = new CreateRoomS2C();
            //synPack = (CreateRoomS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
            //EventDispatcher.Instance().DispatchEvent(EventMessageType.CreateGame, synPack);
            Debug.Log("房间数量超出限制");
        }
        else if (tag == 113)
        {
            
            IMessage IMPlayersPack = new GetRoomInfoS2C();
            GetRoomInfoS2C synPack = new GetRoomInfoS2C();
            synPack = (GetRoomInfoS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
            EventDispatcher.Instance().DispatchEvent(EventMessageType.CreateGame, synPack);



        }
        



        //else if (tag == GeneralType.UserRegister+ 100)
        //{
        //    IMessage IMPlayersPack = new Login.LoginS2C();
        //    Login.LoginS2C synPack = new Login.LoginS2C();
        //    synPack = (Login.LoginS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
        //    if (synPack.Succeed)
        //    {
        //        EventDispatcher.Instance().DispatchEvent("Register", true);
        //    }
        //    else
        //    {
        //        EventDispatcher.Instance().DispatchEvent("Register", false);
        //    }
        //}
        //else if (tag == GeneralType.CreateGame + 100)
        //{
        //    IMessage IMPlayersPack = new CreateRoomS2C();
        //    CreateRoomS2C synPack = new CreateRoomS2C();
        //    synPack = (CreateRoomS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);

        //    if (synPack.Succeed)
        //    {
        //        EventDispatcher.Instance().DispatchEvent("CreateGame", true);
        //    }
        //    else
        //    {
        //        EventDispatcher.Instance().DispatchEvent("CreateGame", false);
        //    }
        //}
        //else if (tag == GeneralType.GetRoomList + 100)
        //{
        //    IMessage IMPlayersPack = new GetRoomListS2C();
        //    GetRoomListS2C synPack = new GetRoomListS2C();
        //    synPack = (GetRoomListS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);


        //    List<RoomModel> ret_list = new List<RoomModel>();
        //    for (int i = 0; i < synPack.Room.Count; i++)
        //    {
        //        RoomModel modelData = new RoomModel();
        //        modelData.Currentsize = synPack.Room[i].Currentsize;
        //        modelData.Maxsize = synPack.Room[i].Maxsize;
        //        modelData.roomID = synPack.Room[i].Roomid;
        //        ret_list.Add(modelData);

        //    }
        //    EventDispatcher.Instance().DispatchEvent("GetRoomList", ret_list);
        //}
        //else if (tag == GeneralType.EnterRoom + 100)
        //{

        //    IMessage IMPlayersPack = new EnterRoomS2C();
        //    EnterRoomS2C synPack = new EnterRoomS2C();
        //    synPack = (EnterRoomS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
        //    //to do


        //    EventDispatcher.Instance().DispatchEvent("EnterRoom", synPack);

        //    EventDispatcher.Instance().DispatchEvent("Enter", 0);
        //}
        //else if (tag == GeneralType.PlayerReady + 100)
        //{
        //    IMessage IMPlayersPack = new PlayerReadyS2C();
        //    PlayerReadyS2C synPack = new PlayerReadyS2C();
        //    synPack = (PlayerReadyS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
        //    EventDispatcher.Instance().DispatchEvent("PlayerReady", synPack);
        //}
        //else if (tag == GeneralType.RoomOwnerStart + 100)
        //{
        //    IMessage IMPlayersPack = new RoomOwnerStartS2C();
        //    RoomOwnerStartS2C synPack = new RoomOwnerStartS2C();
        //    synPack = (RoomOwnerStartS2C)IMPlayersPack.Descriptor.Parser.ParseFrom(pack, 8, pack_size);
        //    EventDispatcher.Instance().DispatchEvent("RoomStart", synPack);

        //}

    }


}
