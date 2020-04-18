using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Google.Protobuf;

using System.Collections.Generic;

public class EventListener : MonoBehaviour
{
    GameObject main;
    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.FindWithTag("GameEntry");
        EventDispatcher.Instance().RegistEventListener(EventMessageType.UserLogin, Login);
        EventDispatcher.Instance().RegistEventListener(EventMessageType.CreateGame, CreateRoom);
        EventDispatcher.Instance().RegistEventListener(EventMessageType.GetRoomInfo, GetRoomInfo);
        EventDispatcher.Instance().RegistEventListener(EventMessageType.GetRoomList, GetRoomList);
        EventDispatcher.Instance().RegistEventListener(EventMessageType.LeaveRoom, LeaveRoom);

        EventDispatcher.Instance().RegistEventListener(EventMessageType.StartGame, StartGame);
        EventDispatcher.Instance().RegistEventListener(EventMessageType.BattleSyn, BattleSyn);

    
    }

    void BattleSyn(EventBase eb)
    {
        BattleFrame synPack = (BattleFrame)eb.eventValue;
        if (synPack.Error == 0)
        {
            //Debug.Log("SynPack");
            // 缓存帧信息
            main.GetComponent<GameMain>().WorldSystem._battle.SeverFrame = synPack.FrameNumber;
            main.GetComponent<GameMain>().WorldSystem._battle.Seed = synPack.RandomCode;

            //Debug.Log("Sever Frame: "+ synPack.FrameNumber);
            //Debug.Log("Seed: " + synPack.RandomCode);
            List<BattleInput> Inputs = new List<BattleInput>();
            for (int i = 0; i <synPack.BattleInputs.Count;i++)
            {
                Inputs.Add(synPack.BattleInputs[i]);

                

            }
            main.GetComponent<GameMain>().WorldSystem._battle._player.frameInfo = Inputs;
            main.GetComponent<GameMain>().WorldSystem._battle.UpdateFrame();
        
            main.GetComponent<GameMain>().socket.sock_c2s.BattleSynC2S(main.GetComponent<GameMain>().WorldSystem._model._JoyStickModule.Ljoystick, main.GetComponent<GameMain>().WorldSystem._model._JoyStickModule.Rjoystick);
        }
    }
    void StartGame(EventBase eb)
    {
        StartGameS2C synPack = (StartGameS2C)eb.eventValue;
        if (synPack.Error == 0)
        {
            if (synPack.Succeed)
            {
                main.GetComponent<GameMain>().WorldSystem._model._RoomModule.MapSeed = synPack.Seed;
                main.GetComponent<GameMain>().WorldSystem._model._RoomModule.MapFloorNumber = synPack.FloorNumber;
                main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("MapCreate");
            }
            else
            {
                Debug.Log("开始游戏失败,有玩家未准备");
            }

        }

    }
    void LeaveRoom(EventBase eb)
    {
        LeaveRoomS2C synPack = (LeaveRoomS2C)eb.eventValue;
        if (synPack.Error == 0)
        {
            Debug.Log("返回成功");
            //main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("RoomList");

            main.GetComponent<GameMain>().socket.sock_c2s.GetRoomList();
        }

    }

    void GetRoomList(EventBase eb)
    {
        GetRoomListS2C synPack = (GetRoomListS2C)eb.eventValue;
        Debug.Log("Error: "+synPack.Error);
        if (synPack.Error==0)
        {
            main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.RoomListPack = synPack;
            main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.NeedUpdate = true;
            main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("RoomList");
        }
    }
    void GetRoomInfo(EventBase eb)
    {
        GetRoomInfoS2C synPack = (GetRoomInfoS2C)eb.eventValue; 

        if (synPack.Error==0)
        {

            main.GetComponent<GameMain>().WorldSystem._model._RoomModule.RemoveAllPlayer();


            main.GetComponent<GameMain>().WorldSystem._model._RoomModule.roomid = synPack.RoomId;

            main.GetComponent<GameMain>().WorldSystem._model._RoomModule.roomOwnerID = synPack.RoomOwnerId;
           
            for (int i = 0; i < synPack.PlayersInfo.Count;i++)
            {
                main.GetComponent<GameMain>().WorldSystem._model._RoomModule.Add_Player(synPack.PlayersInfo[i]);
            }

            main.GetComponent<GameMain>().WorldSystem._model._RoomModule.NeedUpdate = true;
            main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("HeroSelect");

          
        }


    }

    void Login(EventBase eb)
    {
        LoginS2C temp = (LoginS2C)eb.eventValue;

     
        if (temp.LoginRet == (int)LoginS2C.Types.LoginRet.LoginSuccess)
        {
            string UserName = GameObject.Find("Canvas/username").GetComponent<InputField>().text;
            main.GetComponent<GameMain>().WorldSystem._model._PlayerModule.uid = temp.Uid;
            main.GetComponent<GameMain>().WorldSystem._model._PlayerModule.nickname = temp.UserName;
            main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("Main");

           
        }



    }

    void CreateRoom(EventBase eb)
    {
        CreateRoomS2C synPack = (CreateRoomS2C)eb.eventValue;

        if (synPack.Error == 0 )
        {
            if (synPack.Succeed)
            {
                Debug.Log("创建房间成功");
            }
            else
            {
                Debug.Log("房间数量超出限制");
            }
        }

    }


    //void RoomStart(EventBase eb)
    //{
    //    RoomOwnerStartS2C synPack = (RoomOwnerStartS2C)eb.eventValue;
    //    if (synPack.Error == 0)
    //    {
    //        print("进入战斗");
    //        main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("Battle");
    //    }
    //    else if (synPack.Error<0)
    //    {
    //        print("有玩家未准备");
    //    }
    //}
    //void PlayerReady(EventBase eb)
    //{
    //    PlayerReadyS2C synPack = (PlayerReadyS2C)eb.eventValue;
    //    EnterRoomS2C temp = main.GetComponent<GameMain>().WorldSystem._model.RoomInfoModel.RoomInfo;

    //    for (int i = 0; i < temp.Player.Count;i++)
    //    {
    //        string username = temp.Player[i].Playerid;
    //        if (username == synPack.Username)
    //        {
    //            temp.Player[i].Status = synPack.Status;
    //        }    
    //    }
    //    main.GetComponent<GameMain>().WorldSystem._model.RoomInfoModel.RoomInfo = temp;

    //    main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("TeamUpUI");

    //}
    //void EnterRoom(EventBase eb)
    //{
    //    EnterRoomS2C synPack = (EnterRoomS2C)eb.eventValue;
    //    main.GetComponent<GameMain>().WorldSystem._model.RoomInfoModel.RoomInfo = synPack;

    //    main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("TeamUpUI");



    //    //for (int i = 0; i < synPack.Player.Count; i++)
    //    //{
    //    //    string username = synPack.Player[i].Playerid;
    //    //    int state = synPack.Player[i].Status;
    //    //    GameObject.Find("GameObject").GetComponent<TeamUpEvent>().AddItem(username, (StateType)state);
    //    //}
    //}
    //void GetRoomList(EventBase eb)
    //{
    //    List<RoomModel> temp = (List<RoomModel>)eb.eventValue;

    //    main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("RoomList");


    //    main.GetComponent<GameMain>().WorldSystem._model.RoomModel.RoomInfo = temp;


    //    //for (int i = 0; i < temp.Count; i++)
    //    //{
    //    //    RoomModel item = temp[i];
    //    //    string size_Str = item.Currentsize.ToString() + "/" + item.Maxsize.ToString();
    //    //    print("Map Index: " + main.GetComponent<GameMain>().WorldSystem._map.GetCurrentIndex());
    //    //    GameObject temp2 = GameObject.Find("GameObject");
    //    //    temp2.GetComponent<RoomListEvent>().AddItem(item.roomID.ToString(), size_Str);

    //    //    print("房间号： " + item.roomID);
    //    //    print("当前玩家数量： " + item.Currentsize);
    //    //    print("最大玩家数量： " + item.Maxsize);

    //    //}
    //}

    //void CreateGame(EventBase eb)
    //{
    //    if (bool.Parse(eb.eventValue.ToString()))
    //    {
    //        if (main.GetComponent<GameMain>().WorldSystem._map.GetCurrentIndex() == 1)
    //        {
    //            EnterRoomS2C temp = new EnterRoomS2C() ;
    //            string username = main.GetComponent<GameMain>().WorldSystem._model.PlayerModel.username;
    //            PlayerInfo info = new PlayerInfo();
    //            info.Playerid = username;
    //            info.Status = (int)StateType.Not_Ready;
    //            temp.Player.Add(info);
    //            temp.Roomstatus = 0;
    //            main.GetComponent<GameMain>().WorldSystem._model.RoomInfoModel.RoomInfo = temp;
    //            main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("TeamUpUI");

    //        }

    //        //GameObject.Find("GameObject").GetComponent<TeamUpEvent>().AddItem(username,StateType.Not_Ready);
    //        print("创建房间成功");
    //    }
    //    else
    //    {
    //        print("创建房间失败");
    //    }

    //}
    //void Register(EventBase eb)
    //{
    //    if (bool.Parse(eb.eventValue.ToString()))
    //    {
    //        print("创建成功");
    //    }
    //    else
    //    {
    //        print("该用户名已被占用");
    //    }
    //}



}