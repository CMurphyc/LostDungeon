using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIUpdate : MonoBehaviour
{
    public Sprite boss2;
    public Sprite boss3;


    SystemManager sys;
    public GameObject BossUI;
    Text MonsterNum;
    bl_ProgressBar BossHP;
    int CurrnetUID;
    GameObject HP_bar;

    Dictionary<int, GameObject> Teammate=new Dictionary<int, GameObject>();

    float counter = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        sys = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem;

        CurrnetUID = sys._battle._player.FindCurrentPlayerUID();
        BossUI = GameObject.Find("Canvas/BossHint");

        HP_bar =  GameObject.Find("Canvas/PlayerHPUI");
        if (BossUI!=null)
        BossUI.SetActive(false);
        counter = 0;


        if (GameObject.Find("Canvas/Floor/floornum") != null)
        {
            Text FloorNum = GameObject.Find("Canvas/Floor/floornum").GetComponent<Text>();
            FloorNum.text = sys._model._RoomModule.MapFloorNumber.ToString();
        }
        GameObject TeammateHP = Resources.Load("UI/UIPrefabs/TeammateHPUI", typeof(GameObject)) as GameObject;
        List<PlayerData> playerlist = sys._model._RoomModule.PlayerList;
        for (int i=0,j=0;i< playerlist.Count;i++)
        {
            if(playerlist[i].empty==false&&playerlist[i].uid!= sys._model._PlayerModule.uid)
            {
                if(sys._model._RoomListModule.roomType==RoomType.Pvp)
                {
                    Debug.Log("CurrentTeam: "+sys._pvpbattle._pvpplayer.FindCurrentPlayerTeam());
                    Debug.Log("PlayerTeam: " + sys._pvpbattle._pvpplayer.FindPlayerTeamByUID(playerlist[i].uid));
                    if(sys._model._RoomModule.FindCurrentPlayerTeam()!= sys._model._RoomModule.FindPlayerTeamByUID(playerlist[i].uid))
                    {
                        continue;
                    }
                }

                GameObject item=Object.Instantiate(TeammateHP,GameObject.Find("Canvas").transform);
                item.GetComponent<RectTransform>().anchoredPosition =new Vector2(30,-100)+j*new Vector2(0,-50);
                item.transform.Find("name").GetComponent<Text>().text = playerlist[i].username;
                item.transform.Find("Slider").GetComponent<Slider>().value = item.transform.Find("Slider").GetComponent<Slider>().maxValue;
                switch (playerlist[i].type)
                {
                    case CharacterType.Enginner:
                        item.transform.Find("Image").GetComponent<Image>().sprite =
                        Instantiate(Resources.Load("Model/Player/Sprites/Engineer/c06_s1_4", typeof(Sprite))) as Sprite;
                        break;
                    case CharacterType.Magician:
                        item.transform.Find("Image").GetComponent<Image>().sprite =
                        Instantiate(Resources.Load("Model/Player/Sprites/Magician/c03_4", typeof(Sprite))) as Sprite;
                        break;
                    case CharacterType.Warrior:
                        item.transform.Find("Image").GetComponent<Image>().sprite =
                        Instantiate(Resources.Load("Model/Player/Sprites/Guardian/c08_s2_4", typeof(Sprite))) as Sprite;
                        break;
                    case CharacterType.Ghost:
                        item.transform.Find("Image").GetComponent<Image>().sprite =
                        Instantiate(Resources.Load("Model/Player/Sprites/Ghost/c02_s6_12", typeof(Sprite))) as Sprite;
                        break;
                }
                Teammate.Add(playerlist[i].uid,item);
                j++;
            }
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        if (counter >= 0.5f)
        {
            switch (sys._model._RoomListModule.roomType)
            {
                case RoomType.Pve:
                    UpdatePVE();
                    break;
                case RoomType.Pvp:
                    UpdatePVP();
                    break;
            }
            counter = 0;
        }
    }
    void UpdatePVE()
    {
        PlayerInGameData data = sys._battle._player.playerToPlayer[CurrnetUID];
        HP_bar.transform.Find("HP/Text").gameObject.GetComponent<Text>().text = data.obj.GetComponent<PlayerModel_Component>().GetHealthPoint().ToString() + "/" + data.obj.GetComponent<PlayerModel_Component>().GetFullHealthPoint().ToString();
        HP_bar.transform.Find("HP").gameObject.GetComponent<Slider>().value = (float)data.obj.GetComponent<PlayerModel_Component>().GetHealthPoint() / (float)data.obj.GetComponent<PlayerModel_Component>().GetFullHealthPoint();
        List<int> RoomIDList = new List<int>();
        foreach (var i in sys._model._RoomModule.PlayerList)
        {
            if (i.empty) break;
            RoomIDList.Add(sys._battle._player.playerToPlayer[i.uid].RoomID);
        }
        if (GameObject.Find("Canvas").GetComponent<SmallMap>() != null)
        {
            GameObject.Find("Canvas").GetComponent<SmallMap>().ChangeRoom(RoomIDList);
        }
        foreach (var i in Teammate)
        {
            GameObject pler = sys._battle._player.FindPlayerObjByUID(i.Key);
            i.Value.transform.Find("Slider").GetComponent<Slider>().value =
                pler.GetComponent<PlayerModel_Component>().healthPoint * 1.0f / pler.GetComponent<PlayerModel_Component>().fullHealthPoint;
        }

        //Boss UI 

        if (BossUI != null)
        {
            //if (!BossUIInited)
            {
                switch (sys._model._RoomModule.MapFloorNumber)
                {
                    case 1:
                        {

                            break;
                        }

                    case 2:
                        {
                            BossUI.transform.Find("head_frame/head_img").gameObject.GetComponent<Image>().sprite =
                            boss2;
                            BossUI.transform.Find("BossName").gameObject.GetComponent<Text>().text =
                           "Dark Wizard";

                            break;
                        }
                    case 3:
                        {
                            BossUI.transform.Find("head_frame/head_img").gameObject.GetComponent<Image>().sprite =
                             boss3;
                            BossUI.transform.Find("BossName").gameObject.GetComponent<Text>().text =
                          "Black Knight";
                            break;
                        }
                }


            }
            //hp 数量


            int CurrnetUID = sys._battle._player.FindCurrentPlayerUID();
            int CurrnetRoomID = sys._battle._player.playerToPlayer[CurrnetUID].RoomID;
            if (CurrnetRoomID == sys._battle._monster.BossRoom)
            {
                List<GameObject> ListObj = sys._battle._monster.RoomToMonster[CurrnetRoomID];
                bool bossFind = false;
                for (int i = 0; i < ListObj.Count; i++)
                {
                    if (ListObj[i].tag == "Boss")
                    {
                        bossFind = true;
                        if (!BossUI.activeSelf)
                            BossUI.SetActive(true);
                        bl_ProgressBar BossHP = GameObject.Find("Canvas/BossHint/HP/Mask/Slider").GetComponent<bl_ProgressBar>();
                        BossHP.MaxValue = (float)ListObj[i].GetComponent<MonsterModel_Component>().MaxHP;
                        BossHP.Value = (float)ListObj[i].GetComponent<MonsterModel_Component>().HP;

                        break;
                    }
                }
                if (!bossFind)
                {
                    BossUI.SetActive(false);
                }
            }
            Text MonsterNum = GameObject.Find("Canvas/MonsterLeft/monsternum").GetComponent<Text>();
            MonsterNum.text = sys._battle._monster.RoomToMonster[CurrnetRoomID].Count.ToString();

        }

    }
    void UpdatePVP()
    {
        if (sys._pvpbattle._pvpplayer.FindCurrentPlayerTeam() == "RedTeam")
        {
            GameObject.Find("Canvas/Image/RedText").GetComponent<Text>().text = "我方";
            GameObject.Find("Canvas/Image/BlueText").GetComponent<Text>().text = "敌方";
        }
        else
        {
            GameObject.Find("Canvas/Image/RedText").GetComponent<Text>().text = "敌方";
            GameObject.Find("Canvas/Image/BlueText").GetComponent<Text>().text = "我方";
        }
        List<int> RoomIDList = new List<int>();
        if(sys._model._RoomModule.FindCurrentPlayerTeam()== "BlueTeam")
        {
            foreach(var i in sys._model._RoomModule.BlueTeamPlayerList)
            {
                if (i.empty) break;
                RoomIDList.Add(sys._pvpbattle._pvpplayer.playerToPlayer[i.uid].RoomID);
            }
        }
        if(sys._model._RoomModule.FindCurrentPlayerTeam() =="RedTeam")
        {
            foreach (var i in sys._model._RoomModule.RedTeamPlayerList)
            {
                if (i.empty) break;
                RoomIDList.Add(sys._pvpbattle._pvpplayer.playerToPlayer[i.uid].RoomID);
            }
        }
        if (GameObject.Find("Canvas").GetComponent<MeleeSmallMap>() != null )
        {
            GameObject.Find("Canvas").GetComponent<MeleeSmallMap>().ChangeRoom(RoomIDList);
        }
        PlayerInGameData data = sys._pvpbattle._pvpplayer.playerToPlayer[CurrnetUID];
        HP_bar.transform.Find("HP/Text").gameObject.GetComponent<Text>().text = data.obj.GetComponent<PlayerModel_Component>().GetHealthPoint().ToString() + "/" + data.obj.GetComponent<PlayerModel_Component>().GetFullHealthPoint().ToString();
        HP_bar.transform.Find("HP").gameObject.GetComponent<Slider>().value = (float)data.obj.GetComponent<PlayerModel_Component>().GetHealthPoint() / (float)data.obj.GetComponent<PlayerModel_Component>().GetFullHealthPoint();
        foreach (var i in Teammate)
        {
            GameObject pler = sys._pvpbattle._pvpplayer.FindPlayerObjByUID(i.Key);
            i.Value.transform.Find("Slider").GetComponent<Slider>().value =
                pler.GetComponent<PlayerModel_Component>().healthPoint * 1.0f / pler.GetComponent<PlayerModel_Component>().fullHealthPoint;
        }
    }
}
