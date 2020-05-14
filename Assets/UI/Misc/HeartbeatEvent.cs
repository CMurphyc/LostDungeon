using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartbeatEvent : MonoBehaviour
{
    GameObject main;
    GameObject canvas;
    Text pingText;
    GameObject pingPrefab;
    float heartbeatTimer = 0f;
    float pingUITimer = 0f;
    float sendTime = 0f;
    void Start()
    {
        main = GameObject.Find("GameEntry");
        pingPrefab = Resources.Load<GameObject>("UI/Ping");
    }

    void Update()
    {
        Heartbeat();
        UpdatePingUI();
    }

    void UpdatePingUI()
    {
        if (main.GetComponent<GameMain>().WorldSystem._model._MiscModule.NeedUpdatePing() || pingUITimer >= 1f)
        {
            canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                if (canvas.transform.Find("Ping") != null)
                {
                    pingText = canvas.transform.Find("Ping").gameObject.GetComponent<Text>();
                }
                else
                {
                    pingText = GameObject.Instantiate(pingPrefab, canvas.transform).GetComponent<Text>();
                    pingText.gameObject.name = "Ping";
                    pingText.gameObject.transform.SetSiblingIndex(pingText.gameObject.transform.parent.childCount - 1);
                }
                int ping = (int)((Time.time - sendTime) * 1000);
                if (0 <= ping && ping < 100)
                {
                    pingText.color = Color.green;
                    pingText.text = (ping).ToString() + "ms";
                }
                else if (100 <= ping && ping < 200)
                {
                    pingText.color = Color.yellow;
                    pingText.text = (ping).ToString() + "ms";
                }
                else if(200 <= ping && ping < 10000)
                {
                    pingText.color = Color.red;
                    pingText.text = (ping).ToString() + "ms";
                }
                else if (ping >= 10000)
                {
                    pingText.color = Color.white;
                    pingText.text = "offline";
                }
            }
            pingUITimer = 0f;
        }
        pingUITimer += Time.deltaTime;
    }

    void Heartbeat()
    {
        if (heartbeatTimer >= 1f)
        {
            if (main.GetComponent<GameMain>().WorldSystem._model._MiscModule.NeedHeartbeat())
            {
                main.GetComponent<GameMain>().socket.sock_c2s.Heartbeat();
                sendTime = Time.time;
                heartbeatTimer = 0;
            }
        }
        heartbeatTimer += Time.deltaTime;
    }
}
