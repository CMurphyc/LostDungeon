
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

        Counter++;

        if (Counter > 2 && main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.NeedUpdate)
        {
            RefreshWindow();

        }

    }
    void RefreshWindow()
    {
        main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.NeedUpdate = false;
        GetRoomListS2C temp = main.GetComponent<GameMain>().WorldSystem._model._RoomListModule.RoomListPack;
        for (int i = 0; i < temp.RoomsInfo.Count; i++)
        {
            RoomInfo item = temp.RoomsInfo[i];
            string size_Str = item.Currentsize.ToString() + "/" + item.Maxsize.ToString();
            AddItem(item.Roomid.ToString(), size_Str);
        }
    }

    ////添加列表项
    public void AddItem(string roomid, string size)
    {
        GameObject a = Instantiate(item) as GameObject;

        a.transform.Find("RoomID").GetComponent<Text>().text = roomid;
        a.transform.Find("size").GetComponent<Text>().text = size;
        a.transform.Find("Button").GetComponent<Button>().onClick.AddListener(
            delegate ()
            {
                EnterRoom(a);
            }
        );
        a.GetComponent<Transform>().SetParent(parent.GetComponent<Transform>(), false);
        //a.transform.localPosition = new Vector3(itemLocalPos.x, itemLocalPos.y - messages.Count * itemHeight, 0);

        a.GetComponent<RectTransform>().anchoredPosition =new Vector2(9.152496f, -42.1f- messages.Count* itemHeight);
        
        messages.Add(a);
        if (contentSize.y <= messages.Count * itemHeight)//增加内容的高度
        {
            
            //parent.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, messages.Count * itemHeight);
        }
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
        main.GetComponent<GameMain>().socket.sock_c2s.CreateRoomC2S();
        print("Create Room Request Send");

        //if (main != null)
        //{
        //    MapManager mapCtl = main.GetComponent<GameMain>().WorldSystem._map;
        //    mapCtl.SwitchScene("TeamUpUI");
        //   // print(SceneManager.)
        //}

    }

    ////移除列表项
    //public void RemoveItem(GameObject t)
    //{
    //    int index = messages.IndexOf(t);
    //    messages.Remove(t);
    //    Destroy(t);

    //    for (int i = index; i < messages.Count; i++)//移除的列表项后的每一项都向前移动
    //    {
    //        messages[i].transform.localPosition += new Vector3(0, itemHeight, 0);
    //    }

    //    if (contentSize.y <= messages.Count * itemHeight)//调整内容的高度
    //        parent.GetComponent<RectTransform>().sizeDelta = new Vector2(contentSize.x, messages.Count * itemHeight);
    //    else
    //        parent.GetComponent<RectTransform>().sizeDelta = contentSize;
    //}
    //public void CancleOnClick()
    //{
    //    RemoveItem(this.gameObject);
    //}
}
