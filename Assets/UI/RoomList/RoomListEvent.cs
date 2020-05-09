
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RoomListEvent : MonoBehaviour
{
    List<GameObject> messages = new List<GameObject>();
    public GameObject item;
    GameObject myMessage;
    GameObject parent;
    Vector3 itemLocalPos;
    Vector2 contentSize;
    float itemHeight;
    GameObject main;
    float timer = 0;
    int Counter = 0;

    private void Awake()
    {
        main = GameObject.FindWithTag("GameEntry");
    }

    void Start()
    {
     
        parent = GameObject.Find("Canvas/content/Viewport/Content");
        contentSize = parent.GetComponent<RectTransform>().sizeDelta;
        itemHeight = item.GetComponent<RectTransform>().rect.height;
        itemLocalPos = item.transform.localPosition;

     
    }

    void Update()
    {
        RefreshRoomList();
        Counter++;
        if (Counter > 2 && main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.NeedUpdate)
        {
            RefreshWindow();
        }
    }

    void RefreshRoomList()
    {
        if (timer >= 5f)
        {
            RoomType roomType = main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.roomType;
            main.GetComponent<GameMain>().socket.sock_c2s.GetRoomList(roomType);
            timer = 0;
        }
        timer += Time.deltaTime;
    }

    // void RefreshWindow()
    // {
    //     main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.NeedUpdate = false;
    //     GetRoomListS2C temp = main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.RoomListPack;
    //     for (int i = 0; i < temp.RoomsInfo.Count; i++)
    //     {
    //         RoomInfo item = temp.RoomsInfo[i];
    //         string size_Str = item.Currentsize.ToString() + "/" + item.Maxsize.ToString();
    //         AddItem(item.Roomid.ToString(), item.Owner, size_Str);
    //     }
    // }

    void RefreshWindow()
    {
        main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.NeedUpdate = false;
        GetRoomListS2C temp = main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.RoomListPack;
        int i = 0, j = 0;
        while (i < temp.RoomsInfo.Count && j < messages.Count)
        {
            // Debug.Log("now is only edit data");
            RoomInfo item = temp.RoomsInfo[i];
            GameObject tmp = messages[j];
            tmp.transform.Find("RoomID").GetComponent<Text>().text = item.Roomid.ToString();
            tmp.transform.Find("Owner").GetComponent<Text>().text = item.Owner;
            string size_Str = item.Currentsize.ToString() + "/" + item.Maxsize.ToString();
            tmp.transform.Find("size").GetComponent<Text>().text = size_Str;
            tmp.transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
            tmp.transform.Find("Button").GetComponent<Button>().onClick.AddListener(
                delegate ()
                {
                    EnterRoom(tmp);
                }
            );
            ++i;
            ++j;
        }
        if (i < temp.RoomsInfo.Count)
        {
            // Debug.Log("now is add item");
            for (; i < temp.RoomsInfo.Count; ++i)
            {
                RoomInfo item = temp.RoomsInfo[i];
                string size_Str = item.Currentsize.ToString() + "/" + item.Maxsize.ToString();
                AddItem(item.Roomid.ToString(), item.Owner, size_Str);
            }
        }
        else if(j < messages.Count)
        {
            // Debug.Log("now is remove item");
            int tmp = j;
            for (j = messages.Count - 1; j >= tmp; --j)
            {
                Destroy(messages[j]);
                messages.RemoveAt(j);
            }
        }
    }

    //添加列表项
    public void AddItem(string roomid, string owner, string size)
    {
        GameObject a = Instantiate(item) as GameObject;

        a.transform.Find("RoomID").GetComponent<Text>().text = roomid;
        a.transform.Find("Owner").GetComponent<Text>().text = owner;
        a.transform.Find("size").GetComponent<Text>().text = size;
        a.transform.Find("Button").GetComponent<Button>().onClick.AddListener(
            delegate ()
            {
                EnterRoom(a);
            }
        );
        a.GetComponent<Transform>().SetParent(parent.GetComponent<Transform>(), false);
        //a.transform.localPosition = new Vector3(itemLocalPos.x, itemLocalPos.y - messages.Count * itemHeight, 0);

        a.GetComponent<RectTransform>().anchoredPosition = new Vector2(9.152496f, -42.1f- messages.Count* itemHeight);
        
        messages.Add(a);
        if (contentSize.y <= messages.Count * itemHeight)//增加内容的高度
        {
            
            //parent.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, messages.Count * itemHeight);
        }
    }

    public void RemoveAllItem()
    {
        itemHeight = item.GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < messages.Count; ++i)
        {
            Destroy(messages[i]);
        }
        messages.Clear();
    }

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

    public void EnterRoom(GameObject t)
    {
        print("Enter Room Request");
        //string username = main.GetComponent<GameMain>().WorldSystem._model.PlayerModel.username;
        ////send request
        int roomid = int.Parse(t.transform.Find("RoomID").GetComponent<Text>().text);
        main.GetComponent<GameMain>().socket.sock_c2s.PlayerEnterRoom(roomid);
    }

    public void OnBtnCreateGame()
    {

        //send request
        RoomType roomType = main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.roomType;
        main.GetComponent<GameMain>().socket.sock_c2s.CreateRoomC2S(roomType);

        //if (main != null)
        //{
        //    MapManager mapCtl = main.GetComponent<GameMain>().WorldSystem._map;
        //    mapCtl.SwitchScene("TeamUpUI");
        //   // print(SceneManager.)
        //}

    }

}
