using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopmentTest : MonoBehaviour
{
    private GameObject main;
    void Start()
    {
        main = GameObject.FindWithTag("GameEntry");
    }

    void Update()
    {
        
    }

    public void Test()
    {
        int curFloor = main.GetComponent<GameMain>().WorldSystem._model._RoomModule.MapFloorNumber;
        if (curFloor < main.GetComponent<GameMain>().WorldSystem._model._RoomModule.MaxMapFloorNumber)
        {
            Debug.Log("开发人员测试：进入下一层");
            main.GetComponent<GameMain>().socket.sock_c2s.NextFloor(curFloor + 1);
        }
        else
        {
            Debug.Log("开发人员测试：结束游戏");
            main.GetComponent<GameMain>().socket.sock_c2s.GameOver();
        }
    }
}
