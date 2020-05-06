using UnityEngine;
using System.Collections;

public class OverViewWindowUpdate : MonoBehaviour
{
    SystemManager sys;
    private void Awake()
    {
        sys = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
