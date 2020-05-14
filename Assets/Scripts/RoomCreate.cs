using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomCreate : MonoBehaviour
{
    public GameObject NormalRoom;
    public GameObject LargeRoom;
    public GameObject Lshape1Room;
    public GameObject Lshape2Room;
    public GameObject Lshape3Room;
    public GameObject Lshape4Room;
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
    public GameObject NormalCurtain;
    public GameObject LargeCurtain;
    public GameObject Lshape1Curtain;
    public GameObject Lshape2Curtain;
    public GameObject Lshape3Curtain;
    public GameObject Lshape4Curtain;
    public GameObject LongRoad1Curtain;
    public GameObject LongRoad2Curtain;
    public Door NormalDoor;
    public Door TreasureDoor;
    public Door BossDoor;
    public Door ChallengeDoor;
    public Door DevilDoor;
    public Door GameDoor;
    public Door SacrificeDoor;
    public Monster[] MonsterList;
    public Monster[] BossList;
    public int[] MonsterInitHP;

    public GameObject[] NormalNormalTerrain;
    public GameObject[] NormalLshape1Terrain;
    public GameObject[] NormalLshape2Terrain;
    public GameObject[] NormalLshape3Terrain;
    public GameObject[] NormalLshape4Terrain;
    public GameObject[] NormalLongRoad1Terrain;
    public GameObject[] NormalLongRoad2Terrain;
    public GameObject[] NormalLargeTerrain;
    public GameObject[] TreasureNormalTerrain;
    public GameObject[] BossNormalTerrain;
    public GameObject[] DevilNormalTerrain;
    public GameObject[] ChallengeNormalTerrain;

    public int xOffset;
    public int yOffset;
    public int[,] roomTag;
    public int startRoom;
    public Dictionary<int, List<GameObject>> roomToStone ;   // 房间号对应的石头列表

    public Dictionary<int, List<int>> roomToDoor ;   // 房间号对应的门列表
    public Dictionary<int, GameObject> doornumToDoor ;   // 门号对应门的实体
    public Dictionary<int, DoorData> doorToDoor ;   // 一个编号的门传送到的另一个门的编号
    public Dictionary<int, int> doorToRoom ;   // 一个编号的门对应的房间编号
  
    public Dictionary<int, PlayerInGameData> playerToPlayer ;   // 玩家编号对应玩家信息
    public Dictionary<int, List<GameObject>> roomToMonster ;   // 房间号对应的怪物列表

    public Dictionary<int, List<TreasureData>> roomToTreasure;   // 房间号对应宝物列表
    public Dictionary<int, PropData> propToProperty;   // 根据道具名称找到对应道具属性

    public Dictionary<int, GameObject> roomToCurtain = new Dictionary<int, GameObject>();   // 房间对应的幕布实体

    private readonly int[] startPosition = new int[] { -5, 2, 5, 2, -5, -2, 5, -2 };
    private List<List<int>> roomToDoorTmp = new List<List<int>>();
    private int birthX;
    private int birthY;
    private int bossRoom;

    SystemManager sys;
    private void Awake()
    {
        sys = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem;


        //人物初始化
        playerToPlayer = sys._battle._player.playerToPlayer;
        //怪物初始化
        roomToMonster = sys._battle._monster.RoomToMonster;
        //地形初始化
        roomToStone = sys._battle._terrain.roomToStone;
        roomToDoor = sys._battle._terrain.roomToDoor;
        doornumToDoor = sys._battle._terrain.doornumToDoor;
        doorToDoor = sys._battle._terrain.doorToDoor;
        doorToRoom = sys._battle._terrain.doorToRoom;

        //道具初始化
        roomToTreasure = sys._battle._chest.roomToTreasure;
        propToProperty = sys._battle._chest.propToProperty;
        // 加载道具信息
        UploadPropAttr();

        int playerNum = sys._model._RoomModule.GetPlayerSize();

        int seed = sys._model._RoomModule.MapSeed;
        Random.InitState(seed);
        int floorNum = sys._model._RoomModule.MapFloorNumber;
        RandMap.StartRand(seed, playerNum, floorNum);
        int d = RandMap.GetWidth() + 1;
        int h = RandMap.GetHeight() + 1;
        int[,] array = new int[h, d];
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < d; j++)
            {
                array[i, j] = RandMap.GetValue(i, j);
            }
        }
        MakeGraph(array, h, d, playerNum, floorNum);

        // 生成底部黑幕
        GameObject backGround = Instantiate(BackGround, new Vector3((int)(d / 2 + 1) * xOffset, (int)(h / 2 + 1) * yOffset, 0), Quaternion.identity);
        backGround.transform.localScale = new Vector3(d + 2, h + 2, 1);

        //Debug.Log("Astar");
        AstarPath AStar = GameObject.Find("AStar").GetComponent<AstarPath>();
        int Width = (int)(GetRightTop().x - GetLeftBottom().x + 1);
        int Depth = (int)(GetRightTop().y - GetLeftBottom().y + 1);
        AStar.data.gridGraph.center = new Vector3((int)((GetRightTop().x + GetLeftBottom().x) / 2 + 1), (int)((GetRightTop().y + GetLeftBottom().y) / 2 + 1));
        AStar.data.gridGraph.SetDimensions(Width, Depth, 1);
        AStar.Scan();


        List<PlayerData> PlayerList = sys._model._RoomModule.PlayerList;
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (!PlayerList[i].empty)
            {
                CreatePlayer(i, PlayerList[i].uid, PlayerList[i].type);
                GameObject tplayer = sys._battle._player.playerToPlayer[PlayerList[i].uid].obj;

                if (!sys._model._BagModule.PlayerBag.ContainsKey(PlayerList[i].uid))
                {
                    //if (!sys._model._BagModule.PlayerHP.ContainsKey(PlayerList[i].uid)) continue;
                    //tplayer.GetComponent<PlayerModel_Component>().SetHealthPoint(sys._model._BagModule.PlayerHP[PlayerList[i].uid]);
                    continue;
                }

                foreach (var x in sys._model._BagModule.PlayerBag[PlayerList[i].uid])
                {
                    for (int j = 0; j < x.ItemNumber; j++)
                        tplayer.GetComponent<PlayerModel_Component>().Change(
                            sys._battle._chest.propToProperty[x.ItemID].changefullHP,
                            sys._battle._chest.propToProperty[x.ItemID].changeHP,
                            (Fix64)sys._battle._chest.propToProperty[x.ItemID].changeBulletFrequency,
                            (Fix64)sys._battle._chest.propToProperty[x.ItemID].changeBulletSpeed,
                            (Fix64)sys._battle._chest.propToProperty[x.ItemID].changeDamage,
                            (Fix64)sys._battle._chest.propToProperty[x.ItemID].changeSpeed,
                            sys._battle._chest.propToProperty[x.ItemID].bulletType
                            );

                    //Debug.Log("bbbbbbb" + x.ItemID);
                }
                //if (!sys._model._BagModule.PlayerHP.ContainsKey(PlayerList[i].uid)) continue;
                //tplayer.GetComponent<PlayerModel_Component>().SetHealthPoint(sys._model._BagModule.PlayerHP[PlayerList[i].uid]);



            }
        }

        ChangeSkillIcon();
        //怪物初始化
        sys._battle._monster.RoomToMonster = roomToMonster;
        //人物初始化
        sys._battle._player.playerToPlayer = playerToPlayer;
        //地形初始化
        sys._battle._terrain.roomToStone = roomToStone;
        sys._battle._terrain.roomToDoor = roomToDoor;
        sys._battle._terrain.doornumToDoor = doornumToDoor;
        sys._battle._terrain.doorToDoor = doorToDoor;
        sys._battle._terrain.doorToRoom = doorToRoom;
        sys._battle._monster.BossRoom = bossRoom;

        sys._battle._chest.roomToTreasure = roomToTreasure;
        sys._battle._chest.propToProperty = propToProperty;


        //初始化相机
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (!PlayerList[i].empty && PlayerList[i].uid == sys._model._PlayerModule.uid)
            {
                Camera.main.GetComponent<CameraController>().SetTarget(playerToPlayer[PlayerList[i].uid].obj);
                break;
            }
        }


    }


    void MakeGraph(int[,] map, int row, int col, int playerNum, int floorNum)
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
                GameObject curtain = null;
                if (map[i, j] == 2)  // L形
                {
                    // test 10330171
                    if (j + 1 < col && map[i, j + 1] == 2 && i + 1 < row && map[i + 1, j + 1] == 2)  // Lshape1
                    {
                        room = Instantiate(Lshape1Room, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                        terrain = Instantiate(NormalLshape1Terrain[Random.Range(0, NormalLshape1Terrain.Length)], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                        roomTag[i, j] = nowRoom; roomTag[i, j + 1] = nowRoom; roomTag[i + 1, j + 1] = nowRoom;
                        curtain = Instantiate(Lshape1Curtain, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                    }
                    else if (j + 1 < col && map[i, j + 1] == 2 && i + 1 < row && map[i + 1, j] == 2)  // Lshape2
                    {
                        room = Instantiate(Lshape2Room, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                        roomTag[i, j] = nowRoom; roomTag[i, j + 1] = nowRoom; roomTag[i + 1, j] = nowRoom;
                        terrain = Instantiate(NormalLshape2Terrain[Random.Range(0, NormalLshape2Terrain.Length)], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                        curtain = Instantiate(Lshape2Curtain, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                    }
                    // test 10330178
                    else if (i + 1 < col && map[i + 1, j] == 2 && j + 1 < row && map[i + 1, j + 1] == 2)  // Lshape3
                    {
                        room = Instantiate(Lshape3Room, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                        roomTag[i, j] = nowRoom; roomTag[i + 1, j] = nowRoom; roomTag[i + 1, j + 1] = nowRoom;
                        terrain = Instantiate(NormalLshape3Terrain[Random.Range(0, NormalLshape3Terrain.Length)], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                        curtain = Instantiate(Lshape3Curtain, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                    }
                    // test 10330178
                    else if (i + 1 < col && map[i + 1, j] == 2 && j - 1 < row && map[i + 1, j - 1] == 2)  // Lshape4
                    {
                        room = Instantiate(Lshape4Room, new Vector3(xOffset * (j - 1), yOffset * i, 0), Quaternion.identity);
                        roomTag[i, j] = nowRoom; roomTag[i + 1, j] = nowRoom; roomTag[i + 1, j - 1] = nowRoom;
                        terrain = Instantiate(NormalLshape4Terrain[Random.Range(0, NormalLshape4Terrain.Length)], new Vector3(xOffset * (j - 1), yOffset * i, 0), Quaternion.identity);  // 随机地形
                        curtain = Instantiate(Lshape4Curtain, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                    }
                }
                else if (map[i, j] == 3)  // 长直道
                {
                    // test 10330172
                    if (j + 1 < col && map[i, j + 1] == 3)  // LongRoad1
                    {
                        room = Instantiate(LongRoad1Room, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                        roomTag[i, j] = nowRoom; roomTag[i, j + 1] = nowRoom;
                        terrain = Instantiate(NormalLongRoad1Terrain[Random.Range(0, NormalLongRoad1Terrain.Length)], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                        curtain = Instantiate(LongRoad1Curtain, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                    }
                    // test 10330178
                    else if (i + 1 < row && map[i + 1, j] == 3)  // LongRoad2
                    {
                        room = Instantiate(LongRoad2Room, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                        roomTag[i, j] = nowRoom; roomTag[i + 1, j] = nowRoom;
                        terrain = Instantiate(NormalLongRoad2Terrain[Random.Range(0, NormalLongRoad2Terrain.Length)], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                        curtain = Instantiate(LongRoad2Curtain, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                    }
                }
                else if (map[i, j] == 4)  // 大型
                {
                    room = Instantiate(LargeRoom, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // Large
                    roomTag[i, j] = nowRoom; roomTag[i, j + 1] = nowRoom; roomTag[i + 1, j] = nowRoom; roomTag[i + 1, j + 1] = nowRoom;
                    terrain = Instantiate(NormalLargeTerrain[Random.Range(0, NormalLargeTerrain.Length)], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                    curtain = Instantiate(LargeCurtain, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                }
                else  // 普通大小
                {
                    if (map[i, j] == 5)  // Boss
                    {
                        bossRoom = nowRoom;
                        room = Instantiate(BossRoom, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // Boss
                        terrain = Instantiate(BossNormalTerrain[Random.Range(0, BossNormalTerrain.Length)], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                    }
                    else if (map[i, j] == 6)  // 恶魔
                    {
                        room = Instantiate(DevilRoom, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // Devil
                        terrain = Instantiate(DevilNormalTerrain[Random.Range(0, DevilNormalTerrain.Length)], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                    }
                    else if (map[i, j] == 7)  // 商店
                    {
                        room = Instantiate(ShopRoom, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // Shop
                    }
                    else if (map[i, j] == 8)  // 挑战
                    {
                        room = Instantiate(ChallengeRoom, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // Challenge
                        terrain = Instantiate(ChallengeNormalTerrain[Random.Range(0, ChallengeNormalTerrain.Length)], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                    }
                    else if (map[i, j] == 9)  // 游戏
                    {
                        room = Instantiate(GameRoom, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // Game
                    }
                    else if (map[i, j] == 10)  // 献祭
                    {
                        room = Instantiate(SacrificeRoom, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // Sacrifice
                    }
                    else if (map[i, j] == 11)  // 宝箱
                    {
                        room = Instantiate(TreasureRoom, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // Treasure
                        terrain = Instantiate(TreasureNormalTerrain[playerNum - 1], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                    }
                    else  // 普通
                    {
                        room = Instantiate(NormalRoom, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // Normal
                        if (map[i, j] == 12)  // 出生点
                        {
                            //  确定出生点的 x y 值
                            birthX = i;
                            birthY = j;
                            startRoom = nowRoom;
                            //kkkkkkkkkkkkkkkkkkkk
                            terrain = Instantiate(NormalNormalTerrain[0], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 空地形
                            // terrain = Instantiate(NormalNormalTerrain[Random.Range(1, NormalNormalTerrain.Length)], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                        }
                        else
                        {
                            terrain = Instantiate(NormalNormalTerrain[Random.Range(1, NormalNormalTerrain.Length)], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                        }
                    }
                    curtain = Instantiate(NormalCurtain, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);
                    roomTag[i, j] = nowRoom;
                }

                //
                //startRoom = bossRoom;
                //  创建石头对象的列表
                List<GameObject> stones = new List<GameObject>();
                List<GameObject> monsters = new List<GameObject>();
                List<TreasureData> treasures = new List<TreasureData>();

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
                    int monsterNum = 0;
                    // Debug.Log(terrain + "   " + father.childCount);
                    // Debug.Log(nowRoom + "   " + father.childCount);
                    for (int k = 0; k < father.childCount; k++)
                    {
                        GameObject child = father.GetChild(k).gameObject;
                        if (child.tag == "Collision")
                        {
                            stones.Add(child);
                        }
                        else if (child.tag == "MonsterPoint")
                        {
                            if (monsterNum % (5 - playerNum) == 0)
                            {
                                if (map[i, j] == 5)
                                {
                                    if (BossList.Length > floorNum - 1)
                                    {
                                        if (BossList[floorNum - 1] != null)
                                        {
                                            FixVector2 MonsterSpawningPoint = PackConverter.Vector3ToFixVector2(child.transform.position);
                                            sys._battle._terrain.BossSpawningPoint = MonsterSpawningPoint;
                                            if (BossList[floorNum - 1].type == AI_Type.Boss_Rabit)
                                            {
                                                GameObject monster = Instantiate(BossList[floorNum - 1].monsterGameObject, child.transform.position, Quaternion.identity);
                                                monster.GetComponent<MonsterModel_Component>().position = PackConverter.Vector3ToFixVector3(monster.transform.position);
                                                monster.GetComponent<MonsterModel_Component>().HP = (Fix64)200;
                                                monster.GetComponent<MonsterModel_Component>().MaxHP = (Fix64)200;
                                                monster.GetComponent<MonsterModel_Component>().MoveSpeed = (Fix64)2;
                                                monster.GetComponent<EnemyAI>().InitAI(BossList[floorNum - 1].type, nowRoom,null);
                                                monsters.Add(monster);
                                            }
                                            else if (BossList[floorNum - 1].type == AI_Type.Boss_Wizard)
                                            {
                                                GameObject monster = Instantiate(BossList[floorNum - 1].monsterGameObject, child.transform.position, Quaternion.identity);
                                                monster.GetComponent<MonsterModel_Component>().position = PackConverter.Vector3ToFixVector3(monster.transform.position);
                                                monster.GetComponent<MonsterModel_Component>().HP = (Fix64)200;

                                                monster.GetComponent<MonsterModel_Component>().MaxHP = (Fix64)200;
                                                monster.GetComponent<MonsterModel_Component>().MoveSpeed = (Fix64)2;
                                                monster.GetComponent<EnemyAI>().InitAI(BossList[floorNum - 1].type, nowRoom, null);
                                                monsters.Add(monster);

                                            }

                                            else if (BossList[floorNum - 1].type == AI_Type.Boss_DarkKnight)
                                            {
                                                GameObject monster = Instantiate(BossList[floorNum - 1].monsterGameObject, child.transform.position, Quaternion.identity);
                                                monster.GetComponent<MonsterModel_Component>().position = PackConverter.Vector3ToFixVector3(monster.transform.position);
                                                monster.GetComponent<MonsterModel_Component>().HP = (Fix64)200;

                                                monster.GetComponent<MonsterModel_Component>().MaxHP = (Fix64)200;
                                                monster.GetComponent<MonsterModel_Component>().MoveSpeed = (Fix64)2;
                                                monster.GetComponent<EnemyAI>().InitAI(BossList[floorNum - 1].type, nowRoom, null);

                                                monsters.Add(monster);

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    int index = Random.Range(0, MonsterList.Length);
                                    GameObject monster = Instantiate(MonsterList[index].monsterGameObject, child.transform.position, Quaternion.identity);
                                    monster.GetComponent<MonsterModel_Component>().position = PackConverter.Vector3ToFixVector3(monster.transform.position);
                                    monster.GetComponent<MonsterModel_Component>().HP = (Fix64)MonsterInitHP[index];
                                    monster.GetComponent<EnemyAI>().InitAI(MonsterList[index].type, nowRoom, null);
                                    monsters.Add(monster);
                                }
                            }
                            monsterNum++;
                        }
                        else if (child.tag == "TreasureTable")
                        {
                            // 根据随机生成的道具下标来创建道具实体
                            int treasureId = Random.Range(0,propToProperty.Count);
                            GameObject treasure = Instantiate(propToProperty[treasureId].propObject, child.transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);
                            // Debug.Log(treasure.name);
                            //Debug.Log(treasure.name + "   " + treasureId);
                            stones.Add(child);
                            treasures.Add(new TreasureData(treasureId, propToProperty[treasureId].propType, child, treasure, false));
                        }
                    }
                }
                // Debug.Log(stones.Count);
                roomToStone.Add(nowRoom, stones);
                // Debug.Log(monsters.Count);
                roomToMonster.Add(nowRoom, monsters);
                // Debug.Log(treasures.Count);
                roomToTreasure.Add(nowRoom, treasures);

                curtain.SetActive(false);
                roomToCurtain.Add(nowRoom, curtain);
                
            }
        }

        foreach(var x in roomToCurtain)
        {
            x.Value.SetActive(true);
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
                    // 恶魔门
                    if (map[i, j] == 6 || map[i + 1, j] == 6)
                    {
                        door1 = Instantiate(DevilDoor.downDoor, DevilDoor.downDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), DevilDoor.downDoor.transform.rotation);
                        door2 = Instantiate(DevilDoor.upDoor, DevilDoor.upDoor.transform.position + new Vector3(xOffset * j, yOffset * (i + 1), 0), DevilDoor.upDoor.transform.rotation);
                    }
                    // BOSS门
                    else if (map[i, j] == 5 || map[i + 1, j] == 5)
                    {
                        door1 = Instantiate(BossDoor.downDoor, BossDoor.downDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), BossDoor.downDoor.transform.rotation);
                        door2 = Instantiate(BossDoor.upDoor, BossDoor.upDoor.transform.position + new Vector3(xOffset * j, yOffset * (i + 1), 0), BossDoor.upDoor.transform.rotation);
                    }
                    // 挑战门
                    else if (map[i, j] == 8 || map[i + 1, j] == 8)
                    {
                        door1 = Instantiate(ChallengeDoor.downDoor, ChallengeDoor.downDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), ChallengeDoor.downDoor.transform.rotation);
                        door2 = Instantiate(ChallengeDoor.upDoor, ChallengeDoor.upDoor.transform.position + new Vector3(xOffset * j, yOffset * (i + 1), 0), ChallengeDoor.upDoor.transform.rotation);
                    }
                    // 游戏门
                    else if (map[i, j] == 9 || map[i + 1, j] == 9)
                    {
                        door1 = Instantiate(GameDoor.downDoor, GameDoor.downDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), GameDoor.downDoor.transform.rotation);
                        door2 = Instantiate(GameDoor.upDoor, GameDoor.upDoor.transform.position + new Vector3(xOffset * j, yOffset * (i + 1), 0), GameDoor.upDoor.transform.rotation);
                    }
                    // 献祭门
                    else if (map[i, j] == 10 || map[i + 1, j] == 10)
                    {
                        door1 = Instantiate(SacrificeDoor.downDoor, SacrificeDoor.downDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), SacrificeDoor.downDoor.transform.rotation);
                        door2 = Instantiate(SacrificeDoor.upDoor, SacrificeDoor.upDoor.transform.position + new Vector3(xOffset * j, yOffset * (i + 1), 0), SacrificeDoor.upDoor.transform.rotation);
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
                    //  门号对应的另一个门号与传送位置
                    doorToDoor.Add(nowDoor, new DoorData(nowDoor + 1, door2.transform.position + new Vector3(0, -1.5f, 0)));
                    doorToDoor.Add(nowDoor + 1, new DoorData(nowDoor, door1.transform.position + new Vector3(0, 1.5f, 0)));
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
                    // 恶魔门
                    if (map[i, j] == 6 || map[i, j + 1] == 6)
                    {
                        door1 = Instantiate(DevilDoor.rightDoor, DevilDoor.rightDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), DevilDoor.rightDoor.transform.rotation);
                        door2 = Instantiate(DevilDoor.leftDoor, DevilDoor.leftDoor.transform.position + new Vector3(xOffset * (j + 1), yOffset * i, 0), DevilDoor.leftDoor.transform.rotation);
                    }
                    // BOSS门
                    else if (map[i, j] == 5 || map[i, j + 1] == 5)
                    {
                        door1 = Instantiate(BossDoor.rightDoor, BossDoor.rightDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), BossDoor.rightDoor.transform.rotation);
                        door2 = Instantiate(BossDoor.leftDoor, BossDoor.leftDoor.transform.position + new Vector3(xOffset * (j + 1), yOffset * i, 0), BossDoor.leftDoor.transform.rotation);
                    }
                    // 挑战门
                    else if (map[i, j] == 8 || map[i, j + 1] == 8)
                    {
                        door1 = Instantiate(ChallengeDoor.rightDoor, ChallengeDoor.rightDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), ChallengeDoor.rightDoor.transform.rotation);
                        door2 = Instantiate(ChallengeDoor.leftDoor, ChallengeDoor.leftDoor.transform.position + new Vector3(xOffset * (j + 1), yOffset * i, 0), ChallengeDoor.leftDoor.transform.rotation);
                    }
                    // 游戏门
                    else if (map[i, j] == 9 || map[i, j + 1] == 9)
                    {
                        door1 = Instantiate(GameDoor.rightDoor, GameDoor.rightDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), GameDoor.rightDoor.transform.rotation);
                        door2 = Instantiate(GameDoor.leftDoor, GameDoor.leftDoor.transform.position + new Vector3(xOffset * (j + 1), yOffset * i, 0), GameDoor.leftDoor.transform.rotation);
                    }
                    // 献祭门
                    else if (map[i, j] == 10 || map[i, j + 1] == 10)
                    {
                        door1 = Instantiate(SacrificeDoor.rightDoor, SacrificeDoor.rightDoor.transform.position + new Vector3(xOffset * j, yOffset * i, 0), SacrificeDoor.rightDoor.transform.rotation);
                        door2 = Instantiate(SacrificeDoor.leftDoor, SacrificeDoor.leftDoor.transform.position + new Vector3(xOffset * (j + 1), yOffset * i, 0), SacrificeDoor.leftDoor.transform.rotation);
                    }
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
                    //  门号对应的另一个门号与传送位置
                    doorToDoor.Add(nowDoor, new DoorData(nowDoor + 1, door2.transform.position + new Vector3(1.5f, 0, 0)));
                    doorToDoor.Add(nowDoor + 1, new DoorData(nowDoor, door1.transform.position + new Vector3(-1.5f, 0, 0)));
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


    public void ChangeSkillIcon()
    {
        int PlayerUID = sys._model._PlayerModule.uid;
        CharacterType PlayerType = sys._model._RoomModule.GetCharacterType(PlayerUID);

        GameObject js1 = GameObject.Find("SkillStickUI1"), js2=GameObject.Find("SkillStickUI2"),js3 = GameObject.Find("SkillStickUI3");

        switch (PlayerType)
        {
            case CharacterType.Enginner:
                {
                    GameObject PlayerObject = sys._battle._player.FindPlayerObjByUID(PlayerUID);
                    js1.GetComponent<Image>().sprite = sys._battle._skill.enginerBase.skill1Image;
                    js1.GetComponent<SkillIndiactor>().Init(sys._battle._skill.enginerBase.Skill1Range(),
                                                sys._battle._skill.enginerBase.Skill1Area(), PlayerObject,sys._battle._skill.enginerBase.skill1Type);
                    js2.GetComponent<Image>().sprite = sys._battle._skill.enginerBase.skill2Image;
                    js2.GetComponent<SkillIndiactor>().Init(sys._battle._skill.enginerBase.Skill2Range(),
                                                sys._battle._skill.enginerBase.Skill2Area(), PlayerObject, sys._battle._skill.enginerBase.skill2Type);

                    js3.GetComponent<Image>().sprite = sys._battle._skill.enginerBase.skill3Image;
                    js3.GetComponent<SkillIndiactor>().Init(sys._battle._skill.enginerBase.Skill3Range(),
                                                sys._battle._skill.enginerBase.Skill3Area(), PlayerObject, sys._battle._skill.enginerBase.skill3Type);

                    break;
                }
            case CharacterType.Magician:
                {
                    GameObject PlayerObject = sys._battle._player.FindPlayerObjByUID(PlayerUID);
                    js1.GetComponent<Image>().sprite = sys._battle._skill.magicianBase.skill1Image;
                    js1.GetComponent<SkillIndiactor>().Init(sys._battle._skill.magicianBase.Skill1Range(),
                                                sys._battle._skill.magicianBase.Skill1Area(), PlayerObject, sys._battle._skill.magicianBase.skill1Type);
                    js2.GetComponent<Image>().sprite = sys._battle._skill.magicianBase.skill2Image;
                    js2.GetComponent<SkillIndiactor>().Init(sys._battle._skill.magicianBase.Skill2Range(),
                                                sys._battle._skill.magicianBase.Skill2Area(), PlayerObject, sys._battle._skill.magicianBase.skill2Type);
                    js3.GetComponent<Image>().sprite = sys._battle._skill.magicianBase.skill3Image;
                    js3.GetComponent<SkillIndiactor>().Init(sys._battle._skill.magicianBase.Skill3Range(),
                                                sys._battle._skill.magicianBase.Skill3Area(), PlayerObject, sys._battle._skill.magicianBase.skill3Type);

                    break;
                }
            case CharacterType.Warrior:
                {
                    GameObject PlayerObject = sys._battle._player.FindPlayerObjByUID(PlayerUID);
                    js1.GetComponent<Image>().sprite = sys._battle._skill.guardianBase.skill1Image;
                    js1.GetComponent<SkillIndiactor>().Init(sys._battle._skill.guardianBase.Skill1Range(),
                                                sys._battle._skill.guardianBase.Skill1Area(), PlayerObject, sys._battle._skill.guardianBase.skill1Type);
                    js2.GetComponent<Image>().sprite = sys._battle._skill.guardianBase.skill2Image;
                    js2.GetComponent<SkillIndiactor>().Init(sys._battle._skill.guardianBase.Skill2Range(),
                                                sys._battle._skill.guardianBase.Skill2Area(), PlayerObject, sys._battle._skill.guardianBase.skill2Type);
                    js3.GetComponent<Image>().sprite = sys._battle._skill.guardianBase.skill3Image;
                    js3.GetComponent<SkillIndiactor>().Init(sys._battle._skill.guardianBase.Skill3Range(),
                                                sys._battle._skill.guardianBase.Skill3Area(), PlayerObject, sys._battle._skill.guardianBase.skill3Type);

                    break;
                }
            case CharacterType.Ghost:
                {
                    GameObject PlayerObject = sys._battle._player.FindPlayerObjByUID(PlayerUID);
                    js1.GetComponent<Image>().sprite = sys._battle._skill.ghostBase.skill1Image;
                    js1.GetComponent<SkillIndiactor>().Init(sys._battle._skill.ghostBase.Skill1Range(),
                                                sys._battle._skill.ghostBase.Skill1Area(), PlayerObject, sys._battle._skill.ghostBase.skill1Type);
                    js2.GetComponent<Image>().sprite = sys._battle._skill.ghostBase.skill2Image;
                    js2.GetComponent<SkillIndiactor>().Init(sys._battle._skill.ghostBase.Skill2Range(),
                                                sys._battle._skill.ghostBase.Skill2Area(), PlayerObject, sys._battle._skill.ghostBase.skill2Type);

                    js3.GetComponent<Image>().sprite = sys._battle._skill.ghostBase.skill3Image;
                    js3.GetComponent<SkillIndiactor>().Init(sys._battle._skill.ghostBase.Skill3Range(),
                                                sys._battle._skill.ghostBase.Skill3Area(), PlayerObject, sys._battle._skill.ghostBase.skill3Type);
                    break;
                }
            default:
                break;
        }
       
        
        

    }

    public void CreatePlayer(int playerNum , int uid,CharacterType type)
    {
        FixVector2 SpwanPos = new FixVector2((Fix64)xOffset * birthY + startPosition[playerNum * 2], (Fix64)yOffset * birthX + startPosition[playerNum * 2 + 1]);
        //  创建玩家实体并根据玩家编号来决定出生位置
        switch(type)
        {
            case  CharacterType.Enginner:
            {
                GameObject playerTmp = Instantiate(sys._battle._skill.enginerBase.obj,
                new Vector3(xOffset * birthY + startPosition[playerNum * 2], yOffset * birthX + startPosition[playerNum * 2 + 1], 0),
                Quaternion.identity);

                playerTmp.transform.localScale = new Vector3(3, 3, 1);

                    playerTmp.GetComponent<PlayerModel_Component>().Init(sys._battle._skill.enginerBase.HP,
                        (Fix64)sys._battle._skill.enginerBase.moveSpeed,
                        (Fix64)sys._battle._skill.enginerBase.damge,
                        (Fix64)sys._battle._skill.enginerBase.bulletSpeed,
                        (Fix64)sys._battle._skill.enginerBase.fireSpeed,
                        sys._battle._skill.enginerBase.bulletEffect
                        );

                playerTmp.GetComponent<PlayerModel_Component>().SetPlayerPosition(SpwanPos);
                PlayerInGameData data = new PlayerInGameData();
                data.obj = playerTmp;
                data.RoomID = startRoom;
                playerToPlayer.Add(uid, data);
                    if (uid == sys._model._PlayerModule.uid)
                    {
                        //绿色
                        playerTmp.transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
                    }
                    else
                    {
                        //蓝色
                        playerTmp.transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 74f / 255f, 1, 1);
                    }
                    break;
            }
            case CharacterType.Magician:
            {
                    GameObject playerTmp = Instantiate(sys._battle._skill.magicianBase.obj,
                    new Vector3(xOffset * birthY + startPosition[playerNum * 2], yOffset * birthX + startPosition[playerNum * 2 + 1], 0),
                    Quaternion.identity);

                    playerTmp.transform.localScale = new Vector3(3, 3, 1);

                    playerTmp.GetComponent<PlayerModel_Component>().Init(sys._battle._skill.magicianBase.HP,
                        (Fix64)sys._battle._skill.magicianBase.moveSpeed,
                        (Fix64)sys._battle._skill.magicianBase.damge,
                        (Fix64)sys._battle._skill.magicianBase.bulletSpeed,
                        (Fix64)sys._battle._skill.magicianBase.fireSpeed,
                        sys._battle._skill.magicianBase.bulletEffect
                        );

                    playerTmp.GetComponent<PlayerModel_Component>().SetPlayerPosition(SpwanPos);
                    PlayerInGameData data = new PlayerInGameData();
                    data.obj = playerTmp;
                    data.RoomID = startRoom;
                    playerToPlayer.Add(uid, data);
                    if (uid == sys._model._PlayerModule.uid)
                    {
                        //绿色
                        playerTmp.transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
                    }
                    else
                    {
                        //蓝色
                        playerTmp.transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 74f / 255f, 1, 1);
                    }
                    break;
            }



            case CharacterType.Ghost:
                {
                    GameObject playerTmp = Instantiate(sys._battle._skill.ghostBase.obj,
                        new Vector3(xOffset * birthY + startPosition[playerNum * 2], yOffset * birthX + startPosition[playerNum * 2 + 1], 0),
                    Quaternion.identity);

                    playerTmp.transform.localScale = new Vector3(3, 3, 1);

                    playerTmp.GetComponent<PlayerModel_Component>().Init(sys._battle._skill.ghostBase.HP,
                       (Fix64)sys._battle._skill.ghostBase.moveSpeed,
                       (Fix64)sys._battle._skill.ghostBase.damge,
                       (Fix64)sys._battle._skill.ghostBase.bulletSpeed,
                       (Fix64)sys._battle._skill.ghostBase.fireSpeed,
                       sys._battle._skill.ghostBase.bulletEffect);

                        playerTmp.GetComponent<PlayerModel_Component>().SetPlayerPosition(SpwanPos);
                    PlayerInGameData data = new PlayerInGameData();
                    data.obj = playerTmp;
                    data.RoomID = startRoom;
                    playerToPlayer.Add(uid, data);
                    if (uid == sys._model._PlayerModule.uid)
                    {
                        //绿色
                        playerTmp.transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
                    }
                    else
                    {
                        //蓝色
                        playerTmp.transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 74f / 255f, 1, 1);
                    }
                    break;
                }

            case CharacterType.Warrior:
                {
                    GameObject playerTmp = Instantiate(sys._battle._skill.guardianBase.obj,

                    new Vector3(xOffset * birthY + startPosition[playerNum * 2], yOffset * birthX + startPosition[playerNum * 2 + 1], 0),
                    Quaternion.identity);

                    playerTmp.transform.localScale = new Vector3(3, 3, 1);
                    playerTmp.GetComponent<PlayerModel_Component>().Init(sys._battle._skill.guardianBase.HP,
                        (Fix64)sys._battle._skill.guardianBase.moveSpeed,
                        (Fix64)sys._battle._skill.guardianBase.damge,
                        (Fix64)sys._battle._skill.guardianBase.bulletSpeed,
                        (Fix64)sys._battle._skill.guardianBase.fireSpeed,
                        sys._battle._skill.guardianBase.bulletEffect

                        );

                    playerTmp.GetComponent<PlayerModel_Component>().SetPlayerPosition(SpwanPos);
                    PlayerInGameData data = new PlayerInGameData();
                    data.obj = playerTmp;
                    data.RoomID = startRoom;
                    playerToPlayer.Add(uid, data);

                    if (uid == sys._model._PlayerModule.uid)
                    {
                        //绿色
                        playerTmp.transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
                    }
                    else
                    {
                        //蓝色
                        playerTmp.transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 74f / 255f, 1, 1);
                    }
                    break;
                }

            default:
                break;
        }
    }

    public Vector2 GetLeftBottom()
    {
        return new Vector2(-10, RandMap.GetHeight() * yOffset - 5);
    }

    public Vector2 GetRightTop()
    {
        return new Vector2(RandMap.GetWidth() * xOffset + 9, 4);
    }

    public void UploadPropAttr()
    {
        // 根据配置表中的道具编号来添加对应物体的一系列属性
        List<PropData> propConfigList = (Resources.Load("Config/PropConfig") as PropConfig).prop_config_list;
        for (int i = 0; i < propConfigList.Count; i++)
        {
            //Debug.Log(propConfigList[i].propObject.name + "   " + i);
            propToProperty.Add(i, propConfigList[i]);
        }

    }
}

