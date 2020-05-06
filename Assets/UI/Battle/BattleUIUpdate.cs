using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIUpdate : MonoBehaviour
{
    SystemManager sys;
    public GameObject BossUI;
    Text MonsterNum;
    bl_ProgressBar BossHP;
    int CurrnetUID;
    GameObject HP_bar;
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
      
  


    }
}
