using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MoveJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private GameMain Main;
    private Vector2 Velocity = Vector2.zero;
    private Vector2 JoyStickCenter;
    private bool IsOnDrag;

    public float maxRadius;
    void Start()
    {
        //Main = GameObject.FindWithTag("GameEntry").GetComponent<GameMain>();
    }
    void Update()
    {
        if (IsOnDrag)
        {
            //Debug.Log(Velocity);
            //Main.socket.sock_c2s.MoveC2S();
            if (transform.parent.parent.GetComponent<PlayerView_Component>() == null)
            {
                Debug.Log("??");
                return;
            }
            transform.parent.parent.GetComponent<PlayerModel_Component>().Move(Velocity / 1000);
            transform.parent.parent.GetComponent<PlayerView_Component>().Play("Run");
            if (Velocity.x < 0)
            {
                transform.parent.parent.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                transform.parent.parent.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        transform.Find("BK").position=eventData.position;
        JoyStickCenter = eventData.position;
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        IsOnDrag = true;
        Vector2 vec = eventData.position - JoyStickCenter;
        float radius = Mathf.Clamp(vec.magnitude, 0, maxRadius);
        transform.Find("BK/thumb").localPosition= vec.normalized* radius;
        Velocity = vec.normalized * radius;
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        IsOnDrag = false;
        Velocity = Vector2.zero;
        transform.Find("BK/thumb").localPosition = Vector3.zero;
        if(transform.parent.parent.GetComponent<PlayerView_Component>()!=null)transform.parent.parent.GetComponent<PlayerView_Component>().Play("Idle");
    }
}
