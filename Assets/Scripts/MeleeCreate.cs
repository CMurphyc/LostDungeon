using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeCreate : MonoBehaviour
{
    public GameObject NormalRoom;
    public GameObject LongRoad1Room;
    public GameObject LongRoad2Room;
    public GameObject TreasureRoom;
    public GameObject BossRoom;
    public GameObject ChallengeRoom;
    public GameObject DevilRoom;
    public GameObject GameRoom;
    public GameObject ShopRoom;
    public GameObject SacrificeRoom;
    public GameObject BackGround;
    public Door NormalDoor;
    public Door TreasureDoor;
    public Door BossDoor;
    public Door ChallengeDoor;
    public Door DevilDoor;
    public Door GameDoor;
    public Door SacrificeDoor;
    public GameObject[] Terrain;


    public int xOffset;
    public int yOffset;
    public int[,] roomTag;
    public int startRoom;
    public Dictionary<int, List<GameObject>> roomToStone = new Dictionary<int, List<GameObject>>();   // 房间号对应的石头列表

    public Dictionary<int, List<int>> roomToDoor = new Dictionary<int, List<int>>();   // 房间号对应的门列表
    public Dictionary<int, GameObject> doornumToDoor = new Dictionary<int, GameObject>();   // 门号对应门的实体
    public Dictionary<int, int> doorToRoom = new Dictionary<int, int>();   // 一个编号的门对应的房间编号

    public Dictionary<int, List<GameObject>> roomToMonster;   // 房间号对应的怪物列表

    private readonly int[] startPosition = new int[] { -5, 2, 5, 2, -5, -2, 5, -2 };
    private List<List<int>> roomToDoorTmp = new List<List<int>>();
    private int birthX;
    private int birthY;


    // Start is called before the first frame update
    void Start()
    {

        int playerNum = 2;

        int floorNum = 1;
        int d = 5;
        int h = 5;

        // 生成底部黑幕
        GameObject backGround = Instantiate(BackGround, new Vector3((int)(d / 2) * xOffset, (int)(h / 2) * yOffset, 0), Quaternion.identity);
        backGround.transform.localScale = new Vector3(d + 2, h + 2, 1);

        int[,] array = new int[,]{
            { 12, 9, 0, 11, 9 },
            { 1, 3, 3, 1, 1 },
            { 11, 0, 9, 0, 11 },
            { 1, 1, 3, 3, 1 },
            { 9, 11, 0, 9, 13 }
        };
        MakeGraph(array, h, d, playerNum, floorNum, 0);


        //AstarPath AStar = GameObject.Find("AStar").GetComponent<AstarPath>();
        //AStar.data.gridGraph.Width = (int)(GetRightTop().x - GetLeftBottom().x +1);
        //AStar.data.gridGraph.Depth = (int)(GetRightTop().y - GetLeftBottom().y + 1);

        //Debug.Log(AStar.data.gridGraph.Width);
        //Debug.Log(AStar.data.gridGraph.Depth);
        //AStar.data.gridGraph.center = new Vector3(GetRightTop().x - GetLeftBottom().x, GetRightTop().y - GetLeftBottom().y);
        //Debug.Log(AStar.data.gridGraph.center);
        //AStar.Scan();



    }

    void MakeGraph(int[,] map, int row, int col, int playerNum, int floorNum, int team)
    {

        // 根据传入的矩阵生成整体房间地图

        // Debug.Log("row = " + row + ", col = " + col);
        roomTag = new int[row, col];
        int nowRoom = 0;
        int nowDoor = 0;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                // 如果当前位置不是房间  则跳过
                if (map[i, j] == 0) continue;
                // 如果当前位置已经属于某一个房间  则不生成房间
                if (roomTag[i, j] != 0) continue;
                // Debug.Log(i + "   " + j);
                nowRoom++;
                roomToDoorTmp.Add(new List<int>());
                GameObject room = null;
                GameObject terrain = null;
                if (nowRoom <= Terrain.Length)
                {
                    terrain = Instantiate(Terrain[nowRoom - 1], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                }
                if (map[i, j] == 3)  // 长直道
                {
                    // test 10330172
                    if (j + 1 < col && map[i, j + 1] == 3)  // LongRoad1
                    {
                        room = Instantiate(LongRoad1Room, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                        roomTag[i, j] = nowRoom; roomTag[i, j + 1] = nowRoom;
                        // terrain = Instantiate(NormalLongRoad1Terrain[Random.Range(0, NormalLongRoad1Terrain.Length)], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                    }
                    // test 10330178
                    else if (i + 1 < row && map[i + 1, j] == 3)  // LongRoad2
                    {
                        room = Instantiate(LongRoad2Room, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                        roomTag[i, j] = nowRoom; roomTag[i + 1, j] = nowRoom;
                        // terrain = Instantiate(NormalLongRoad2Terrain[Random.Range(0, NormalLongRoad2Terrain.Length)], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                    }
                }
                else  // 普通大小
                {
                    if (map[i, j] == 9)  // 游戏
                    {
                        room = Instantiate(GameRoom, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // Game
                    }
                    else if (map[i, j] == 11)  // 宝箱
                    {
                        room = Instantiate(TreasureRoom, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // Treasure
                        // terrain = Instantiate(TreasureNormalTerrain[playerNum - 1], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                    }
                    else  // 普通
                    {
                        room = Instantiate(NormalRoom, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // Normal
                        if (map[i, j] == 12 + team)  // 出生点
                        {
                            //  确定出生点的 x y 值
                            birthX = i;
                            birthY = j;
                            startRoom = nowRoom;
                            //kkkkkkkkkkkkkkkkkkkk
                            // terrain = Instantiate(NormalNormalTerrain[0], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 空地形
                            // terrain = Instantiate(NormalNormalTerrain[Random.Range(1, NormalNormalTerrain.Length)], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                        }
                        else
                        {
                            // terrain = Instantiate(NormalNormalTerrain[Random.Range(1, NormalNormalTerrain.Length)], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                        }
                    }
                    roomTag[i, j] = nowRoom;
                }

                //
                //startRoom = bossRoom;
                //  创建石头对象的列表
                List<GameObject> stones = new List<GameObject>();
                List<GameObject> monsters = new List<GameObject>();

                //  获得当前地形下所有的子物体，即所有石头
                if (room != null)
                {
                    Transform father = room.transform;
                    for (int k = 0; k < father.childCount; k++)
                    {
                        GameObject child = father.GetChild(k).gameObject;
                        if (child.tag == "Collision")
                        {
                            Transform childTransform = child.transform;
                            for (int q = 0; q < childTransform.childCount; q++)
                            {
                                GameObject wall = childTransform.GetChild(q).gameObject;
                                if (wall.tag == "Collision")
                                {
                                    stones.Add(wall);
                                }
                            }
                        }
                    }
                }
                //kkkkkkk
                // if (terrain != null && nowRoom != startRoom)
                if (terrain != null)
                {
                    Transform father = terrain.transform;
                    // Debug.Log(terrain + "   " + father.childCount);
                    // Debug.Log(nowRoom + "   " + father.childCount);
                    for (int k = 0; k < father.childCount; k++)
                    {
                        GameObject child = father.GetChild(k).gameObject;
                        if (child.tag == "Collision")
                        {
                            stones.Add(child);
                        }
                        else if (child.tag == "TreasureTable")
                        {

                        }
                    }
                }
                // Debug.Log(stones.Count);
                // roomToStone.Add(nowRoom, stones);
                // Debug.Log(monsters.Count);
                // roomToMonster.Add(nowRoom, monsters);


            }
        }

        // 生成门
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (map[i, j] == 0)
                {
                    continue;
                }
                // 下方有和当前房间不同的房间
                if (i + 1 < row && roomTag[i + 1, j] != 0 && roomTag[i + 1, j] != roomTag[i, j])
                {
                    GameObject door1;
                    GameObject door2;
                    // 游戏门
                    if (map[i, j] == 9 || map[i + 1, j] == 9)
                    {
                        door1 = Instantiate(GameDoor.downDoor, GameDoor.downDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), GameDoor.downDoor.transform.rotation);
                        door2 = Instantiate(GameDoor.upDoor, GameDoor.upDoor.transform.position + new Vector3(xOffset * j, yOffset * (i + 1), 0), GameDoor.upDoor.transform.rotation);
                    }
                    // 宝箱门
                    else if (map[i, j] == 11 || map[i + 1, j] == 11)
                    {
                        door1 = Instantiate(TreasureDoor.downDoor, TreasureDoor.downDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), TreasureDoor.downDoor.transform.rotation);
                        door2 = Instantiate(TreasureDoor.upDoor, TreasureDoor.upDoor.transform.position + new Vector3(xOffset * j, yOffset * (i + 1), 0), TreasureDoor.upDoor.transform.rotation);
                    }
                    // 普通门
                    else
                    {
                        door1 = Instantiate(NormalDoor.downDoor, NormalDoor.downDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), NormalDoor.downDoor.transform.rotation);
                        door2 = Instantiate(NormalDoor.upDoor, NormalDoor.upDoor.transform.position + new Vector3(xOffset * j, yOffset * (i + 1), 0), NormalDoor.upDoor.transform.rotation);
                    }
                    //  门号对应的门实体
                    doornumToDoor.Add(nowDoor, door1);
                    doornumToDoor.Add(nowDoor + 1, door2);
                    //  对应房间号添加门号
                    roomToDoorTmp[roomTag[i, j] - 1].Add(nowDoor);
                    roomToDoorTmp[roomTag[i + 1, j] - 1].Add(nowDoor + 1);
                    //  门号对应的房间号
                    doorToRoom.Add(nowDoor, roomTag[i, j]);
                    doorToRoom.Add(nowDoor + 1, roomTag[i + 1, j]);
                    nowDoor += 2;
                }
                // 右方有和当前房间不同的房间
                if (j + 1 < col && roomTag[i, j + 1] != 0 && roomTag[i, j + 1] != roomTag[i, j])
                {
                    GameObject door1;
                    GameObject door2;
                    if (map[i, j] == 9 || map[i, j + 1] == 9)
                    {
                        door1 = Instantiate(GameDoor.rightDoor, GameDoor.rightDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), GameDoor.rightDoor.transform.rotation);
                        door2 = Instantiate(GameDoor.leftDoor, GameDoor.leftDoor.transform.position + new Vector3(xOffset * (j + 1), yOffset * i, 0), GameDoor.leftDoor.transform.rotation);
                    }
                    // 献祭门
                    // 宝箱门
                    else if (map[i, j] == 11 || map[i, j + 1] == 11)
                    {
                        door1 = Instantiate(TreasureDoor.rightDoor, TreasureDoor.rightDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), TreasureDoor.rightDoor.transform.rotation);
                        door2 = Instantiate(TreasureDoor.leftDoor, TreasureDoor.leftDoor.transform.position + new Vector3(xOffset * (j + 1), yOffset * i, 0), TreasureDoor.leftDoor.transform.rotation);
                    }
                    // 普通门
                    else
                    {
                        door1 = Instantiate(NormalDoor.rightDoor, NormalDoor.rightDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), NormalDoor.rightDoor.transform.rotation);
                        door2 = Instantiate(NormalDoor.leftDoor, NormalDoor.leftDoor.transform.position + new Vector3(xOffset * (j + 1), yOffset * i, 0), NormalDoor.leftDoor.transform.rotation);
                    }
                    //  门号对应的门实体
                    doornumToDoor.Add(nowDoor, door1);
                    doornumToDoor.Add(nowDoor + 1, door2);
                    //  对应房间号添加门号
                    roomToDoorTmp[roomTag[i, j] - 1].Add(nowDoor);
                    roomToDoorTmp[roomTag[i, j + 1] - 1].Add(nowDoor + 1);
                    //  门号对应的房间号
                    doorToRoom.Add(nowDoor, roomTag[i, j]);
                    doorToRoom.Add(nowDoor + 1, roomTag[i, j + 1]);
                    nowDoor += 2;
                }
            }
        }
        for (int i = 0; i < roomToDoorTmp.Count; i++)
        {
            roomToDoor.Add(i + 1, roomToDoorTmp[i]);
        }


    }

}

