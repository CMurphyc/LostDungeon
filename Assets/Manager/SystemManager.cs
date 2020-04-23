using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SystemManager
{
    public AudioManager _audio;
    public BattleManager _battle;
    public MapManager _map;
    public ModelManager _model;
    public AnimationManager _animation;
    public SystemManager()
    {
        _battle = new BattleManager(this);
        _audio = new AudioManager();
        _map = new MapManager(this);
        _model = new ModelManager();
        _animation = new AnimationManager(this);
    }
    
    //void Destory()
    //{
    //    _battle.de

    //}
}



