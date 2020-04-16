using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BattleManager
{
    int local_frame;

    SystemManager sys;

    MonsterModule _monster;
    PlayerDataModule _player;
    SkillModule _skill;
    TerrainModule _terrain;


    public BattleManager()
    {
        local_frame = 0;
    }
    public BattleManager(SystemManager system)
    {
        local_frame = 0;
        sys = system;

        _monster = new MonsterModule(this);
        _player = new PlayerDataModule(this);
        _skill = new SkillModule(this);
        _terrain = new TerrainModule(this);
    }
    void UpdateFrame()
    {
        UpdateLogicByFrame();
        UpdateView();
    }
    void UpdateLogicByFrame()
    {

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