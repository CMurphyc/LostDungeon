using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector2 JoyStickCenter;
    private Vector2 Direction;
    private bool IsPressDown;

    private float AttackCD;
    public float AttackSpeed;

    public float maxRadius;
    void Start()
    {
        Direction = new Vector2(1, 0);
        JoyStickCenter = transform.position;
    }
    void Update()
    {
        AttackCD -= Time.deltaTime;
        if(IsPressDown)
        {
            Attack();
        }
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        IsPressDown = true;
        Attack();
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Direction = eventData.position - JoyStickCenter;
        float radius = Mathf.Clamp(Direction.magnitude, 0, maxRadius);
        if(Direction.x<0)
        {
            if (transform.parent.parent.Find("weapons3_121") != null)
            {
                transform.parent.parent.Find("weapons3_121").GetComponent<SpriteRenderer>().flipY = true;
            }
        }
        else
        {
            if (transform.parent.parent.Find("weapons3_121") != null)
            {
                transform.parent.parent.Find("weapons3_121").GetComponent<SpriteRenderer>().flipY = false;
            }
        }
        if (transform.parent.parent.Find("weapons3_121") != null)
        {
            transform.parent.parent.Find("weapons3_121").eulerAngles = new Vector3(0, 0, Mathf.Atan2(Direction.y, Direction.x) * 180 / Mathf.PI);
        }
        transform.Find("BK/thumb").localPosition = Direction.normalized * radius;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        transform.Find("BK/thumb").localPosition = Vector3.zero;
        IsPressDown = false;
    }
    void Attack()
    {
        if (AttackCD > 0) return;
        if (transform.parent.parent.Find("weapons3_121") == null) return;
        //Main.socket.sock_c2s.AttackC2S();
        GameObject it = Instantiate(Resources.Load("Model/Bullet/bullet_121")) as GameObject;
        it.GetComponent<BulletModel_Component>().SetDirction(Direction.normalized);
        it.GetComponent<BulletModel_Component>().SetVelocity(1);
        it.GetComponent<BulletModel_Component>().SetPosition(transform.parent.parent.Find("weapons3_121").position);
        AttackCD = AttackSpeed;
    }
}
