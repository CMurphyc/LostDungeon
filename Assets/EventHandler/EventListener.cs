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
        EventDispatcher.Instance().RegistEventListener(EventMessageType.StartSync, StartSync);
        EventDispatcher.Instance().RegistEventListener(EventMessageType.NextFloor, NextFloor);
        EventDispatcher.Instance().RegistEventListener(EventMessageType.GameOver, GameOver);
    }

    void BattleSyn(EventBase eb)
    {
        BattleFrame synPack = (BattleFrame)eb.eventValue;
        if (synPack.Error == 0)
        {
            List<BattleInput> Inputs = new List<BattleInput>();
            for (int i = 0; i < synPack.BattleInputs.Count; i++)
            {
                Inputs.Add(synPack.BattleInputs[i]);
            }
            AttackType temp = main.GetComponent<GameMain>().WorldSystem._model._JoyStickModule.type;

            switch (main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.roomType)
            {
                case RoomType.Pve:
                    //Debug.Log("SynPack");
                    // 缓存帧信息
                    main.GetComponent<GameMain>().WorldSystem._battle.SeverFrame = synPack.FrameNumber;
                    main.GetComponent<GameMain>().WorldSystem._battle.Seed = synPack.RandomCode;

                    //Debug.Log("Sever Frame: "+ synPack.FrameNumber);
                    //Debug.Log("Seed: " + synPack.RandomCode);

                    main.GetComponent<GameMain>().WorldSystem._battle._player.frameInfo = Inputs;
                    main.GetComponent<GameMain>().WorldSystem._battle.UpdateFrame();

                    main.GetComponent<GameMain>().socket.sock_c2s.BattleSynC2S(main.GetComponent<GameMain>().WorldSystem._model._JoyStickModule.Ljoystick, main.GetComponent<GameMain>().WorldSystem._model._JoyStickModule.Rjoystick, main.GetComponent<GameMain>().WorldSystem._model._JoyStickModule.type);

                    
                    if (temp == AttackType.Skill1 || temp == AttackType.Skill2 || temp == AttackType.Pick || temp == AttackType.Skill3)
                    {
                        //Debug.Log("Reset JoyStick");
                        main.GetComponent<GameMain>().WorldSystem._model._JoyStickModule.Rjoystick = Vector3.zero;
                        main.GetComponent<GameMain>().WorldSystem._model._JoyStickModule.type = AttackType.BasicAttack;
                    }
                    break;
                case RoomType.Pvp:
                    //Debug.Log("SynPack");
                    // 缓存帧信息
                    main.GetComponent<GameMain>().WorldSystem._pvpbattle.SeverFrame = synPack.FrameNumber;
                    main.GetComponent<GameMain>().WorldSystem._pvpbattle.Seed = synPack.RandomCode;

                    //Debug.Log("Sever Frame: "+ synPack.FrameNumber);
                    //Debug.Log("Seed: " + synPack.RandomCode);
                    main.GetComponent<GameMain>().WorldSystem._pvpbattle._pvpplayer.frameInfo = Inputs;
                    main.GetComponent<GameMain>().WorldSystem._pvpbattle.UpdateFrame();

                    main.GetComponent<GameMain>().socket.sock_c2s.BattleSynC2S(main.GetComponent<GameMain>().WorldSystem._model._JoyStickModule.Ljoystick, main.GetComponent<GameMain>().WorldSystem._model._JoyStickModule.Rjoystick, main.GetComponent<GameMain>().WorldSystem._model._JoyStickModule.type);
                    if (temp == AttackType.Skill1 || temp == AttackType.Skill2 || temp == AttackType.Pick || temp == AttackType.Skill3)
                    {
                        //Debug.Log("Reset JoyStick");
                        main.GetComponent<GameMain>().WorldSystem._model._JoyStickModule.Rjoystick = Vector3.zero;
                        main.GetComponent<GameMain>().WorldSystem._model._JoyStickModule.type = AttackType.BasicAttack;
                    }
                    break;
            } 
            

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
                main.GetComponent<GameMain>().WorldSystem._model._RoomModule.MaxMapFloorNumber = synPack.MaxFloorNumber;
                main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("LoadingPanel");
            }
            else
            {
                main.GetComponent<GameMain>().WorldSystem._message.PopText("Start Failed,All Players Need to Be Ready");
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
            RoomType roomType = main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.roomType;
            main.GetComponent<GameMain>().socket.sock_c2s.GetRoomList(roomType);
        }
        else
        {
            main.GetComponent<GameMain>().WorldSystem._message.PopText("Leave Room Request Failed");
        }

    }

    void GetRoomList(EventBase eb)
    {
        GetRoomListS2C synPack = (GetRoomListS2C)eb.eventValue;
        // Debug.Log("Error: "+synPack.Error);
        if (synPack.Error==0)
        {
            main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.RoomListPack = synPack;
            main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.NeedUpdate = true;
            main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("RoomList");
        }
        else
        {
            main.GetComponent<GameMain>().WorldSystem._message.PopText("Get RoomList Faield");
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
            Debug.LogError(synPack.PlayersInfo.Count);
            for (int i = 0; i < synPack.PlayersInfo.Count; i++)
            {
                switch (main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.roomType)
                {
                    case RoomType.Pve:
                        main.GetComponent<GameMain>().WorldSystem._model._RoomModule.Add_Player(synPack.PlayersInfo[i]);
                        break;
                    case RoomType.Pvp:
                        main.GetComponent<GameMain>().WorldSystem._model._RoomModule.Add_Player(synPack.PlayersInfo[i]);
                        break;
                }
            }

            main.GetComponent<GameMain>().WorldSystem._model._RoomModule.NeedUpdate = true;
            switch (main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.roomType)
            {
                case RoomType.Pve:
                    main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("HeroSelect");
                    break;
                case RoomType.Pvp:
                    main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("HeroSelect2");
                    break;
            }
        }
        else
        {
            main.GetComponent<GameMain>().WorldSystem._message.PopText("Enter Room Failed");
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
                main.GetComponent<GameMain>().WorldSystem._message.PopText("Exceed Max Room Number");
            }
        }
        else
        {
            main.GetComponent<GameMain>().WorldSystem._message.PopText("Sever Error");
        }

    }

    void StartSync(EventBase eb)
    {
        StartSyncS2C synPack = (StartSyncS2C)eb.eventValue;
        if (synPack.Error == 0)
        {
            if (synPack.Succeed)
            {
                Debug.Log("开始帧同步");
                main.GetComponent<GameMain>().WorldSystem._model._RoomModule.isLoadingCompleted = true;
                main.GetComponent<GameMain>().WorldSystem._map.CurrentScene = "MapCreate";
            }
            else
            {
                Debug.Log("开始帧同步失败");
                main.GetComponent<GameMain>().WorldSystem._message.PopText("Start Game Failed");
            }
        }
        else
        {
            main.GetComponent<GameMain>().WorldSystem._message.PopText("Sever Error");
        }
    }

    void NextFloor(EventBase eb)
    {
        NextFloorS2C synPack = (NextFloorS2C)eb.eventValue;
        if (synPack.Error == 0)
        {
            if (synPack.Succeed)
            {
                Debug.Log("准备进入第" + synPack.FloorNumber + "层");
                main.GetComponent<GameMain>().WorldSystem._model._RoomModule.MapFloorNumber = synPack.FloorNumber;
                main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("LoadingPanel");
            }
            else
            {
                Debug.Log("进入下一层失败");
                main.GetComponent<GameMain>().WorldSystem._message.PopText("Enter Next Floor Failed");
            }
        }
        else
        {
            main.GetComponent<GameMain>().WorldSystem._message.PopText("Sever Error");
        }
    }

    void GameOver(EventBase eb)
    {
        GameOverS2C synPack = (GameOverS2C)eb.eventValue;
        if (synPack.Error == 0)
        {
            if (synPack.Succeed)
            {
                Debug.Log("游戏结束了");
                main.GetComponent<GameMain>().WorldSystem._model._RoomModule.isOver = true;
                switch (main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.roomType)
                {
                    case RoomType.Pve:
                        main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("Overview");
                        break;
                    case RoomType.Pvp:
                        main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("Main2");
                        break;
                }
                

                //清除背包数据
                main.GetComponent<GameMain>().WorldSystem._model._BagModule.Free();
            }
            else
            {
                Debug.Log("结束游戏失败");
                main.GetComponent<GameMain>().WorldSystem._message.PopText("GameOver Failed");
            }
        }
        else
        {
            main.GetComponent<GameMain>().WorldSystem._message.PopText("Sever Error");
        }
    }

}