using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public SystemManager WorldSystem;

    public Client socket;

    // Start is called before the first frame update
    void Start()
    {
        //初始化系统
        UnityEngine.Object.DontDestroyOnLoad(gameObject);
        WorldSystem = new SystemManager();
        socket = new Client();
        //服务器连接
        socket.SetIP("116.62.11.228");
        socket.SetPort(10000);
        socket.Connect();
        socket.CreateListenThread();
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
