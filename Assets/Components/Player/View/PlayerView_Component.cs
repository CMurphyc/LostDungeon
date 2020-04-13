using UnityEngine;
using System.Collections;

public class PlayerView_Component : MonoBehaviour
{
    GameObject main;
    private Animator ani;
    void Start()
    {
        ani =GetComponent<Animator>();
    }
    void Update()
    {
        RefreshView();
    }
    public void RefreshView()
    {
        UpdatePosition();
        UpdateRotation();
    }
    private void UpdatePosition()
    {
        Vector3 FloatPos = GetComponent<PlayerModel_Component>().GetPosition();
        transform.position = FloatPos;
        
    }
    private void UpdateRotation()
    {
        GetComponent<SpriteRenderer>().flipX = GetComponent<PlayerModel_Component>().GetRotation();
    }
    public void Play(string animation)
    {
        ani.Play(animation);
    }
}
