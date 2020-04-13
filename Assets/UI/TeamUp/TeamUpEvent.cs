
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TeamUpEvent : MonoBehaviour
{
    List<GameObject> messages = new List<GameObject>();
    public GameObject item;
    GameObject myMessage;
    GameObject parent;
    Vector3 itemLocalPos;
    Vector2 contentSize;
    float itemHeight;
    GameObject main;
    float counter;
    List<GameObject> listItem = new List<GameObject>();

    string RoomOwner;
    private void Update()
    {
        counter += Time.deltaTime;
        if (counter >= 5)
        {
            ReleaseObjects();
            RefreshList();
            counter = 0;
        }
    }
    void ReleaseObjects()
    {
     
        for (int i = 0; i < listItem.Count; i++)
        {
            RemoveItem(listItem[i]);
          
        }
        listItem.Clear();





    }
    void Start()
    {
        main = GameObject.FindWithTag("GameEntry");
        parent = GameObject.Find("Canvas/content");
        contentSize = parent.GetComponent<RectTransform>().sizeDelta;

        itemHeight = item.GetComponent<RectTransform>().rect.height;
        itemLocalPos = item.transform.localPosition;
       
        RefreshList();
    }
   
    void RefreshList()
    {
        //EnterRoomS2C synPack = main.GetComponent<GameMain>().WorldSystem._model.RoomInfoModel.RoomInfo;
        //print("刷新玩家状态: " + synPack.Player.Count);
        //for (int i = 0; i < synPack.Player.Count; i++)
        //{

        //    string username = synPack.Player[i].Playerid;
        //    if (i==0)
        //    {
        //        RoomOwner = username;
        //        print("房主是： " + RoomOwner);
        //    }
        //    int state = synPack.Player[i].Status;
        //    GameObject.Find("GameObject").GetComponent<TeamUpEvent>().AddItem(username, (StateType)state);
        //}
    }
    //添加列表项
    public void AddItem(string username, StateType state)
    {
        //GameObject a = Instantiate(item) as GameObject;
        //listItem.Add(a);
        //a.transform.Find("username").GetComponent<Text>().text = username;
        //if (username == main.GetComponent<GameMain>().WorldSystem._model.PlayerModel.username)
        //{
        //    a.transform.Find("username").GetComponent<Text>().color = Color.red;
        //}
        //if (state == StateType.Not_Ready)
        //{
        //    a.transform.Find("State").GetComponent<Text>().text = "Not Ready" ;
        //}
        //else if (state == StateType.Ready)
        //{
        //    a.transform.Find("State").GetComponent<Text>().text = "Ready";
        //}
        //if (username != RoomOwner)
        //{
        //    if (state == StateType.Not_Ready)
        //    {
             
        //        a.transform.Find("Button/Text").GetComponent<Text>().text = "准备";
        //    }
        //    else if (state == StateType.Ready)
        //    {
              
        //        a.transform.Find("Button/Text").GetComponent<Text>().text = "取消准备";
        //    }
        //    a.transform.Find("Button").GetComponent<Button>().onClick.AddListener(
        //        delegate ()
        //        {
        //            OnBtnReady(a);
        //        }
        //    );
        //}
        //else
        //{
        //    a.transform.Find("Button/Text").GetComponent<Text>().text = "开始游戏";
        //    a.transform.Find("Button").GetComponent<Button>().onClick.AddListener(
        //       delegate ()
        //       {
        //           OnBtnStart(a);
        //       }
        //   );

        //}
        //a.GetComponent<Transform>().SetParent(parent.GetComponent<Transform>(), false);
        //a.transform.localPosition = new Vector3(itemLocalPos.x, itemLocalPos.y - messages.Count * itemHeight, 0);
        //messages.Add(a);

        //if (contentSize.y <= messages.Count * itemHeight)//增加内容的高度
        //{
        //    parent.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, messages.Count * itemHeight);
        //}
    }
    //public void OnBtnReady(GameObject t)
    //{
    //    int status;
    //    if (t.transform.Find("State").GetComponent<Text>().text == "Not Ready")
    //    {
    //        status = (int)StateType.Ready;
    //    }
    //    else
    //    {
    //        status = (int)StateType.Not_Ready;
    //    }
    //    string username = t.transform.Find("username").GetComponent<Text>().text;

    //    main.GetComponent<GameMain>().socket.sock_c2s.PlayerReadyC2S(username, status);
    //}
    //public void OnBtnStart(GameObject t)
    //{
    //    string username = main.GetComponent<GameMain>().WorldSystem._model.PlayerModel.username;
    //    print("Start Game : " + username);
    //    main.GetComponent<GameMain>().socket.sock_c2s.RoomOwnerStartC2S(username);
    //}


    //移除列表项
    public void RemoveItem(GameObject t)
    {
        int index = messages.IndexOf(t);
        messages.Remove(t);
        Destroy(t);

        for (int i = index; i < messages.Count; i++)//移除的列表项后的每一项都向前移动
        {
            messages[i].transform.localPosition += new Vector3(0, itemHeight, 0);
        }

        if (contentSize.y <= messages.Count * itemHeight)//调整内容的高度
            parent.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, messages.Count * itemHeight);
        else
            parent.GetComponent<RectTransform>().sizeDelta = contentSize;
    }
    public void CancleOnClick()
    {
        RemoveItem(this.gameObject);
    }
}
