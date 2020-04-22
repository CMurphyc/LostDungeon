using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIEvent : MonoBehaviour
{
    BattleManager battle;
    // Start is called before the first frame update
    void Start()
    {
        battle = GameObject.FindWithTag("GameEntry").GetComponent<GameMain>().WorldSystem._battle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBtnInitTerretTower()
    {
      
        int UID = battle._player.FindCurrentPlayerUID();
        if  (battle._player.playerToPlayer.ContainsKey(UID))
        {

            int RoomID = battle._player.playerToPlayer[UID].RoomID;
         
            


            GameObject TerretPrefab = (GameObject)Resources.Load("Model/dddppp/Monster/Prefab/terrettower");

            FixVector2 PlayerPos = battle._player.playerToPlayer[UID].obj.GetComponent<PlayerModel_Component>().GetPlayerPosition();

            FixVector3 PlayerPos3 = new FixVector3(PlayerPos.x, PlayerPos.y, (Fix64)0);
            
            Vector3 PlayerPos2 = new Vector3((float)PlayerPos.x, (float)PlayerPos.y);
            //Debug.Log(PlayerPos2);
            GameObject TerretInstance = Instantiate(TerretPrefab, PlayerPos2, Quaternion.identity);
            TerretInstance.GetComponent<MonsterModel_Component>().position = PlayerPos3;
          
            AliasMonsterPack temp = new AliasMonsterPack();
            BossAttribute attribute = new BossAttribute();
            attribute.Attack_FrameInterval = 20;
            attribute.SpinRate = 3;
            TerretInstance.GetComponent<EnemyAI>().InitAI(AI_Type.Engineer_TerretTower, RoomID, attribute);
            TerretInstance.GetComponent<MonsterModel_Component>().HP = (Fix64)10;
            temp.obj = TerretInstance;
            temp.RemainingFrame = 200;

            if (!battle._monster.RoomToAliasUnit.ContainsKey(RoomID))
            {
                List<AliasMonsterPack> ListAlias = new List<AliasMonsterPack>();
                ListAlias.Add(temp);
                battle._monster.RoomToAliasUnit.Add(RoomID, ListAlias);
            }
            else
            {
                battle._monster.RoomToAliasUnit[RoomID].Add(temp);

            }
          

        }

    }
}
