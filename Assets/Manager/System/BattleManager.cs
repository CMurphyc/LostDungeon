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
    public ChestModule _chest;
    public ItemLogicModule _itemlogic;
    public TextJumpModule _textjump;

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
        _chest = new ChestModule(this);
        _textjump = new TextJumpModule();
        _itemlogic = new ItemLogicModule(this);

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
        _player.UpdateLogic(local_frame);
        _monster.UpdateLogic(local_frame);
        _skill.UpdateLogic(local_frame);
        _terrain.UpdateLogic(local_frame);
        _chest.UpdateLogic(local_frame) ;
        local_frame++;
    }

    void UpdateView()
    {
        _monster.UpdateView();
        _player.UpdateView();
        _skill.UpdateView();
        _terrain.UpdateView();
        _chest.UpdateView();
        _textjump.UpdateView();
    }

    public void ReleaseMemory()
    {
        _monster.Free();
        _player.Free();
        _skill.Free();
        _terrain.Free();
        _chest.Free();
        _textjump.Free();
    }

}