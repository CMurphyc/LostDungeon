using UnityEngine;
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
        UpdateBuff();
        UpdatePosition();
        UpdateRotation();
    }

    private void UpdateBuff()
    {
        PlayerModel_Component PlayerComp = GetComponent<PlayerModel_Component>();
        if (PlayerComp.debuff.Poison)
        {
            GetComponent<SpriteRenderer>().color = new Color(134f / 255f, 255f / 255f, 100f / 255f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        }

        if (PlayerComp.buff.AttackIncrease)
        {
            GetComponent<SpriteRenderer>().color = new Color(150f / 255f, 255f / 255f, 255f / 255f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        }

    }
    private void UpdatePosition()
    {
        FixVector2 FloatPos = GetComponent<PlayerModel_Component>().GetPlayerPosition();
        Vector2 p = new Vector2((float)FloatPos.x,(float)FloatPos.y);

        if (!GetComponent<PlayerModel_Component>().inDash)
        {
            SetDash(false);
        }
        else
        {
            SetDash(true);
        }
        if (Mathf.Abs(p.x - transform.position.x) <= 0.1f && Mathf.Abs(p.y - transform.position.y) <= 0.1f)
            SetRun(false);
        else
        {
            SetRun(true);
        }

        if (this.GetComponent<PlayerModel_Component>().GetDead() == 1) SetDead(true);
        else SetDead(false);
        
        
        //transform.position = new Vector3((float)FloatPos.x, (float)FloatPos.y);
        FixVector2 FixedVec = GetComponent<PlayerModel_Component>().GetPlayerPosition();
        Vector2 CurrentPos = new Vector2((float)FixedVec.x, (float)FixedVec.y);
        transform.position = new Vector3(CurrentPos.x, CurrentPos.y);
        //transform.position = Vector3.SmoothDamp(transform.position, CurrentPos, ref Velocity, Global.FrameRate/1000f);

        Camera.main.GetComponent<CameraController>().ViewUpdate();
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

    public void SetRun(bool state)
    {
        anim.SetBool("IsRun", state);
    }

    public void SetDead(bool state)
    {
        anim.SetBool("dead", state);
    }
    void SetDash(bool state)
    {
        anim.SetBool("isDash", state);
    }
}
