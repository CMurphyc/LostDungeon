using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BattleManager
{
    int local_frame;

    public SystemManager sys;

    public MonsterModule _monster;
    public PlayerDataModule _player;
    public SkillModule _skill;
    public TerrainModule _terrain;

    public int Seed;
    public int SeverFrame;

    public BattleManager(SystemManager system)
    {
        local_frame = 0;
        sys = system;

        _monster = new MonsterModule(this);
        _player = new PlayerDataModule(this);
        _skill = new SkillModule(this);
        _terrain = new TerrainModule(this);
    }


    public void UpdateFrame()
    {
        UpdateLogicByFrame();
        UpdateView();
    }
    void UpdateLogicByFrame()
    {
        //初始化随机种子
        UnityEngine.Random.InitState(Seed);
        //战斗主逻辑
        _monster.UpdateLogic(local_frame);
        _player.UpdateLogic(local_frame);
        _skill.UpdateLogic(local_frame);
        _terrain.UpdateLogic(local_frame);
        local_frame++;



    }

    void UpdateView()
    {
        _monster.UpdateView();
        _player.UpdateView();
        _skill.UpdateView();
        _terrain.UpdateView();
    }

}