using UnityEngine;
using System.Collections;

public class PlayerView_Component : MonoBehaviour
{
    GameObject main;
    private Animator ani;
    void Start()
    {
        ani =GetComponent<Animator>();
        //main = GameObject.FindWithTag("GameEntry");
    }
    void Update()
    {
        RefreshView();
    }
    public void RefreshView()
    {
        UpdatePosition();
    }
    private void UpdatePosition()
    {
        FixVector3 FixPos=gameObject.GetComponent<PlayerModel_Component>().GetPosition();
        Vector3 FloatPos = new Vector3((float)FixPos.x, (float)FixPos.y, (float)FixPos.z);
        //FloatPos= main.GetComponent<GameMain>().WorldSystem._model.
        transform.position = FloatPos;
        CameraManager.instance.transform.position = new Vector3(FloatPos.x, FloatPos.y, -2);
    }
    public void Play(string animation)
    {
        ani.Play(animation);
    }
}
