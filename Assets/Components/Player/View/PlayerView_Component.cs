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
            if (transform.Find("back") != null)
            {
                transform.Find("back").gameObject.SetActive(true);
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);

            if (transform.Find("back") != null)
            {
                transform.Find("back").gameObject.SetActive(false);
            }
        }

        if (PlayerComp.buff.Invisible)
        {
            if (GameObject.FindWithTag("GameEntry") != null)
            {
                SystemManager sys = GameObject.FindWithTag("GameEntry").GetComponent<GameMain>().WorldSystem;


                RoomType type = sys._model._RoomListModule.roomType;
                if (type == RoomType.Pve)
                {
                    Color temp = GetComponent<SpriteRenderer>().color;
                    GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 120f / 255f);


                    transform.Find("weapon").gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 120f / 255f);

                    transform.Find("bag").gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 120f / 255f);
                    temp = transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color;
                    transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 120f / 255f);

                    GameObject Buff = transform.Find("back").gameObject;
                    if (Buff!=null)
                    {
                        Buff.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 120f / 255f);
                    }
                }
                else
                {
                    if(sys._pvpbattle._pvpplayer.FindPlayerTeamByGameObject(gameObject) == sys._pvpbattle._pvpplayer.FindCurrentPlayerTeam())
                    {
                        Color temp = GetComponent<SpriteRenderer>().color;
                        GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 120f / 255f);

                        transform.Find("weapon").gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 120f / 255f);

                        transform.Find("bag").gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 120f / 255f);

                        temp = transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color;
                        transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 120f / 255f);

                        GameObject Buff = transform.Find("back").gameObject;
                        if (Buff != null)
                        {
                            Buff.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 120f / 255f);
                        }
                    }
                    else
                    {
                        Color temp = GetComponent<SpriteRenderer>().color;
                        GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 0f / 255f);
                        transform.Find("weapon").gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 0);

                        transform.Find("bag").gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 0);

                        temp = transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color;
                        transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 0);

                        GameObject Buff = transform.Find("back").gameObject;
                        if (Buff != null)
                        {
                            Buff.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 0);
                        }

                    }
                }
            }
        }
        else
        {
            Color temp = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 255f / 255f);

            if (transform.Find("weapon")!=null)
                transform.Find("weapon").gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 1);
            if (transform.Find("bag")!= null)
                transform.Find("bag").gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 1);
            if (transform.Find("back")!= null)
                transform.Find("back").gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 1);

            if (transform.Find("bottom_circle") != null)
            {
                temp = transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color;
                transform.Find("bottom_circle").gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g, temp.b, 1);
            }
              
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
