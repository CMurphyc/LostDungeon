using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OverViewWindowUpdate : MonoBehaviour
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
        int totkill = 0;
        List<PlayerData> PlayerList = sys._model._RoomModule.PlayerList;
        Dictionary<int, PVEData> PlayerPVEResult = sys._model._RoomModule.PVEResult;
        for (int i=0;i<4;i++)
        {
            if (PlayerList[i].empty)
            {
                Canvas.transform.Find("Player" + i.ToString()).gameObject.SetActive(false);
                continue;
            }
            Canvas.transform.Find("Player" + i.ToString()+"/"+"NickName").GetComponent<Text>().text = PlayerList[i].username;
            Canvas.transform.Find("Player" + i.ToString()+"/"+"Text").GetComponent<Text>().text = "";
            Canvas.transform.Find("Player" + i.ToString()+"/"+"Kill").GetComponent<Text>().text =
                PlayerPVEResult[PlayerList[i].uid].kills.ToString();
            Canvas.transform.Find("Player" + i.ToString()+"/"+"Coin").GetComponent<Text>().text = 
                PlayerPVEResult[PlayerList[i].uid].coins.ToString()+ "<color=#FFF900><size=20>+(" + ((int)(PlayerPVEResult[PlayerList[i].uid].kills*1.5f)).ToString() + ")</size></color>";

            switch(PlayerList[i].type)
            {
                case CharacterType.Enginner:
                    Canvas.transform.Find("Player" + i.ToString() + "/" + "Player0/Image").GetComponent<Image>().sprite =
                    Instantiate( Resources.Load("Model/Player/Sprites/Engineer/c06_s1_4",typeof(Sprite))) as Sprite;
                    break;
                case CharacterType.Magician:
                    Canvas.transform.Find("Player" + i.ToString() + "/" + "Player0/Image").GetComponent<Image>().sprite =
                    Instantiate( Resources.Load("Model/Player/Sprites/Magician/c03_4",typeof(Sprite))) as Sprite;
                    break;
                case CharacterType.Warrior:
                    Canvas.transform.Find("Player" + i.ToString() + "/" + "Player0/Image").GetComponent<Image>().sprite =
                    Instantiate( Resources.Load("Model/Player/Sprites/Guardian/c08_s2_4",typeof(Sprite))) as Sprite;
                    break;
            }
        }
        int GameTime = sys._model._RoomModule.PVEGameTime / 40;
        Canvas.transform.Find("End/Time/Text").GetComponent<Text>().text = (GameTime / 3600).ToString("d2") + " : " + ((GameTime % 3600) / 60).ToString("d2") + " : " + (GameTime % 60).ToString("d2");
        Canvas.transform.Find("End/Image/Text").GetComponent<Text>().text = sys._model._RoomModule.MapFloorNumber.ToString();
        Canvas.transform.Find("End/kill/Text").GetComponent<Text>().text = totkill.ToString();
    }

}
