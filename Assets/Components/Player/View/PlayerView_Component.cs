﻿using UnityEngine;
using System.Collections;

public class PlayerView_Component : MonoBehaviour
{
    GameObject player;
    private Animator anim;

    private Vector3 Velocity = Vector3.zero;

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

        //transform.position = new Vector3((float)FloatPos.x, (float)FloatPos.y);
        FixVector2 FixedVec = GetComponent<PlayerModel_Component>().GetPlayerPosition();
        Vector2 CurrentPos = new Vector2((float)FixedVec.x, (float)FixedVec.y);
        transform.position = Vector3.SmoothDamp(transform.position, CurrentPos, ref Velocity, Global.FrameRate/1000f);
    }

    private void UpdateRotation()
    {
        if(GetComponent<PlayerModel_Component>().GetPlayerRotation())
        {
            this.transform.rotation = new Quaternion(0, 180, 0, 0);
            //this.transform.Rotate(0, 180, 0);
        }
        else
        {
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        //GetComponent<SpriteRenderer>().flipX = GetComponent<PlayerModel_Component>().GetPlayerRotation();
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
