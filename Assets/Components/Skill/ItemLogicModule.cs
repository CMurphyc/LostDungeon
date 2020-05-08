using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemLogicModule
{
    BattleManager _parentManager;

    public ItemLogicModule(BattleManager parent)
    {
        _parentManager = parent;
    }

    //时停3s
    private void SkySin()
    {

    }
    //无敌2.5s
    // public void Invincible(GameObject obj)
    // {
    //     GameObject wudiCircle = Resources.Load("UI/wudiCircle") as GameObject;
    //     GameObject circle = GameObject.Instantiate(wudiCircle);
    //     circle.transform.parent = obj.transform;
    //     circle.transform.position = obj.transform.position;
        
    //     Object.Destroy(wudiCircle, 2.5f);

    //     // obj.GetComponent<PlayerModel_Component>().SetMuteki((int)(2.5f * 1000) / Global.FrameRate);
    // }

    //又粗又长的激光射出去
    public void JOJOStand(PlayerInGameData player, FixVector2 toward)
    {
        GameObject sd = Resources.Load("Assets/Resources/Model/Player/Prefab/Stand") as GameObject;
        GameObject stand = GameObject.Instantiate(sd);
        
        stand.transform.parent = player.obj.transform;
        stand.transform.position = new Vector2(player.obj.transform.position.x - 163, player.obj.transform.position.y + 74);

        GameObject buster = Resources.Load("Assets/Resources/Model/Bullet/Prefab/buster") as GameObject;

        List<int> list = new List<int>();
                                    BulletUnion bu = new BulletUnion(_parentManager);
                                    bu.BulletInit("Player", new FixVector2((Fix64)stand.transform.position.x,
                                                                        (Fix64)stand.transform.position.y),
                                                                        toward,
                                                                        (Fix64)15, (Fix64)2, player.RoomID,
                                                                        buster,
                                                                        list);
        _parentManager._player.bulletList.Add(bu);
        Object.Destroy(stand, 1.5f);
    }
}
