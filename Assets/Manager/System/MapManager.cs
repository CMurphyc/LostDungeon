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

    string CurrentScene  = "LoginPanel";

    SystemManager sys;

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
        SceneList.Add("Scenes/MapCreate");
    }

    public void SwitchScene(string targetScene)
    {
        if (CurrentScene != targetScene)
        {
            SceneManager.LoadScene(scene_dir + targetScene);
            CurrentScene = targetScene;
            Debug.Log("ChangeScene: " + CurrentScene);
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
            string Path = "Canvas/player" + (i + 1).ToString() + "/Character";
            //Debug.Log("Path: "+Path);
            GameObject Character = GameObject.Find(Path);

            if (!sys._model._RoomModule.PlayerList[i].empty)
            {
                GameObject Animation_Prefab = null;

                if (sys._model._RoomModule.PlayerList[i].type == CharacterType.Enginner)
                {
                    Animation_Prefab = (GameObject)Resources.Load("Model/Player/Prefab/Engineer");
                    Character.GetComponent<Text>().text = "工程师";
                }
                else if (sys._model._RoomModule.PlayerList[i].type == CharacterType.Warrior)
                {
                    Animation_Prefab = (GameObject)Resources.Load("Model/Player/Prefab/Guardian");
                    Character.GetComponent<Text>().text = "圣骑士";
                }
                else if (sys._model._RoomModule.PlayerList[i].type == CharacterType.Magician)
                {
                    Animation_Prefab = (GameObject)Resources.Load("Model/Player/Prefab/Magician");
                    Character.GetComponent<Text>().text = "元素使";
                }


                Animation_Prefab.transform.localScale = new Vector3(400, 400, 1);
                Animation_Prefab.transform.position = Global.PlayerPosList[sys._model._RoomModule.GetPlayerIndex(sys._model._RoomModule.PlayerList[i].uid)];
                GameObject Enginner_Instance = UnityEngine.Object.Instantiate(Animation_Prefab);
                sys._model._RoomModule.PlayerAnimation.Add(Enginner_Instance);


                GameObject username = GameObject.Find("Canvas/player" + (i + 1).ToString() + "/name");

                username.GetComponent<Text>().text = sys._model._RoomModule.PlayerList[i].username.ToString();

                GameObject status = GameObject.Find("Canvas/player" + (i + 1).ToString() + "/status");
                if (sys._model._RoomModule.PlayerList[i].ready)
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

}