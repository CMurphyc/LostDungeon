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
    public Transform Player;
    void Update()
    {
        if (IsOnDrag)
        {
            Player.GetComponent<PlayerModel_Component>().Move(Velocity / 1000);
            Player.GetComponent<PlayerView_Component>().Play("Run");
            if (Velocity.x < 0)
            {
                Player.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                Player.GetComponent<SpriteRenderer>().flipX = false;
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
        Debug.Log("Velocity:  " + Velocity);
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        IsOnDrag = false;
        Velocity = Vector2.zero;
        transform.Find("BK/thumb").localPosition = Vector3.zero;
        Player.GetComponent<PlayerView_Component>().Play("Idle");
    }
}
