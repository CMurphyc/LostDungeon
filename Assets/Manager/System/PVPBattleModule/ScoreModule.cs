using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreModule
{

    PVPBattleManager _pvp;
    string winer;
    //  红方分数     蓝方分数
    public int RedTeamScore, BlueTeamScore;
    //  游戏剩余时间
    int GameTime;
    int SliderMaxValue = 800;
    //据点房间号-红方所占比例
    List<KeyValuePair<int, int>> StrongHold;
    //据点房间号-被某一方完全占领的时间
    List<int> HoldTime;
    public ScoreModule(PVPBattleManager pvp)
    {
        winer = "";
        _pvp = pvp;
        RedTeamScore = 0;
        BlueTeamScore = 0;
        GameTime = 300*Global.FrameRate;
        StrongHold = new List<KeyValuePair<int, int>>();
        HoldTime = new List<int>();
    }
    public void Free()
    {
        winer = "";
        RedTeamScore = BlueTeamScore = 0;
        GameTime= 300 * Global.FrameRate;
        StrongHold.Clear();
        HoldTime.Clear();
    }
    public void UpdateLogic(int frame)
    {
        GameTime--;
        CheckGameEnd();
        //更新分数及据点占领时间
        for(int i=0;i<StrongHold.Count;i++)
        {
            if(StrongHold[i].Value!=0&& StrongHold[i].Value!= SliderMaxValue)
            {
                HoldTime[i] = 0;
                continue;
            }
            if (HoldTime[i] >= 120)
            {
                if (StrongHold[i].Value == SliderMaxValue) RedTeamScore += 1;
                else BlueTeamScore += 1;
                HoldTime[i] = 0;
            }
            else
            {
                HoldTime[i]++;
            }
        }
        //更新据点抢占情况
        for(int i=0;i<StrongHold.Count;i++)
        {
            if (_pvp._pvpplayer.FindRedTeamPlayerInRoomID(StrongHold[i].Key) > _pvp._pvpplayer.FindBlueTeamPlayerInRoomID(StrongHold[i].Key))
            {
                StrongHold[i] =new KeyValuePair<int, int>(StrongHold[i].Key, Mathf.Min(StrongHold[i].Value + 1, SliderMaxValue));
            }
            if (_pvp._pvpplayer.FindRedTeamPlayerInRoomID(StrongHold[i].Key) < _pvp._pvpplayer.FindBlueTeamPlayerInRoomID(StrongHold[i].Key))
            {
                StrongHold[i] = new KeyValuePair<int, int>(StrongHold[i].Key, Mathf.Max(StrongHold[i].Value -1, 0));
            }
        }
    }

    public void UpdateView()
    {
        int LeftTime =Mathf.Max(0,GameTime / 40);
        GameObject Canvas = GameObject.Find("Canvas");
        Canvas.transform.Find("Image/Time").GetComponent<Text>().text = (LeftTime / 3600).ToString("d2") + ":" + ((LeftTime % 3600) / 60).ToString("d2") + ":" + (LeftTime % 60).ToString("d2");
        Canvas.transform.Find("Image/RedScore").GetComponent<Text>().text = RedTeamScore.ToString();
        Canvas.transform.Find("Image/BlueScore").GetComponent<Text>().text = BlueTeamScore.ToString();
        UpdateScoreSlider(Canvas);
    }
    void UpdateScoreSlider(GameObject Canvas)
    {
        int CurrentPlayerRoomID = _pvp._pvpplayer.FindRoomIDCurrentPlayerIn();
        foreach (var i in StrongHold)
        {
            if (i.Key == CurrentPlayerRoomID)
            {
                Canvas.transform.Find("RedSlider").GetComponent<Slider>().value = i.Value;
                Canvas.transform.Find("BlueSlider").GetComponent<Slider>().value = SliderMaxValue - i.Value;
                Canvas.transform.Find("RedSlider").gameObject.SetActive(true);
                Canvas.transform.Find("BlueSlider").gameObject.SetActive(true);
                return;
            }
        }
        Canvas.transform.Find("RedSlider").gameObject.SetActive(false);
        Canvas.transform.Find("BlueSlider").gameObject.SetActive(false);
    }
    void CheckGameEnd()
    {
        if (RedTeamScore >= 100)
        {
            winer = "RedTeam";
            GameObject.Find("GameEntry").GetComponent<GameMain>().socket.sock_c2s.GameOver();
        }
        if(BlueTeamScore>=100)
        {
            winer = "BlueTeam";
            GameObject.Find("GameEntry").GetComponent<GameMain>().socket.sock_c2s.GameOver();
        }
        if(GameTime<=0)
        {
            if(RedTeamScore>BlueTeamScore)
            {
                winer = "RedTeam";
                GameObject.Find("GameEntry").GetComponent<GameMain>().socket.sock_c2s.GameOver();
            }
            if(BlueTeamScore>RedTeamScore)
            {
                winer = "BlueTeam";
                GameObject.Find("GameEntry").GetComponent<GameMain>().socket.sock_c2s.GameOver();
            }
        }
    }
    /// <summary>
    /// 添加据点房间
    /// </summary>
    /// <param name="RoomID">据点房间号</param>
    public void AddRoom(int RoomID)
    {
        StrongHold.Add(new KeyValuePair<int, int>(RoomID, SliderMaxValue/2));
        HoldTime.Add(0);
    }
    /// <summary>
    /// 蓝方加分
    /// </summary>
    /// <param name="num"></param>
    public void AddBlueTeamScore(int num)
    {
        BlueTeamScore += num;
    }
    /// <summary>
    /// 红方加分
    /// </summary>
    /// <param name="num"></param>
    public void AddRedTeamScore(int num)
    {
        RedTeamScore += num;
    }
    public void AddTeamScoreByPlayerUID(int uid)
    {
        if(_pvp._pvpplayer.FindPlayerTeamByUID(uid)=="BlueTeam")
        {
            AddBlueTeamScore(1);
        }
        else
        {
            AddRedTeamScore(1);
        }
    }
    public string GetWinner() { return winer; }
}
