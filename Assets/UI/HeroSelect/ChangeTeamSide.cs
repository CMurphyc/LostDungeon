using UnityEngine;
using System.Collections;

public class ChangeTeamSide : MonoBehaviour
{
    GameObject main;
    // Use this for initialization
    void Start()
    {
        main = GameObject.FindWithTag("GameEntry");
    }

    public void onBtnRedTeam()
    {
        main.GetComponent<GameMain>().socket.sock_c2s.ChangeFaction(1);
    }
    public void onBtnBlueTeam()
    {
        main.GetComponent<GameMain>().socket.sock_c2s.ChangeFaction(0);
    }
}
