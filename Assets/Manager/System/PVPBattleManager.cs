using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPBattleManager 
{
    int local_frame;

    public SystemManager sys;

    public PVPPlayerDataModule _pvpplayer;
    public SummonModule _summon;
    public PVPChestModule _chest;
    public PVPSkillModule _pvpskill;
    public PVPTerrainModule _pvpterrain;
    public TextJumpModule _textjump;
    public ScoreModule _score;

    public int Seed=114514;
    public int SeverFrame;

    public PVPBattleManager(SystemManager system)
    {
        local_frame = 0;
        sys = system;

        _pvpplayer = new PVPPlayerDataModule(this);
        _summon = new SummonModule(this);
        _pvpskill = new PVPSkillModule(this);
        _pvpterrain = new PVPTerrainModule(this);
        _textjump = new TextJumpModule();
        _score = new ScoreModule(this);
        _chest = new PVPChestModule(this);

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
        _pvpplayer.UpdateLogic(local_frame);
        _summon.UpdateLogic(local_frame);
        _pvpskill.UpdateLogic(local_frame);
        _pvpterrain.UpdateLogic(local_frame);
        _score.UpdateLogic(local_frame);
        _chest.UpdateLogic(local_frame);
        local_frame++;
    }

    void UpdateView()
    {
        _pvpplayer.UpdateView();
        _summon.UpdateView();
        _pvpskill.UpdateView();
        _pvpterrain.UpdateView();
        _textjump.UpdateView();
        _score.UpdateView();
        _chest.UpdateView();
    }

    public void ReleaseMemory()
    {
        _pvpplayer.Free();
        _summon.Free();
        _pvpskill.Free();
        _pvpterrain.Free();
        _textjump.Free();
        _score.Free();
        _chest.Free();
    }
}
