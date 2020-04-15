using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSelectUIEvent : MonoBehaviour
{
    GameObject main;
    // Start is called before the first frame update

    private void Awake()
    {
        main = GameObject.FindWithTag("GameEntry");
    }
    void Start()
    {
        
    }

    public void OnBtnReady()
    {
        main.GetComponent<GameMain>().socket.sock_c2s.PlayerReady();

    }

    public void OnBtnExit()
    {
        main.GetComponent<GameMain>().socket.sock_c2s.PlayerExitRoom();

    }

    public void OnHeroEngineer()
    {
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
