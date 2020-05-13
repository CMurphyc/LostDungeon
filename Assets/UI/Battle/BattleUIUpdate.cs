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
    }
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        if (counter >= 1f)
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
