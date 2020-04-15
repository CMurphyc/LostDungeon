using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterModule 
{
    BattleManager _parentManager;

    //房间号-怪物列表
    public Dictionary<int, List<GameObject>> RoomToMonster;

    public MonsterModule(BattleManager parent)
    {
        _parentManager = parent;
    }

    public void UpdateLogic(int frame)
    {



    }

    public void UpdateView()
    {



    }
}
