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
    public Dictionary<int, DoorData> doorToDoor=new Dictionary<int, DoorData>();   // 一个编号的门传送到的另一个门的编号
    public Dictionary<int, int> doorToRoom = new Dictionary<int, int>();   // 一个编号的门对应的房间编号

    public Dictionary<int, List<GameObject>> roomToMonster;   // 房间号对应的怪物列表

    private readonly float[] startPosition = new float[] { -5, 2, -2.5f, -1, 0, 2, 2.5f, -1, 5, 2 };
    private List<List<int>> roomToDoorTmp = new List<List<int>>();
    private int birthX;
    private int birthY;

    SystemManager sys;
    void Awake()
    {
        sys = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem;
    }
    // Start is called before the first frame update
    void Start()
    {
        roomToStone = sys._pvpbattle._pvpterrain.roomToStone;
        roomToDoor = sys._pvpbattle._pvpterrain.roomToDoor;
        doorToDoor = sys._pvpbattle._pvpterrain.doorToDoor;
        doornumToDoor = sys._pvpbattle._pvpterrain.doornumToDoor;
        doorToRoom = sys._pvpbattle._pvpterrain.doorToRoom;


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

        MakeGraph(array, h, d, playerNum, floorNum);
        List<PlayerData> PlayerList = sys._model._RoomModule.RedTeamPlayerList;
        for (int i=0;i< PlayerList.Count;i++)
        {
            if (PlayerList[i].empty) break;
            birthX = 0;
            birthY = 0;
            startRoom = 1;
            CreatePlayer(i, PlayerList[i].uid, PlayerList[i].type);
            GameObject tplayer = sys._pvpbattle._pvpplayer.playerToPlayer[PlayerList[i].uid].obj;

            

            sys._pvpbattle._pvpplayer.RedTeam.Add(PlayerList[i].uid);
            Debug.Log("MINI red added!");
            if (!sys._model._BagModule.PlayerBag.ContainsKey(PlayerList[i].uid)) continue;

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

                Debug.Log("bbbbbbb" + x.ItemID);
            }
            if (!sys._model._BagModule.PlayerHP.ContainsKey(PlayerList[i].uid)) continue;
            tplayer.GetComponent<PlayerModel_Component>().SetHealthPoint(sys._model._BagModule.PlayerHP[PlayerList[i].uid]);
        }
        PlayerList = sys._model._RoomModule.BlueTeamPlayerList;
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (PlayerList[i].empty) break;
            birthX = 4;
            birthY = 4;
            startRoom = 19;
            CreatePlayer(i, PlayerList[i].uid, PlayerList[i].type);
            GameObject tplayer = sys._pvpbattle._pvpplayer.playerToPlayer[PlayerList[i].uid].obj;
            sys._pvpbattle._pvpplayer.BlueTeam.Add(PlayerList[i].uid);

            Debug.Log("MINI blue added!");
            if (!sys._model._BagModule.PlayerBag.ContainsKey(PlayerList[i].uid)) continue;

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

                Debug.Log("bbbbbbb" + x.ItemID);
            }
            if (!sys._model._BagModule.PlayerHP.ContainsKey(PlayerList[i].uid)) continue;
            tplayer.GetComponent<PlayerModel_Component>().SetHealthPoint(sys._model._BagModule.PlayerHP[PlayerList[i].uid]);
        }
        
        
        ChangeSkillIcon();


        //地形初始化
        sys._pvpbattle._pvpterrain.roomToStone = roomToStone;
        sys._pvpbattle._pvpterrain.roomToDoor = roomToDoor;
        sys._pvpbattle._pvpterrain.doornumToDoor = doornumToDoor;
        sys._pvpbattle._pvpterrain.doorToRoom = doorToRoom;

        //AstarPath AStar = GameObject.Find("AStar").GetComponent<AstarPath>();
        //AStar.data.gridGraph.Width = (int)(GetRightTop().x - GetLeftBottom().x +1);
        //AStar.data.gridGraph.Depth = (int)(GetRightTop().y - GetLeftBottom().y + 1);

        //Debug.Log(AStar.data.gridGraph.Width);
        //Debug.Log(AStar.data.gridGraph.Depth);
        //AStar.data.gridGraph.center = new Vector3(GetRightTop().x - GetLeftBottom().x, GetRightTop().y - GetLeftBottom().y);
        //Debug.Log(AStar.data.gridGraph.center);
        //AStar.Scan();

        //初始化相机
        PlayerList = sys._model._RoomModule.PlayerList;
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (!PlayerList[i].empty && PlayerList[i].uid == sys._model._PlayerModule.uid)
            {
                Camera.main.GetComponent<CameraController>().SetTarget(sys._pvpbattle._pvpplayer.playerToPlayer[PlayerList[i].uid].obj);
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
                        sys._pvpbattle._score.AddRoom(nowRoom);
                    }
                    else if (map[i, j] == 11)  // 宝箱
                    {
                        room = Instantiate(TreasureRoom, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // Treasure
                        // terrain = Instantiate(TreasureNormalTerrain[playerNum - 1], new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // 随机地形
                    }
                    else  // 普通
                    {
                        room = Instantiate(NormalRoom, new Vector3(xOffset * j, yOffset * i, 0), Quaternion.identity);  // Normal
                        /*
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
                        */
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
                roomToStone.Add(nowRoom, stones);
                // Debug.Log(treasures.Count);


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
    public void CreatePlayer(int playerNum, int uid, CharacterType type)
    {
        FixVector2 SpwanPos = new FixVector2((Fix64)xOffset * birthY + startPosition[playerNum * 2], (Fix64)yOffset * birthX + startPosition[playerNum * 2 + 1]);
        if(sys._model._RoomModule.FindCurrentPlayerTeam()== sys._model._RoomModule.FindPlayerTeamByUID(uid))
        {
            if (GameObject.Find("Canvas").GetComponent<MeleeSmallMap>() != null)
            {
                GameObject.Find("Canvas").GetComponent<MeleeSmallMap>().ChangeRoom(startRoom, startRoom);
            }
        }
        //  创建玩家实体并根据玩家编号来决定出生位置
        switch (type)
        {
            case CharacterType.Enginner:
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
                    sys._pvpbattle._pvpplayer.playerToPlayer.Add(uid, data);
                    sys._pvpbattle._pvpplayer.playerToBirthpos.Add(uid, SpwanPos);
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
                    sys._pvpbattle._pvpplayer.playerToPlayer.Add(uid, data);
                    sys._pvpbattle._pvpplayer.playerToBirthpos.Add(uid, SpwanPos);
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
                    sys._pvpbattle._pvpplayer.playerToPlayer.Add(uid, data);
                    sys._pvpbattle._pvpplayer.playerToBirthpos.Add(uid, SpwanPos);
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
                    sys._pvpbattle._pvpplayer.playerToPlayer.Add(uid, data);
                    sys._pvpbattle._pvpplayer.playerToBirthpos.Add(uid, SpwanPos);

                    break;
                }
            default:
                break;
        }
    }
    public void ChangeSkillIcon()
    {
        int PlayerUID = sys._model._PlayerModule.uid;
        CharacterType PlayerType = sys._model._RoomModule.GetCharacterType(PlayerUID);

        GameObject js1 = GameObject.Find("SkillStickUI1"), js2 = GameObject.Find("SkillStickUI2"), js3 = GameObject.Find("SkillStickUI3");

        switch (PlayerType)
        {
            case CharacterType.Enginner:
                {
                    GameObject PlayerObject = sys._pvpbattle._pvpplayer.FindPlayerObjByUID(PlayerUID);
                    js1.GetComponent<Image>().sprite = sys._battle._skill.enginerBase.skill1Image;
                    js1.GetComponent<SkillIndiactor>().Init(sys._battle._skill.enginerBase.Skill1Range(),
                                                sys._battle._skill.enginerBase.Skill1Area(), PlayerObject, sys._battle._skill.enginerBase.skill1Type);
                    js2.GetComponent<Image>().sprite = sys._battle._skill.enginerBase.skill2Image;

                    js2.GetComponent<SkillIndiactor>().Init(sys._battle._skill.enginerBase.Skill2Range(),
                                                sys._battle._skill.enginerBase.Skill2Area(), PlayerObject, sys._battle._skill.enginerBase.skill1Type);


                    js3.GetComponent<Image>().sprite = sys._battle._skill.enginerBase.skill3Image;
                    js3.GetComponent<SkillIndiactor>().Init(sys._battle._skill.enginerBase.Skill3Range(),
                                                sys._battle._skill.enginerBase.Skill3Area(), PlayerObject, sys._battle._skill.enginerBase.skill3Type);

                    break;
                }
            case CharacterType.Magician:
                {
                    GameObject PlayerObject = sys._pvpbattle._pvpplayer.FindPlayerObjByUID(PlayerUID);
                    js1.GetComponent<Image>().sprite = sys._battle._skill.magicianBase.skill1Image;
                    js1.GetComponent<SkillIndiactor>().Init(sys._battle._skill.magicianBase.Skill1Range(),
                                                sys._battle._skill.magicianBase.Skill1Area(), PlayerObject, sys._battle._skill.enginerBase.skill1Type);
                    js2.GetComponent<Image>().sprite = sys._battle._skill.magicianBase.skill2Image;
                    js2.GetComponent<SkillIndiactor>().Init(sys._battle._skill.magicianBase.Skill2Range(),
                                                sys._battle._skill.magicianBase.Skill2Area(), PlayerObject, sys._battle._skill.enginerBase.skill1Type);
                    js3.GetComponent<Image>().sprite = sys._battle._skill.magicianBase.skill3Image;
                    js3.GetComponent<SkillIndiactor>().Init(sys._battle._skill.magicianBase.Skill3Range(),
                                                sys._battle._skill.magicianBase.Skill3Area(), PlayerObject, sys._battle._skill.magicianBase.skill3Type);

                    break;
                }
            case CharacterType.Warrior:
                {
                    GameObject PlayerObject = sys._pvpbattle._pvpplayer.FindPlayerObjByUID(PlayerUID);
                    js1.GetComponent<Image>().sprite = sys._battle._skill.guardianBase.skill1Image;
                    js1.GetComponent<SkillIndiactor>().Init(sys._battle._skill.guardianBase.Skill1Range(),
                                                sys._battle._skill.guardianBase.Skill1Area(), PlayerObject, sys._battle._skill.guardianBase.skill1Type);
                    js2.GetComponent<Image>().sprite = sys._battle._skill.guardianBase.skill2Image;
                    js2.GetComponent<SkillIndiactor>().Init(sys._battle._skill.guardianBase.Skill2Range(),
                                                sys._battle._skill.guardianBase.Skill2Area(), PlayerObject, sys._battle._skill.guardianBase.skill2Type);
                    break;
                }
            case CharacterType.Ghost:
                {
                    GameObject PlayerObject = sys._pvpbattle._pvpplayer.FindPlayerObjByUID(PlayerUID);
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
}

