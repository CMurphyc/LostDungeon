using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapManager : SceneManager
{
    List<string> SceneList;
    string scene_dir = "Scenes/";


    int SceneIndex = 0;

    public string CurrentScene  = "LoginPanel";

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
        SceneList.Add("Scenes/LoadingPanel");
        SceneList.Add("Scenes/Overview");

        AudioManager.instance.PlayAudio(AudioName.MainSceneBGM, true);

    }

    public void SwitchScene(string targetScene)
    {
        if (CurrentScene != targetScene)
        {
            SceneManager.LoadScene(scene_dir + targetScene);
            CurrentScene = targetScene;
            Debug.Log("ChangeScene: " + CurrentScene);
            SceneIndex = SceneList.IndexOf(scene_dir + targetScene);
            Debug.Log(targetScene);
            PlayAudioByScene(targetScene);
        }
        
    }

    public void PlayAudioByScene(string name)
    {
        //if(name == "MapCreate") AudioManager.instance.PlayAudio(AudioName.MainSceneBGM,true);
        switch (name)
        {
            case "LoginPanel":
                {
                    if (!AudioManager.instance.isPlaying(AudioName.MainSceneBGM))
                    {
                        AudioManager.instance.MuteAll();
                        AudioManager.instance.PlayAudio(AudioName.MainSceneBGM, true);
                    }
                    break;
                }
            case "Main":
                {
                    if (!AudioManager.instance.isPlaying(AudioName.MainSceneBGM))
                    {
                        AudioManager.instance.MuteAll();
                        AudioManager.instance.PlayAudio(AudioName.MainSceneBGM, true);
                    }
                    break;
                }
            case "RoomList":
                {
                    if (!AudioManager.instance.isPlaying(AudioName.MainSceneBGM))
                    {
                        AudioManager.instance.MuteAll();
                        AudioManager.instance.PlayAudio(AudioName.MainSceneBGM, true);
                    }
                    break;
                }
            case "HeroSelect":
                {
                    if (!AudioManager.instance.isPlaying(AudioName.TeamBGM))
                    {
                        AudioManager.instance.MuteAll();
                        AudioManager.instance.PlayAudio(AudioName.TeamBGM, true);
                    }
                    break;
                }
            
            case "MapCreate":
                {
                    if (!AudioManager.instance.isPlaying(AudioName.BattleBGM))
                    {
                        AudioManager.instance.MuteAll();
                        AudioManager.instance.PlayAudio(AudioName.BattleBGM, true);
                    }
                    break;
                }



            default:
                {
                    break;
                }
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



    public void PVE_RefreshRoomInfo()
    {

        Debug.Log("Refresh UI .......");
        Text RoomID = GameObject.Find("Canvas/RoomInfo").GetComponent<Text>();
        RoomID.text = "RoomID："+sys._model._RoomModule.roomid.ToString();

        for (int i = 0; i < 4; i++)
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
                    Character.GetComponent<Text>().text = "Guardian";
                }
                else if (sys._model._RoomModule.PlayerList[i].type == CharacterType.Magician)
                {
                    Animation_Prefab = (GameObject)Resources.Load("Model/Player/Prefab/Magician");
                    Character.GetComponent<Text>().text = "Wizard";
                }

                else if (sys._model._RoomModule.PlayerList[i].type == CharacterType.Ghost)
                {
                    Animation_Prefab = (GameObject)Resources.Load("Model/Player/Prefab/Ghost");
                    Character.GetComponent<Text>().text = "Ghost";
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
    public void RefreshPVP(List<PlayerData> temp, bool isRed)
    {
        for (int i = 0; i < temp.Count; i++)
        {
            string Path;
            if (isRed)
                Path = "Canvas/Player/player" + i.ToString();
            else
                Path = "Canvas/Player/player" + (i+5).ToString();
            //Debug.Log("Path: "+Path);
            Transform Player = GameObject.Find(Path).transform;

            if (!temp[i].empty)
            {
                //Debug.Log("I : "+ i);
                //Debug.Log("isRed : " + isRed);
                GameObject Animation_Prefab = null;

                if (temp[i].type == CharacterType.Enginner)
                {
                    Animation_Prefab = (GameObject)Resources.Load("Model/Player/Prefab/Engineer");
                    Player.Find("CharacterImage/Character").GetComponent<Text>().text = "Engineer";
                }
                else if (temp[i].type == CharacterType.Warrior)
                {
                    Animation_Prefab = (GameObject)Resources.Load("Model/Player/Prefab/Guardian");
                    Player.Find("CharacterImage/Character").GetComponent<Text>().text = "Warrior";
                }
                else if (temp[i].type == CharacterType.Magician)
                {
                    Animation_Prefab = (GameObject)Resources.Load("Model/Player/Prefab/Magician");
                    Player.Find("CharacterImage/Character").GetComponent<Text>().text = "Wizard";
                }
                else if (temp[i].type == CharacterType.Ghost)
                {
                    Animation_Prefab = (GameObject)Resources.Load("Model/Player/Prefab/Ghost");
                    Player.Find("CharacterImage/Character").GetComponent<Text>().text = "Ghost";
                }


                //Vector3 WorldPos = Camera.main.ScreenToWorldPoint(Global.PlayerPosList[sys._model._RoomModule.GetPlayerIndex(sys._model._RoomModule.PlayerList[i].uid)]);
                //Animation_Prefab.transform.position = WorldPos;

                GameObject Enginner_Instance = UnityEngine.Object.Instantiate(Animation_Prefab);
                Enginner_Instance.transform.parent = GameObject.Find("Canvas").transform;
                //int index = sys._model._RoomModule.GetPlayerIndex(sys._model._RoomModule.PlayerList[i].uid);
                Vector3 UIpos;
                GameObject username;
                GameObject status;
                if (isRed)
                {
                    UIpos = GameObject.Find("Canvas/Player/player" + (i).ToString() + "/Avatar").transform.position;
                    username = GameObject.Find("Canvas/Player/player" + (i).ToString() + "/nameImage/name");
                    status = GameObject.Find("Canvas/Player/player" + (i).ToString() + "/statusImage/status");
                    //Debug.Log("Path: " +"Canvas/Player/player" + (i).ToString() + "/nameImage/name");
                }
                else
                {
                    UIpos = GameObject.Find("Canvas/Player/player" + (i+5).ToString() + "/Avatar").transform.position;
                    username = GameObject.Find("Canvas/Player/player" + (i + 5).ToString() + "/nameImage/name");
                    status = GameObject.Find("Canvas/Player/player" + (i + 5).ToString() + "/statusImage/status");
                }
                Enginner_Instance.transform.position = UIpos;
                Enginner_Instance.transform.localScale = new Vector3(200, 200, 1);
                sys._model._RoomModule.PlayerAnimation.Add(Enginner_Instance);
            
                username.GetComponent<Text>().text = temp[i].username.ToString();

                if (temp[i].ready)
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
                GameObject username;
                GameObject status;
                GameObject character;
                if (isRed)
                {
                    username = GameObject.Find("Canvas/Player/player" + (i).ToString() + "/nameImage/name");
                    status = GameObject.Find("Canvas/Player/player" + (i).ToString() + "/statusImage/status");
                    character = GameObject.Find("Canvas/Player/player" + (i).ToString() + "/CharacterImage/Character");
                }
                else
                {
                    username = GameObject.Find("Canvas/Player/player" + (i+5).ToString() + "/nameImage/name");
                    status = GameObject.Find("Canvas/Player/player" + (i+5).ToString() + "/statusImage/status");
                    character = GameObject.Find("Canvas/Player/player" + (i+5).ToString() + "/CharacterImage/Character");
                }
                status.GetComponent<Text>().text = "";
                username.GetComponent<Text>().text = "";
                character.GetComponent<Text>().text = "";

              
            }














        }


    }

    public void PVP_RefreshRoomInfo()
    {
        //return;
        Debug.Log("Refresh UI .......");
        Text RoomID = GameObject.Find("Canvas/RoomInfo").GetComponent<Text>();
        RoomID.text = "RoomID：" + sys._model._RoomModule.roomid.ToString();


        Debug.Log("SIZE :"　+　sys._model._RoomModule.RedTeamPlayerList.Count);
        Debug.Log("SIZE :" + sys._model._RoomModule.BlueTeamPlayerList.Count);



        RefreshPVP(sys._model._RoomModule.RedTeamPlayerList, true);
        RefreshPVP(sys._model._RoomModule.BlueTeamPlayerList, false);


        //for (int i = 0; i < sys._model._RoomModule.PlayerList.Count; i++)
        //{
        //    string Path = "Canvas/Player/player" + i.ToString();
        //    //Debug.Log("Path: "+Path);
        //    Transform Player = GameObject.Find(Path).transform;

        //    if (!sys._model._RoomModule.PlayerList[i].empty)
        //    {
        //        GameObject Animation_Prefab = null;

        //        if (sys._model._RoomModule.PlayerList[i].type == CharacterType.Enginner)
        //        {
        //            Animation_Prefab = (GameObject)Resources.Load("Model/Player/Prefab/Engineer");
        //            Player.Find("CharacterImage/Character").GetComponent<Text>().text = "Engineer";
        //        }
        //        else if (sys._model._RoomModule.PlayerList[i].type == CharacterType.Warrior)
        //        {
        //            Animation_Prefab = (GameObject)Resources.Load("Model/Player/Prefab/Guardian");
        //            Player.Find("CharacterImage/Character").GetComponent<Text>().text = "Warrior";
        //        }
        //        else if (sys._model._RoomModule.PlayerList[i].type == CharacterType.Magician)
        //        {
        //            Animation_Prefab = (GameObject)Resources.Load("Model/Player/Prefab/Magician");
        //            Player.Find("CharacterImage/Character").GetComponent<Text>().text = "Wizard";
        //        }
        //        else if (sys._model._RoomModule.PlayerList[i].type == CharacterType.Ghost)
        //        {
        //            Animation_Prefab = (GameObject)Resources.Load("Model/Player/Prefab/Ghost");
        //            Player.Find("CharacterImage/Character").GetComponent<Text>().text = "Ghost";
        //        }


        //        //Vector3 WorldPos = Camera.main.ScreenToWorldPoint(Global.PlayerPosList[sys._model._RoomModule.GetPlayerIndex(sys._model._RoomModule.PlayerList[i].uid)]);
        //        //Animation_Prefab.transform.position = WorldPos;

        //        GameObject Enginner_Instance = UnityEngine.Object.Instantiate(Animation_Prefab);
        //        Enginner_Instance.transform.parent = GameObject.Find("Canvas").transform;
        //        int index = sys._model._RoomModule.GetPlayerIndex(sys._model._RoomModule.PlayerList[i].uid);

        //        Vector3 UIpos = GameObject.Find("Canvas/Player/player"+index.ToString()+"/Avatar").transform.position;
        //        Enginner_Instance.transform.position = UIpos;

        //        Enginner_Instance.transform.localScale = new Vector3(200, 200, 1);


        //        sys._model._RoomModule.PlayerAnimation.Add(Enginner_Instance);
        //        GameObject username = GameObject.Find("Canvas/Player/player" + (i ).ToString() + "/nameImage/name");

        //        username.GetComponent<Text>().text = sys._model._RoomModule.PlayerList[i].username.ToString();

        //        GameObject status = GameObject.Find("Canvas/Player/player" + (i).ToString() + "/statusImage/status");
        //        if (sys._model._RoomModule.PlayerList[i].ready)
        //        {
        //            status.GetComponent<Text>().text = "Ready";
        //        }
        //        else
        //        {
        //            status.GetComponent<Text>().text = "Waiting...";
        //        }
        //        GameObject btn = GameObject.Find("Canvas/btnReady/Text");
        //        btn.GetComponent<Text>().text = "Ready";
        //    }
        //    else
        //    {
        //        GameObject username = GameObject.Find("Canvas/Player/player" + (i ).ToString() + "/nameImage/name");
        //        GameObject status = GameObject.Find("Canvas/Player/player" + (i ).ToString() + "/statusImage/status");
        //        status.GetComponent<Text>().text = "";
        //        username.GetComponent<Text>().text = "";
        //        //Character.GetComponent<Text>().text = "";
        //    }
        //}
        GameObject btnStatus = GameObject.Find("Canvas/btnReady/Text");
        if (sys._model._PlayerModule.uid == sys._model._RoomModule.roomOwnerID)
        {
            btnStatus.GetComponent<Text>().text = "Start";
        }


    }

}