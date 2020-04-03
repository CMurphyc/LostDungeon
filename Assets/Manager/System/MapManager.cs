using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections.Generic;

public class MapManager : SceneManager
{
    List<string> SceneList;
    string scene_dir = "Scenes/";

    int SceneIndex = 0;
    SystemManager sys;
    public MapManager()
    {
        SceneList = new List<string>();
        SceneList.Add("Scenes/LoginPanel");
        SceneList.Add("Scenes/Main");
        SceneList.Add("Scenes/TeamUpUI");
        SceneList.Add("Scenes/Battle");
        SceneList.Add("Scenes/RoomList");
    }
    public MapManager(SystemManager system)
    {
        sys = system;
        SceneList = new List<string>();
        SceneList.Add("Scenes/LoginPanel");
        SceneList.Add("Scenes/Main");
        SceneList.Add("Scenes/TeamUpUI");
        SceneList.Add("Scenes/Battle");
        SceneList.Add("Scenes/RoomList");
    }

    public void SwitchScene(string targetScene)
    {
        if (SceneList.Contains(scene_dir+targetScene))
        {
            SceneManager.LoadScene(scene_dir + targetScene);
            SceneIndex = SceneList.IndexOf(scene_dir + targetScene);
         }
    }

    public int GetCurrentIndex()
    {

        return SceneIndex;
    }

    //初始化战场地图信息
    public void InitBattleScene()
    {
        


    }






}