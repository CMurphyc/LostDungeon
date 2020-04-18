using UnityEngine;
using System.Collections;

public class PlayerView_Component : MonoBehaviour
{
    GameObject player;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void RefreshView()
    {
        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition()
    {
        FixVector2 FloatPos = GetComponent<PlayerModel_Component>().GetPlayerPosition();
        if (FloatPos == new FixVector2((Fix64)transform.position.x, (Fix64)transform.position.y)) SetRun(false);
        else SetRun(true);
        transform.position = new Vector3((float)FloatPos.x, (float)FloatPos.y);
    }

    private void UpdateRotation()
    {
        GetComponent<SpriteRenderer>().flipX = GetComponent<PlayerModel_Component>().GetPlayerRotation();
    }

    public void Play(string animation)
    {
        anim.Play(animation);
    }

    public void SetRun(bool state)
    {
        anim.SetBool("IsRun", state);
    }
}
