using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIUpdate : MonoBehaviour
{
    SystemManager sys;
    GameObject BossUI;
    Text MonsterNum;
    bl_ProgressBar BossHP;
    int CurrnetUID;
    // Start is called before the first frame update
    private void Awake()
    {
        sys = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem;

        CurrnetUID = sys._battle._player.FindCurrentPlayerUID();

    }
    void Start()
    {
        BossUI = GameObject.Find("Canvas/BossHint");
        
        BossUI.SetActive(false);
        MonsterNum = GameObject.Find("Canvas/MonsterLeft/monsternum").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
       int BossRoom = sys._battle._monster.BossRoom;
        int CurrnetRoomID = sys._battle._player.playerToPlayer[CurrnetUID].RoomID;
       if (CurrnetRoomID == BossRoom)
        {
            if (!BossUI.activeSelf)
            {
                BossUI.SetActive(true);
            }
            else
            {
                List<GameObject> ListObj = sys._battle._monster.RoomToMonster[BossRoom];
                for (int i = 0; i < ListObj.Count;i++)
                {
                    if (ListObj[i].tag == "Boss")
                    {
                        BossHP = GameObject.Find("Canvas/BossHint/HP/Mask/Slider").GetComponent<bl_ProgressBar>();
                        BossHP.MaxValue = (float)ListObj[i].GetComponent<MonsterModel_Component>().MaxHP;
                        BossHP.Value = (float)ListObj[i].GetComponent<MonsterModel_Component>().HP ;
                        break;
                    }
                }
            }
        }
        
        MonsterNum.text = sys._battle._monster.RoomToMonster[CurrnetRoomID].Count.ToString();
    }
}
