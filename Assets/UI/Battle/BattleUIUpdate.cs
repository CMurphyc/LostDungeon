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
    // Start is called before the first frame update
    private void Awake()
    {
        sys = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem;

        CurrnetUID = sys._battle._player.FindCurrentPlayerUID();
        BossUI = GameObject.Find("Canvas/BossHint");

        HP_bar =  GameObject.Find("Canvas/PlayerHPUI");



        BossUI.SetActive(false);
    }
    void Start()
    {

        Text FloorNum = GameObject.Find("Canvas/Floor/floornum").GetComponent<Text>();
        FloorNum.text = sys._model._RoomModule.MapFloorNumber.ToString();

        GameObject TeammateHP = Resources.Load("UI/UIPrefabs/TeammateHPUI", typeof(GameObject)) as GameObject;
        List<PlayerData> playerlist = sys._model._RoomModule.PlayerList;
        for (int i=0,j=0;i< playerlist.Count;i++)
        {
            if(playerlist[i].empty==false&&playerlist[i].uid!= sys._battle._player.FindCurrentPlayerUID())
            {
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
                }
                Teammate.Add(playerlist[i].uid,item);
                j++;
            }
        }



    }

    // Update is called once per frame
    void Update()
    {
        if (sys._battle._player.playerToPlayer.ContainsKey(CurrnetUID))
        {
            PlayerInGameData data = sys._battle._player.playerToPlayer[CurrnetUID];

            HP_bar.transform.Find("HP/Text").gameObject.GetComponent<Text>().text = data.obj.GetComponent<PlayerModel_Component>().GetHealthPoint().ToString() + "/"+ data.obj.GetComponent<PlayerModel_Component>().GetFullHealthPoint().ToString();

            HP_bar.transform.Find("HP").gameObject.GetComponent<Slider>().value = (float)data.obj.GetComponent<PlayerModel_Component>().GetHealthPoint() / (float)data.obj.GetComponent<PlayerModel_Component>().GetFullHealthPoint();

        }
        foreach(var i in Teammate)
        {
            GameObject pler=sys._battle._player.FindPlayerObjByUID(i.Key);
            i.Value.transform.Find("Slider").GetComponent<Slider>().value =
                pler.GetComponent<PlayerModel_Component>().healthPoint*1.0f / pler.GetComponent<PlayerModel_Component>().fullHealthPoint;
        }
    }
}
