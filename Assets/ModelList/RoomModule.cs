using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomModule 
{
    ModelManager model;
    public RoomModule(ModelManager parent)
    {
        model = parent;
    }

    public int roomid;

    public int roomOwnerID;
    public List<PlayerData> PlayerList = new List<PlayerData> { new PlayerData{ }, new PlayerData { }, new PlayerData { }, new PlayerData { } };

    List<GameObject> PlayerAnimation = new List<GameObject>();


    public void RemoveAllPlayer()
    {
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (!PlayerList[i].empty )
            {
                PlayerData temp = PlayerList[i];
                temp.empty = true;
                temp.uid = 0;
                temp.ready = false;
                Object.Destroy(temp.obj);
                temp.obj = null;
                PlayerList[i] = temp;
               

            }
        }
         PlayerAnimation.RemoveAll(it=> PlayerAnimation.Contains(it));
    }


    public void Add_Player(PlayerInfo playerinfo)
    {
        for (int i = 0; i < PlayerList.Count;i++)
        {
            if (PlayerList[i].empty)
            {
             
                PlayerData temp = PlayerList[i];
                temp.uid = playerinfo.PlayerId;
                temp.ready = playerinfo.IsReady;
                temp.type = (CharacterType)playerinfo.Role;
                temp.empty = false;
                PlayerList[i] = temp;
                GameObject Animation_Prefab;
                if (temp.type == CharacterType.Enginner)
                {
                    Animation_Prefab = (GameObject)Resources.Load("Resources/Model/Player/Prefab/Engineer");
                }
                else if (temp.type == CharacterType.Warrior)
                {
                    Animation_Prefab = (GameObject)Resources.Load("Resources/Model/Player/Prefab/Guardian");
                }
                else
                {
                    Animation_Prefab = (GameObject)Resources.Load("Resources/Model/Player/Prefab/Magician");
                }

                Animation_Prefab.transform.localScale = new Vector3(400, 400, 1);
                Animation_Prefab.transform.position = Global.PlayerPosList[(int)temp.type];
                GameObject Enginner_Instance = Object.Instantiate(Animation_Prefab);
                PlayerAnimation.Add(Enginner_Instance);


                Debug.Log("PlayerUid: " + temp.uid);
                Debug.Log("PlayerReady: " + temp.ready);
                Debug.Log("PlayerType: " + temp.type);
                //UI
                //NickName
                //Character
                //Ready

                break;
            }
        }
    }

    public void Delete_Player(int uid)
    {
        int DeleteIndex= -1;
        bool DeleteFromList = false;
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (!PlayerList[i].empty && PlayerList[i].uid == uid)
            {
                PlayerData temp = PlayerList[i];
                temp.empty = true;
                temp.uid = 0;
                temp.ready = false;
                Object.Destroy(temp.obj);
                temp.obj = null;
                PlayerList[i] = temp;
                DeleteFromList = true;
                DeleteIndex = i;


                //UI
                //NickName
                //Character
                //Ready




                break;
            }
        }
        if (DeleteFromList)
        {
            PlayerAnimation.RemoveAt(DeleteIndex);

        }
    }

    public CharacterType GetCharacterType(int uid)
    {
       for (int i =0; i < PlayerList.Count; i++)
       {
            if (PlayerList[i].uid == uid)
                return PlayerList[i].type;
       }
        return CharacterType.None;
    }



    public void ChangeCharacter(CharacterType type, int uid )
    {
        int PlayerUid = uid;
        if (type == CharacterType.Enginner)
        {
            for (int i = 0; i < PlayerList.Count;i++)
            {
                if (!PlayerList[i].empty && PlayerList[i].uid == PlayerUid && PlayerList[i].type != CharacterType.Enginner)
                {
                    GameObject Enginner_Prefab = (GameObject)Resources.Load("Resources/Model/Player/Prefab/Engineer");
                    Enginner_Prefab.transform.localScale = new Vector3(400, 400, 1);
                    Enginner_Prefab.transform.position = Global.PlayerPosList[0];
                    GameObject Enginner_Instance = Object.Instantiate(Enginner_Prefab);
                    PlayerData temp = PlayerList[i];
                    temp.type = CharacterType.Enginner;
                    temp.obj = Enginner_Instance;
                    PlayerAnimation.Add(Enginner_Instance);
                    PlayerList[i] = temp;


                    //UI
                    //Character



                    break;
                }
            }
        }
        else if (type == CharacterType.Warrior)
        {
            for (int i = 0; i < PlayerList.Count; i++)
            {
                if (!PlayerList[i].empty && PlayerList[i].uid == PlayerUid && PlayerList[i].type != CharacterType.Warrior)
                {
                    GameObject Enginner_Prefab = (GameObject)Resources.Load("Resources/Model/Player/Prefab/Guardian");
                    Enginner_Prefab.transform.localScale = new Vector3(400, 400, 1);
                    Enginner_Prefab.transform.position = Global.PlayerPosList[0];
                    GameObject Enginner_Instance = Object.Instantiate(Enginner_Prefab);
                    PlayerData temp = PlayerList[i];
                    temp.type = CharacterType.Enginner;
                    temp.obj = Enginner_Instance;
                    PlayerAnimation.Add(Enginner_Instance);
                    PlayerList[i] = temp;


                    //UI
                    //Character


                    break;
                }
            }

        }
        else if (type == CharacterType.Magician)
        {

            for (int i = 0; i < PlayerList.Count; i++)
            {
                if (!PlayerList[i].empty && PlayerList[i].uid == PlayerUid && PlayerList[i].type != CharacterType.Magician)
                {
                    GameObject Enginner_Prefab = (GameObject)Resources.Load("Resources/Model/Player/Prefab/Magician");
                    Enginner_Prefab.transform.localScale = new Vector3(400, 400, 1);
                    Enginner_Prefab.transform.position = Global.PlayerPosList[0];
                    GameObject Enginner_Instance = Object.Instantiate(Enginner_Prefab);
                    PlayerData temp = PlayerList[i];
                    temp.type = CharacterType.Enginner;
                    temp.obj = Enginner_Instance;
                    PlayerAnimation.Add(Enginner_Instance);
                    PlayerList[i] = temp;

                    //UI
                    //Character

                    break;
                }
            }
        }

    }

}


public struct PlayerData
{
    public bool empty;
    public int uid;
    public bool ready;
    public GameObject obj;
    public CharacterType type;
    public PlayerData(bool emp=true , int id=0 , bool status=false, GameObject gameObject =null , CharacterType character = CharacterType.Enginner)
    {
        empty = emp;
        uid = id;
        ready = status;
        obj = gameObject;
        type = character;
    }

}