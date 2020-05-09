using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIEvent : MonoBehaviour
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
        
    }

    public void OnBtnCreateGame()
    {
      
        //send request
        RoomType roomType = main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.roomType;
        main.GetComponent<GameMain>().socket.sock_c2s.CreateRoomC2S(roomType);
        print("Create Room Request Send");

        //if (main != null)
        //{
        //    MapManager mapCtl = main.GetComponent<GameMain>().WorldSystem._map;
        //    mapCtl.SwitchScene("TeamUpUI");
        //   // print(SceneManager.)
        //}

    }

    public void OnBtnSelectPVE()
    {
        //send request
        RoomType roomType = RoomType.Pve;
        main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.roomType = roomType;
        main.GetComponent<GameMain>().socket.sock_c2s.GetRoomList(roomType);
        print("Select PVE and GetRoomList Request Send");
    }

    public void OnBtnSelectPVP()
    {
        //send request
        RoomType roomType = RoomType.Pvp;
        main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.roomType = roomType;
        main.GetComponent<GameMain>().socket.sock_c2s.GetRoomList(roomType);
        print("Select PVP and GetRoomList Request Send");
    }

    public void OnBtnReturn()
    {
        // main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("LoginPanel");
    }
}
