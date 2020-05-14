using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallMap : MonoBehaviour
{
    public Image BackGround;
    public Image None;
    public Image U;
    public Image D;
    public Image L;
    public Image R;
    public Image LU;
    public Image LD;
    public Image RU;
    public Image RD;
    public Image NowNone;
    public Image NowU;
    public Image NowD;
    public Image NowL;
    public Image NowR;
    public Image NowLU;
    public Image NowLD;
    public Image NowRU;
    public Image NowRD;
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

    SystemManager sys;
    private void Awake()
    {
        sys = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem;
    }

    void Start()
    {
        canvas = GameObject.Find("Canvas").gameObject;
        background = Instantiate(BackGround, canvas.transform);
        //background.transform.SetParent(canvas.transform);
        background.GetComponent<RectTransform>().anchoredPosition = 
        canvas.GetComponent<RectTransform>().position+ new Vector3( canvas.GetComponent<RectTransform>().rect.width/2, canvas.GetComponent<RectTransform>().rect.height / 2);
        // test();

        int playerNum = sys._model._RoomModule.GetPlayerSize();

        int seed = sys._model._RoomModule.MapSeed;
        Random.InitState(seed);
        int floorNum = sys._model._RoomModule.MapFloorNumber;
        RandMap.StartRand(seed, playerNum, floorNum);
        int d = RandMap.GetWidth() + 1;
        int h = RandMap.GetHeight() + 1;

        int mn = (d > h) ? h : d;
        int mx = (d > h) ? d : h;

        roomSize = 110f / mx;
        // Debug.Log("roomSize = " + roomSize);
        dOffset = (120 - d * roomSize) / 2;
        hOffset = (120 - h * roomSize) / 2;

        int[,] array = new int[h, d];
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < d; j++)
            {
                array[i, j] = RandMap.GetValue(i, j);
            }
        }
        // Debug.Log("d = " + d + ", h = " + h);
        CreateSmallMap(array, h, d);
    }

    private void test()
    {
        int[,] array = new int[,]{
            { 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0 },
            { 0,  0,  0,  0,  0, 11,  0,  0,  0,  0,  0,  0 },
            { 0,  0,  0,  0,  0,  1,  0, 11,  0,  0,  0,  0 },
            { 0,  0,  0,  0,  4,  4,  0,  4,  4,  0,  0,  0 },
            { 0, 11,  3,  3,  4,  4,  1,  4,  4,  5,  0,  0 },
            { 0,  0,  0,  0,  0,  3,  0,  0,  0,  6,  0,  0 },
            { 0,  0,  0,  0,  0,  3,  0,  0,  0,  0,  0,  0 },
            { 0,  0,  0,  0,  4,  4,  0,  0,  0,  0,  0,  0 },
            { 0, 11,  1, 12,  4,  4,  4,  4,  1, 11,  0,  0 },
            { 0,  0,  0,  0,  0,  0,  4,  4,  0,  0,  0,  0 },
            { 0,  0,  0,  0,  0,  0,  0, 11,  0,  0,  0,  0 },
            { 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0 }
        };
        CreateSmallMap(array, 12, 12);
    }

    void Update()
    {
        
    }

    public void CreateSmallMap(int[,] map, int row, int col)
    {
        background.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 120);
        background.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 120);
        int[,] roomTag = new int[row, col];
        int nowRoom = 0;
        int birthRoom = 0;
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
                if (map[i, j] == 2)  // L形
                {
                    // test 10330171
                    if (j + 1 < col && map[i, j + 1] == 2 && i + 1 < row && map[i + 1, j + 1] == 2)  // Lshape1
                    {
                        Image image_1 = Instantiate(R);
                        image_1.transform.SetParent(canvas.transform);
                        ChangePosition(image_1, row, col, i, j);
                        Image image_2 = Instantiate(LD);
                        image_2.transform.SetParent(canvas.transform);
                        ChangePosition(image_2, row, col, i, j + 1);
                        Image image_4 = Instantiate(U);
                        image_4.transform.SetParent(canvas.transform);
                        ChangePosition(image_4, row, col, i + 1, j + 1);

                        image_1.gameObject.SetActive(false);
                        image_2.gameObject.SetActive(false);
                        image_4.gameObject.SetActive(false);

                        rooms.Add(image_1);
                        rooms.Add(image_2);
                        rooms.Add(image_4);

                        Image image_5 = Instantiate(NowRD);
                        image_5.transform.SetParent(canvas.transform);
                        ChangePosition(image_5, row, col, i, j);
                        Image image_6 = Instantiate(NowLD);
                        image_6.transform.SetParent(canvas.transform);
                        ChangePosition(image_6, row, col, i, j + 1);
                        Image image_8 = Instantiate(NowLU);
                        image_8.transform.SetParent(canvas.transform);
                        ChangePosition(image_8, row, col, i + 1, j + 1);

                        image_5.gameObject.SetActive(false);
                        image_6.gameObject.SetActive(false);
                        image_8.gameObject.SetActive(false);

                        nowrooms.Add(image_5);
                        nowrooms.Add(image_6);
                        nowrooms.Add(image_8);

                        roomTag[i, j] = nowRoom; roomTag[i, j + 1] = nowRoom; roomTag[i + 1, j + 1] = nowRoom;
                    }
                    else if (j + 1 < col && map[i, j + 1] == 2 && i + 1 < row && map[i + 1, j] == 2)  // Lshape2
                    {
                        Image image_1 = Instantiate(RD);
                        image_1.transform.SetParent(canvas.transform);
                        ChangePosition(image_1, row, col, i, j);
                        Image image_2 = Instantiate(L);
                        image_2.transform.SetParent(canvas.transform);
                        ChangePosition(image_2, row, col, i, j + 1);
                        Image image_3 = Instantiate(U);
                        image_3.transform.SetParent(canvas.transform);
                        ChangePosition(image_3, row, col, i + 1, j);

                        image_1.gameObject.SetActive(false);
                        image_2.gameObject.SetActive(false);
                        image_3.gameObject.SetActive(false);

                        rooms.Add(image_1);
                        rooms.Add(image_2);
                        rooms.Add(image_3);

                        Image image_5 = Instantiate(NowRD);
                        image_5.transform.SetParent(canvas.transform);
                        ChangePosition(image_5, row, col, i, j);
                        Image image_6 = Instantiate(NowLD);
                        image_6.transform.SetParent(canvas.transform);
                        ChangePosition(image_6, row, col, i, j + 1);
                        Image image_7 = Instantiate(NowRU);
                        image_7.transform.SetParent(canvas.transform);
                        ChangePosition(image_7, row, col, i + 1, j);

                        image_5.gameObject.SetActive(false);
                        image_6.gameObject.SetActive(false);
                        image_7.gameObject.SetActive(false);

                        nowrooms.Add(image_5);
                        nowrooms.Add(image_6);
                        nowrooms.Add(image_7);

                        roomTag[i, j] = nowRoom; roomTag[i, j + 1] = nowRoom; roomTag[i + 1, j] = nowRoom;
                    }
                    // test 10330178
                    else if (i + 1 < col && map[i + 1, j] == 2 && j + 1 < row && map[i + 1, j + 1] == 2)  // Lshape3
                    {
                        Image image_1 = Instantiate(D);
                        image_1.transform.SetParent(canvas.transform);
                        ChangePosition(image_1, row, col, i, j);
                        Image image_3 = Instantiate(RU);
                        image_3.transform.SetParent(canvas.transform);
                        ChangePosition(image_3, row, col, i + 1, j);
                        Image image_4 = Instantiate(L);
                        image_4.transform.SetParent(canvas.transform);
                        ChangePosition(image_4, row, col, i + 1, j + 1);

                        image_1.gameObject.SetActive(false);
                        image_3.gameObject.SetActive(false);
                        image_4.gameObject.SetActive(false);

                        rooms.Add(image_1);
                        rooms.Add(image_3);
                        rooms.Add(image_4);

                        Image image_5 = Instantiate(NowRD);
                        image_5.transform.SetParent(canvas.transform);
                        ChangePosition(image_5, row, col, i, j);
                        Image image_7 = Instantiate(NowRU);
                        image_7.transform.SetParent(canvas.transform);
                        ChangePosition(image_7, row, col, i + 1, j);
                        Image image_8 = Instantiate(NowLU);
                        image_8.transform.SetParent(canvas.transform);
                        ChangePosition(image_8, row, col, i + 1, j + 1);

                        image_5.gameObject.SetActive(false);
                        image_7.gameObject.SetActive(false);
                        image_8.gameObject.SetActive(false);

                        nowrooms.Add(image_5);
                        nowrooms.Add(image_7);
                        nowrooms.Add(image_8);

                        roomTag[i, j] = nowRoom; roomTag[i + 1, j] = nowRoom; roomTag[i + 1, j + 1] = nowRoom;
                    }
                    // test 10330178
                    else if (i + 1 < col && map[i + 1, j] == 2 && j - 1 < row && map[i + 1, j - 1] == 2)  // Lshape4
                    {
                        Image image_2 = Instantiate(D);
                        image_2.transform.SetParent(canvas.transform);
                        ChangePosition(image_2, row, col, i, j);
                        Image image_3 = Instantiate(R);
                        image_3.transform.SetParent(canvas.transform);
                        ChangePosition(image_3, row, col, i + 1, j - 1);
                        Image image_4 = Instantiate(LU);
                        image_4.transform.SetParent(canvas.transform);
                        ChangePosition(image_4, row, col, i + 1, j);
                        
                        image_2.gameObject.SetActive(false);
                        image_3.gameObject.SetActive(false);
                        image_4.gameObject.SetActive(false);

                        rooms.Add(image_2);
                        rooms.Add(image_3);
                        rooms.Add(image_4);

                        Image image_6 = Instantiate(NowLD);
                        image_6.transform.SetParent(canvas.transform);
                        ChangePosition(image_6, row, col, i, j);
                        Image image_7 = Instantiate(NowRU);
                        image_7.transform.SetParent(canvas.transform);
                        ChangePosition(image_7, row, col, i + 1, j - 1);
                        Image image_8 = Instantiate(NowLU);
                        image_8.transform.SetParent(canvas.transform);
                        ChangePosition(image_8, row, col, i + 1, j);

                        image_6.gameObject.SetActive(false);
                        image_7.gameObject.SetActive(false);
                        image_8.gameObject.SetActive(false);
                        
                        nowrooms.Add(image_6);
                        nowrooms.Add(image_7);
                        nowrooms.Add(image_8);

                        roomTag[i, j] = nowRoom; roomTag[i + 1, j] = nowRoom; roomTag[i + 1, j - 1] = nowRoom;
                    }
                }
                else if (map[i, j] == 3)  // 长直道
                {
                    // test 10330172
                    if (j + 1 < col && map[i, j + 1] == 3)  // LongRoad1
                    {
                        Image image_1 = Instantiate(R);
                        image_1.transform.SetParent(canvas.transform);
                        ChangePosition(image_1, row, col, i, j);
                        Image image_2 = Instantiate(L);
                        image_2.transform.SetParent(canvas.transform);
                        ChangePosition(image_2, row, col, i, j + 1);

                        image_1.gameObject.SetActive(false);
                        image_2.gameObject.SetActive(false);

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
                    // test 10330178
                    else if (i + 1 < row && map[i + 1, j] == 3)  // LongRoad2
                    {
                        Image image_1 = Instantiate(D);
                        image_1.transform.SetParent(canvas.transform);
                        ChangePosition(image_1, row, col, i, j);
                        Image image_3 = Instantiate(U);
                        image_3.transform.SetParent(canvas.transform);
                        ChangePosition(image_3, row, col, i + 1, j);

                        image_1.gameObject.SetActive(false);
                        image_3.gameObject.SetActive(false);

                        rooms.Add(image_1);
                        rooms.Add(image_3);

                        Image image_5 = Instantiate(NowD);
                        image_5.transform.SetParent(canvas.transform);
                        ChangePosition(image_5, row, col, i, j);
                        Image image_7 = Instantiate(NowU);
                        image_7.transform.SetParent(canvas.transform);
                        ChangePosition(image_7, row, col, i + 1, j);

                        image_5.gameObject.SetActive(false);
                        image_7.gameObject.SetActive(false);

                        nowrooms.Add(image_5);
                        nowrooms.Add(image_7);

                        roomTag[i, j] = nowRoom; roomTag[i + 1, j] = nowRoom;
                    }
                }
                else if (map[i, j] == 4)  // 大型
                {
                    Image image_1 = Instantiate(RD);
                    image_1.transform.SetParent(canvas.transform);
                    ChangePosition(image_1, row, col, i, j);
                    Image image_2 = Instantiate(LD);
                    image_2.transform.SetParent(canvas.transform);
                    ChangePosition(image_2, row, col, i, j + 1);
                    Image image_3 = Instantiate(RU);
                    image_3.transform.SetParent(canvas.transform);
                    ChangePosition(image_3, row, col, i + 1, j);
                    Image image_4 = Instantiate(LU);
                    image_4.transform.SetParent(canvas.transform);
                    ChangePosition(image_4, row, col, i + 1, j + 1);

                    image_1.gameObject.SetActive(false);
                    image_2.gameObject.SetActive(false);
                    image_3.gameObject.SetActive(false);
                    image_4.gameObject.SetActive(false);

                    rooms.Add(image_1);
                    rooms.Add(image_2);
                    rooms.Add(image_3);
                    rooms.Add(image_4);

                    Image image_5 = Instantiate(NowRD);
                    image_5.transform.SetParent(canvas.transform);
                    ChangePosition(image_5, row, col, i, j);
                    Image image_6 = Instantiate(NowLD);
                    image_6.transform.SetParent(canvas.transform);
                    ChangePosition(image_6, row, col, i, j + 1);
                    Image image_7 = Instantiate(NowRU);
                    image_7.transform.SetParent(canvas.transform);
                    ChangePosition(image_7, row, col, i + 1, j);
                    Image image_8 = Instantiate(NowLU);
                    image_8.transform.SetParent(canvas.transform);
                    ChangePosition(image_8, row, col, i + 1, j + 1);

                    image_5.gameObject.SetActive(false);
                    image_6.gameObject.SetActive(false);
                    image_7.gameObject.SetActive(false);
                    image_8.gameObject.SetActive(false);

                    nowrooms.Add(image_5);
                    nowrooms.Add(image_6);
                    nowrooms.Add(image_7);
                    nowrooms.Add(image_8);

                    roomTag[i, j] = nowRoom; roomTag[i, j + 1] = nowRoom; roomTag[i + 1, j] = nowRoom; roomTag[i + 1, j + 1] = nowRoom;
                }
                else  // 普通大小
                {
                    Image image_1 = Instantiate(None);
                    image_1.transform.SetParent(canvas.transform);
                    ChangePosition(image_1, row, col, i, j);

                    image_1.gameObject.SetActive(false);

                    rooms.Add(image_1);

                    Image image_2 = Instantiate(NowNone);
                    image_2.transform.SetParent(canvas.transform);
                    ChangePosition(image_2, row, col, i, j);

                    image_2.gameObject.SetActive(false);

                    nowrooms.Add(image_2);

                    roomTag[i, j] = nowRoom;
                    if (map[i, j] == 5)  // Boss
                    {
                        image_tag = Instantiate(BossTag);
                        image_tag.transform.SetParent(canvas.transform);
                        ChangePosition(image_tag, row, col, i, j);
                    }
                    else if (map[i, j] == 6)  // 恶魔
                    {
                        image_tag = Instantiate(DevilTag);
                        image_tag.transform.SetParent(canvas.transform);
                        ChangePosition(image_tag, row, col, i, j);
                    }
                    else if (map[i, j] == 7)  // 商店
                    {
                        image_tag = Instantiate(ShopTag);
                        image_tag.transform.SetParent(canvas.transform);
                        ChangePosition(image_tag, row, col, i, j);
                    }
                    else if (map[i, j] == 8)  // 挑战
                    {
                        image_tag = Instantiate(ChallengeTag);
                        image_tag.transform.SetParent(canvas.transform);
                        ChangePosition(image_tag, row, col, i, j);
                    }
                    else if (map[i, j] == 9)  // 游戏
                    {
                        image_tag = Instantiate(GameTag);
                        image_tag.transform.SetParent(canvas.transform);
                        ChangePosition(image_tag, row, col, i, j);
                    }
                    else if (map[i, j] == 10)  // 献祭
                    {
                        image_tag = Instantiate(SacrificeTag);
                        image_tag.transform.SetParent(canvas.transform);
                        ChangePosition(image_tag, row, col, i, j);
                    }
                    else if (map[i, j] == 11)  // 宝箱
                    {
                        image_tag = Instantiate(TreasureTag);
                        image_tag.transform.SetParent(canvas.transform);
                        ChangePosition(image_tag, row, col, i, j);
                    }
                    else if (map[i, j] == 12)
                    {
                        birthRoom = nowRoom;
                    }
                    if (image_tag != null) image_tag.gameObject.SetActive(false);
                }
                roomToSmallMap.Add(nowRoom, rooms);
                nowroomToSmallMap.Add(nowRoom, nowrooms);
                roomToTag.Add(nowRoom, image_tag);
            }
            
        }
        //ChangeRoom(birthRoom, birthRoom);
    }

    void ChangePosition(Image image, int row, int col, int i, int j)
    {
        image.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        image.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, dOffset + roomSize * (col - j - 1f), roomSize);
        image.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, hOffset + roomSize * (i + 1 - 1f), roomSize);
    }

    public void ChangeRoom(List<int> RoomIDList)
    {
        foreach (var i in nowroomToSmallMap)
        {
            foreach (var j in i.Value)
            {
                j.gameObject.SetActive(false);
            }
        }
        foreach (var i in RoomIDList)
        {
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
