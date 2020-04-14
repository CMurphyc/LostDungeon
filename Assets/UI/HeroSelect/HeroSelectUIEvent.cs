using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSelectUIEvent : MonoBehaviour
{
    GameObject main;
    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.FindWithTag("GameEntry");

    }

    public void OnBtnReady()
    {


    }

    public void OnBtnExit()
    {


    }

    public void OnHeroEngineer()
    {
        //int PlayerUid = main.GetComponent<GameMain>().WorldSystem._model._PlayerModule.uid;
        //CharacterType type =main.GetComponent<GameMain>().WorldSystem._model._RoomModule.GetCharacterType(PlayerUid);

        main.GetComponent<GameMain>().socket.sock_c2s.ChangeCharacter(CharacterType.Enginner);

    }

    public void OnHeroWarrior()
    {
        main.GetComponent<GameMain>().socket.sock_c2s.ChangeCharacter(CharacterType.Warrior);

    }

    public void OnHeroMagician()
    {
        main.GetComponent<GameMain>().socket.sock_c2s.ChangeCharacter(CharacterType.Magician);

    }
}
