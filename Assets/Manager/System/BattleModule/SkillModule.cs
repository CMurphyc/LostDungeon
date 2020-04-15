using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillModule 
{
    BattleManager _parentManager;

    //房间号-抛射物
    public Dictionary<int, List<GameObject>> RoomToProjectile;

    public SkillModule(BattleManager parent)
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
