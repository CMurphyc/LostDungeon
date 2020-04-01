using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginEvent : MonoBehaviour
{
    GameObject main;
    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.FindWithTag("GameEntry");
       
    }

    public void OnBtnLogin()
    {
        string UserName = GameObject.Find("Canvas/username").GetComponent<InputField>().text;
        string Passward = GameObject.Find("Canvas/passward").GetComponent<InputField>().text;

        if (UserName.Length == 0 || Passward.Length == 0)
        {
            print("账户或密码不能为空");
            return;
        }

        main.GetComponent<GameMain>().socket.sock_c2s.LoginC2S(UserName, Passward);
        print("Login Request Send");
    }

    public void OnBtnRegister()
    {
        string UserName = GameObject.Find("Canvas/username").GetComponent<InputField>().text;
        string Passward = GameObject.Find("Canvas/passward").GetComponent<InputField>().text;
        string NickName = GameObject.Find("Canvas/nickname").GetComponent<InputField>().text;
        if (UserName.Length == 0 || Passward.Length == 0)
        {
            print("账户或密码不能为空");
            return;
        }
        if (NickName.Length == 0)
        {
            print("昵称不能为空");
            return;
        }

        main.GetComponent<GameMain>().socket.sock_c2s.RegisterC2S(UserName, Passward, NickName);
        print("Register Request Send");
    }
}
