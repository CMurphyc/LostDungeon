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
        Text RoomID = GameObject.Find("Canvas/RoomInfo").GetComponent<Text>();
        RoomID.text = "RoomID："+sys._model._RoomModule.roomid.ToString();

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
                    Character.GetComponent<Text>().text = "Engineer";
                }
                else if (sys._model._RoomModule.PlayerList[i].type == CharacterType.Warrior)
                {
                    Animation_Prefab = (GameObject)Resources.Load("Model/Player/Prefab/Guardian");
                    Character.GetComponent<Text>().text = "Warrior";
                }
                else if (sys._model._RoomModule.PlayerList[i].type == CharacterType.Magician)
                {
                    Animation_Prefab = (GameObject)Resources.Load("Model/Player/Prefab/Magician");
                    Character.GetComponent<Text>().text = "Wizard";
                }
                //Animation_Prefab.transform.localScale = new Vector3(400, 400, 1);
      
                //Vector3 WorldPos = Camera.main.ScreenToWorldPoint(Global.PlayerPosList[sys._model._RoomModule.GetPlayerIndex(sys._model._RoomModule.PlayerList[i].uid)]);
                //Animation_Prefab.transform.position = WorldPos;

                GameObject Enginner_Instance = UnityEngine.Object.Instantiate(Animation_Prefab);
                Enginner_Instance.transform.parent = GameObject.Find("Canvas").transform;
                int index = sys._model._RoomModule.GetPlayerIndex(sys._model._RoomModule.PlayerList[i].uid);
                if (index == 0)
                {
                    Vector3 UIpos = GameObject.Find("Canvas/player1").transform.position;
                    Enginner_Instance.transform.position = UIpos;
                }
                else if  (index ==1)
                {
                    Vector3 UIpos = GameObject.Find("Canvas/player2").transform.position;
                    Enginner_Instance.transform.position = UIpos;

                }
                else if (index ==2)
                {
                    Vector3 UIpos = GameObject.Find("Canvas/player3").transform.position;
                    Enginner_Instance.transform.position = UIpos;
                }
                else if (index ==3)
                {
                    Vector3 UIpos = GameObject.Find("Canvas/player4").transform.position;
                    Enginner_Instance.transform.position = UIpos;

                }

                Enginner_Instance.transform.localScale = new Vector3(400, 400, 1);


                sys._model._RoomModule.PlayerAnimation.Add(Enginner_Instance);


            
                GameObject username = GameObject.Find("Canvas/player" + (i + 1).ToString() + "/name");

                username.GetComponent<Text>().text = sys._model._RoomModule.PlayerList[i].username.ToString();

                GameObject status = GameObject.Find("Canvas/player" + (i + 1).ToString() + "/status");
                if (sys._model._RoomModule.PlayerList[i].ready)
                {
                    status.GetComponent<Text>().text = "Ready";
                }
                else
                {
                    status.GetComponent<Text>().text = "Waiting...";
                }
                GameObject btn = GameObject.Find("Canvas/btnReady/Text");
                btn.GetComponent<Text>().text = "Ready";
            }
            else
            {
                GameObject username = GameObject.Find("Canvas/player" + (i + 1).ToString() + "/name");
                GameObject status = GameObject.Find("Canvas/player" + (i + 1).ToString() + "/status");
                status.GetComponent<Text>().text = "";
                username.GetComponent<Text>().text = "";
                Character.GetComponent<Text>().text = "";
            }
        }
        GameObject btnStatus = GameObject.Find("Canvas/btnReady/Text");
        if (sys._model._PlayerModule.uid == sys._model._RoomModule.roomOwnerID)
        {
            btnStatus.GetComponent<Text>().text = "Start";
        }
       

    }

}