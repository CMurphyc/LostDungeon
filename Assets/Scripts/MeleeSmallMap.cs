using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeSmallMap : MonoBehaviour
{
    public Image BackGround;
    public Image None;
    public Image R;
    public Image L;
    public Image NowNone;
    public Image NowR;
    public Image NowL;
    public Image BossTag;
    public Image ChallengeTag;
    public Image DevilTag;
    public Image GameTag;
    public Image SacrificeTag;
    public Image ShopTag;
    public Image TreasureTag;
    private GameObject canvas;
    private Image background;
    private float roomSize;
    private float dOffset;
    private float hOffset;

    public Dictionary<int, List<Image>> roomToSmallMap = new Dictionary<int, List<Image>>();    // 房间对应的小地图底色
    public Dictionary<int, List<Image>> nowroomToSmallMap = new Dictionary<int, List<Image>>();    // 房间对应的小地图亮色
    public Dictionary<int, Image> roomToTag = new Dictionary<int, Image>();    // 房间对应的小地图图标
    
    
    void Start()
    {
        canvas = GameObject.Find("Canvas").gameObject;
        background = Instantiate(BackGround);
        background.transform.SetParent(canvas.transform);
        background.GetComponent<RectTransform>().anchoredPosition = canvas.GetComponent<RectTransform>().position + new Vector3(canvas.GetComponent<RectTransform>().rect.width / 2, canvas.GetComponent<RectTransform>().rect.height / 2);
        // test();

        int d = 5;
        int h = 5;

        roomSize = 110f / 5;
        // Debug.Log("roomSize = " + roomSize);
        dOffset = (120 - d * roomSize) / 2;
        hOffset = (120 - h * roomSize) / 2;

        int[,] array = new int[,]{
            { 12, 9, 0, 11, 9 },
            { 1, 3, 3, 1, 1 },
            { 11, 0, 9, 0, 11 },
            { 1, 1, 3, 3, 1 },
            { 9, 11, 0, 9, 13 }
        };
        // Debug.Log("d = " + d + ", h = " + h);
        CreateSmallMap(array, h, d);
    }


    public void CreateSmallMap(int[,] map, int row, int col)
    {
        background.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 120);
        background.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 120);
        int[,] roomTag = new int[row, col];
        int nowRoom = 0;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (map[i, j] == 0) continue;
                // 如果当前位置已经属于某一个房间  则不生成房间
                if (roomTag[i, j] != 0) continue;
                // Debug.Log(i + "   " + j);
                nowRoom++;
                List<Image> rooms = new List<Image>();
                List<Image> nowrooms = new List<Image>();
                Image image_tag = null;
                if (map[i, j] == 3)  // 长直道
                {
                    Image image_1 = Instantiate(R);
                    image_1.transform.SetParent(canvas.transform);
                    ChangePosition(image_1, row, col, i, j);
                    Image image_2 = Instantiate(L);
                    image_2.transform.SetParent(canvas.transform);
                    ChangePosition(image_2, row, col, i, j + 1);

                    rooms.Add(image_1);
                    rooms.Add(image_2);

                    Image image_5 = Instantiate(NowR);
                    image_5.transform.SetParent(canvas.transform);
                    ChangePosition(image_5, row, col, i, j);
                    Image image_6 = Instantiate(NowL);
                    image_6.transform.SetParent(canvas.transform);
                    ChangePosition(image_6, row, col, i, j + 1);

                    image_5.gameObject.SetActive(false);
                    image_6.gameObject.SetActive(false);

                    nowrooms.Add(image_5);
                    nowrooms.Add(image_6);

                    roomTag[i, j] = nowRoom; roomTag[i, j + 1] = nowRoom;
                }
                else  // 普通大小
                {
                    Image image_1 = Instantiate(None);
                    image_1.transform.SetParent(canvas.transform);
                    ChangePosition(image_1, row, col, i, j);

                    rooms.Add(image_1);

                    Image image_2 = Instantiate(NowNone);
                    image_2.transform.SetParent(canvas.transform);
                    ChangePosition(image_2, row, col, i, j);

                    image_2.gameObject.SetActive(false);

                    nowrooms.Add(image_2);

                    roomTag[i, j] = nowRoom;
                    if (map[i, j] == 9)  // 游戏
                    {
                        image_tag = Instantiate(GameTag);
                        image_tag.transform.SetParent(canvas.transform);
                        ChangePosition(image_tag, row, col, i, j);
                    }
                    else if (map[i, j] == 11)  // 宝箱
                    {
                        image_tag = Instantiate(TreasureTag);
                        image_tag.transform.SetParent(canvas.transform);
                        ChangePosition(image_tag, row, col, i, j);
                    }

                }
                roomToSmallMap.Add(nowRoom, rooms);
                nowroomToSmallMap.Add(nowRoom, nowrooms);
                roomToTag.Add(nowRoom, image_tag);
            }

        }

    }

    void ChangePosition(Image image, int row, int col, int i, int j)
    {
        image.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        image.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, dOffset + roomSize * (col - j - 1f), roomSize);
        image.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, hOffset + roomSize * (i + 1 - 1f), roomSize);
    }

    public void ChangeRoom(List<int> RoomIDList)
    {
        foreach(var i in nowroomToSmallMap)
        {
            //Debug.LogError("AllRoom:"+i);
            foreach(var j in i.Value)
            {
                j.gameObject.SetActive(false);
            }
        }
        foreach(var i in RoomIDList)
        {
            //Debug.LogError("PlayerRoom:"+i);
            List<Image> newNowRooms = nowroomToSmallMap[i];
            foreach (var item in newNowRooms)
            {
                item.gameObject.SetActive(true);
            }
            List<Image> newRooms = roomToSmallMap[i];
            foreach (var item in newRooms)
            {
                item.gameObject.SetActive(true);
            }
            Image roomTag = roomToTag[i];
            if (roomTag != null) roomTag.gameObject.SetActive(true);
        }
    }
    public void ChangeRoom(int oldRoom, int newRoom)
    {
        List<Image> oldNowRooms = nowroomToSmallMap[oldRoom];
        foreach (var item in oldNowRooms)
        {
            item.gameObject.SetActive(false);
        }

        List<Image> newNowRooms = nowroomToSmallMap[newRoom];
        foreach (var item in newNowRooms)
        {
            item.gameObject.SetActive(true);
        }

        List<Image> newRooms = roomToSmallMap[newRoom];
        foreach (var item in newRooms)
        {
            item.gameObject.SetActive(true);
        }

        Image roomTag = roomToTag[newRoom];
        if (roomTag != null) roomTag.gameObject.SetActive(true);
    }
}
