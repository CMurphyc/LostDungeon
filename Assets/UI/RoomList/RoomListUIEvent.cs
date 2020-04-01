using UnityEngine;
using System.Collections;

public class RoomListUIEvent : MonoBehaviour
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
    public void onBtnBack()
    {
        main.GetComponent<GameMain>().WorldSystem._map.SwitchScene("Main");
    }
}
