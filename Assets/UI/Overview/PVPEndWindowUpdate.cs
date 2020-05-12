using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PVPEndWindowUpdate : MonoBehaviour
{
    SystemManager sys;
    GameObject Canvas;
    private void Awake()
    {
        sys = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem;
        Canvas = GameObject.Find("Canvas");
    }
    void Start()
    {
        if(sys._pvpbattle._pvpplayer.FindPlayerTeamByUID(sys._model._PlayerModule.uid)==sys._pvpbattle._score.GetWinner())
        {
            Canvas.transform.Find("GameOver/Text").GetComponent<Text>().text = "Win";
            Canvas.transform.Find("GameOver/Text").GetComponent<Text>().color =new Color(1, 220.0f/255.0f,0);
        }
        else
        {
            Canvas.transform.Find("GameOver/Text").GetComponent<Text>().text = "Lose";
            Canvas.transform.Find("GameOver/Text").GetComponent<Text>().color = new Color(174.0f/255.0f, 174.0f / 255.0f, 174.0f / 255.0f);
        }
        for(int i=0;i<5;i++)
        {
            Canvas.transform.Find("Team/RedTeam/Player" + i.ToString()).gameObject.SetActive(false);
            Canvas.transform.Find("Team/BlueTeam/Player" + i.ToString()).gameObject.SetActive(false);
        }
        Dictionary<int, PVPData> PVP = sys._model._RoomModule.PVPResult;
        List<PlayerData> RedTeamPlayerList = sys._model._RoomModule.RedTeamPlayerList;
        for (int i = 0; i < RedTeamPlayerList.Count; i++)
        {
            string path ="Team/RedTeam/Player";
            Canvas.transform.Find(path + i.ToString() + "/NickName").GetComponent<Text>().text = RedTeamPlayerList[i].username;
            Canvas.transform.Find(path + i.ToString() + "/Text").GetComponent<Text>().text = "";
            Canvas.transform.Find(path + i.ToString() + "/Kill").GetComponent<Text>().text =PVP[RedTeamPlayerList[i].uid].kills.ToString();
            Canvas.transform.Find(path + i.ToString() + "/Dead").GetComponent<Text>().text = PVP[RedTeamPlayerList[i].uid].Dead.ToString();

            switch (RedTeamPlayerList[i].type)
            {
                case CharacterType.Enginner:
                    Canvas.transform.Find(path + i.ToString() + "/" + "Player0/Image").GetComponent<Image>().sprite =
                    Instantiate(Resources.Load("Model/Player/Sprites/Engineer/c06_s1_4", typeof(Sprite))) as Sprite;
                    break;
                case CharacterType.Magician:
                    Canvas.transform.Find(path + i.ToString() + "/" + "Player0/Image").GetComponent<Image>().sprite =
                    Instantiate(Resources.Load("Model/Player/Sprites/Magician/c03_4", typeof(Sprite))) as Sprite;
                    break;
                case CharacterType.Warrior:
                    Canvas.transform.Find(path + i.ToString() + "/" + "Player0/Image").GetComponent<Image>().sprite =
                    Instantiate(Resources.Load("Model/Player/Sprites/Guardian/c08_s2_4", typeof(Sprite))) as Sprite;
                    break;
                case CharacterType.Ghost:
                    Canvas.transform.Find(path + i.ToString() + "/Player0/Image").GetComponent<Image>().sprite =
                  Instantiate(Resources.Load("Model/Player/Sprites/Ghost/c02_s6_13", typeof(Sprite))) as Sprite;
                    break;
            }
            Canvas.transform.Find(path +i.ToString()).gameObject.SetActive(true);
            if(RedTeamPlayerList[i].uid==sys._model._PlayerModule.uid)
            {
                Canvas.transform.Find(path + i.ToString() + "/Image").gameObject.SetActive(true);
            }
        }

        List<PlayerData> BlueTeamPlayerList = sys._model._RoomModule.BlueTeamPlayerList;
        for (int i = 0; i < BlueTeamPlayerList.Count; i++)
        {
            string path ="Team/BlueTeam/Player";
            Canvas.transform.Find(path + i.ToString() + "/NickName").GetComponent<Text>().text = BlueTeamPlayerList[i].username;
            Canvas.transform.Find(path + i.ToString() + "/Text").GetComponent<Text>().text = "";
            Canvas.transform.Find(path + i.ToString() + "/Kill").GetComponent<Text>().text = PVP[BlueTeamPlayerList[i].uid].kills.ToString();
            Canvas.transform.Find(path + i.ToString() + "/Dead").GetComponent<Text>().text = PVP[BlueTeamPlayerList[i].uid].Dead.ToString();

            switch (BlueTeamPlayerList[i].type)
            {
                case CharacterType.Enginner:
                    Canvas.transform.Find(path + i.ToString() + "/Player0/Image").GetComponent<Image>().sprite =
                    Instantiate(Resources.Load("Model/Player/Sprites/Engineer/c06_s1_4", typeof(Sprite))) as Sprite;
                    break;
                case CharacterType.Magician:
                    Canvas.transform.Find(path + i.ToString() + "/Player0/Image").GetComponent<Image>().sprite =
                    Instantiate(Resources.Load("Model/Player/Sprites/Magician/c03_4", typeof(Sprite))) as Sprite;
                    break;
                case CharacterType.Warrior:
                    Canvas.transform.Find(path + i.ToString() + "/Player0/Image").GetComponent<Image>().sprite =
                    Instantiate(Resources.Load("Model/Player/Sprites/Guardian/c08_s2_4", typeof(Sprite))) as Sprite;
                    break;
                case CharacterType.Ghost:
                    Canvas.transform.Find(path + i.ToString() + "/Player0/Image").GetComponent<Image>().sprite =
                  Instantiate(Resources.Load("Model/Player/Sprites/Ghost/c02_s6_13", typeof(Sprite))) as Sprite;
                    break;
            }
            Canvas.transform.Find(path + i.ToString()).gameObject.SetActive(true);
            if (RedTeamPlayerList[i].uid == sys._model._PlayerModule.uid)
            {
                Canvas.transform.Find(path + i.ToString() + "/Image").gameObject.SetActive(true);
            }
        }
    }

}
