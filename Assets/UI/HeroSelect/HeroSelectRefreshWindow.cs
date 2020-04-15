using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSelectRefreshWindow : MonoBehaviour
{
    GameObject main;

    int Counter = 0;
    private void Awake()
    {
        main = GameObject.FindWithTag("GameEntry");

       
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void RefreshWindow()
    {
        main.GetComponent<GameMain>().WorldSystem._model._RoomModule.NeedUpdate = false;
        main.GetComponent<GameMain>().WorldSystem._map.RefreshRoomInfo();
     
    }

    // Update is called once per frame
    void Update()
    {
        Counter++;

        if (Counter > 2 && main.GetComponent<GameMain>().WorldSystem._model._RoomModule.NeedUpdate)
        {
            RefreshWindow();
        }

    }
}
