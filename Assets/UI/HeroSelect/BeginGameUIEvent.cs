using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginGameUIEvent : MonoBehaviour
{
    GameObject main;
    void Awake()
    {
        main = GameObject.FindWithTag("GameEntry");
    }
    public void OnBtnReady()
    {
        main.GetComponent<GameMain>().socket.sock_c2s.PlayerReady();

    }

    public void OnBtnExit()
    {
        main.GetComponent<GameMain>().socket.sock_c2s.PlayerExitRoom();
    }
}
