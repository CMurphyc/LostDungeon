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
        string username = main.GetComponent<GameMain>().WorldSystem._model.PlayerModel.username ;
        //send request
        main.GetComponent<GameMain>().socket.sock_c2s.CreateRoomC2S(username,4);
        print("Create Room Request Send");

        //if (main != null)
        //{
        //    MapManager mapCtl = main.GetComponent<GameMain>().WorldSystem._map;
        //    mapCtl.SwitchScene("TeamUpUI");
        //   // print(SceneManager.)
        //}

    }

    public void OnBtnGetRoomList()
    {
        //send request
        main.GetComponent<GameMain>().socket.sock_c2s.GetRoomListC2S();
        print("GetRoomList Request Send");

        //if (main != null)
        //{
        //    MapManager mapCtl = main.GetComponent<GameMain>().WorldSystem._map;
        //    mapCtl.SwitchScene("RoomList");
        //    // print(SceneManager.)
        //}

    }

}
