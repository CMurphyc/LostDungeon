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
    // Start is called before the first frame update
    private void Awake()
    {
        sys = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem;

        CurrnetUID = sys._battle._player.FindCurrentPlayerUID();
        BossUI = GameObject.Find("Canvas/BossHint");

        BossUI.SetActive(false);
    }
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
