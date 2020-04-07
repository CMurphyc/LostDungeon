using UnityEngine;
using System.Collections;

public class TeamUpUIEvent : MonoBehaviour
{
    GameObject main;
    // Use this for initialization
    void Start()
    {
        main = GameObject.FindWithTag("GameEntry");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBtnBack()
    {
        //main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("RoomList");
        //main.GetComponent<GameMain>().socket.sock_c2s.GetRoomListC2S();
        print("GetRoomList Request Send");
    }
}
