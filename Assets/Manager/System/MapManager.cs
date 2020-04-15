using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

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
        SceneList.Add("Scenes/HeroSelect");
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
        SceneList.Add("Scenes/HeroSelect");
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



    public void RefreshRoomInfo()
    {
        Debug.Log("Refresh UI .......");

        for (int i = 0; i < sys._model._RoomModule.PlayerList.Count; i++)
        {
            GameObject Character = GameObject.Find("Canvas/Player" + (i + 1).ToString() + "/Character");

            if (sys._model._RoomModule.PlayerList[i].type == CharacterType.Enginner)
            {
                Character.GetComponent<Text>().text = "工程师";
            }
            else if (sys._model._RoomModule.PlayerList[i].type == CharacterType.Warrior)
            {
                Character.GetComponent<Text>().text = "圣骑士";
            }
            else if (sys._model._RoomModule.PlayerList[i].type == CharacterType.Magician)
            {
                Character.GetComponent<Text>().text = "元素使";
            }

            GameObject username = GameObject.Find("Canvas/Player" + (i + 1).ToString() + "/name");

            username.GetComponent<Text>().text = sys._model._RoomModule.PlayerList[i].uid.ToString();

            GameObject status = GameObject.Find("Canvas/Player" + (i + 1).ToString() + "/status");
            if  (sys._model._RoomModule.PlayerList[i].ready)
            {
                status.GetComponent<Text>().text = "已准备";
            }
            else
            {
                status.GetComponent<Text>().text = "未准备";
            }
            if (sys._model._RoomModule.PlayerList[i].uid == sys._model._RoomModule.roomOwnerID)
            {
                status.GetComponent<Text>().text = "";
            }

        }

 

    }






}