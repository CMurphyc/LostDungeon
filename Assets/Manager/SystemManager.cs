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

    public MessageManager _message;
    public SystemManager()
    {
        _battle = new BattleManager(this);
        _audio = new AudioManager();
        _map = new MapManager(this);
        _model = new ModelManager();
        _animation = new AnimationManager(this);
        _message = new MessageManager();
    }
    
    public void ResetBattle()
    {
        //to do 释放
        _battle.ReleaseMemory();
        _model._JoyStickModule.Free();
    }
    //void Destory()
    //{
    //    _battle.de

    //}
}



