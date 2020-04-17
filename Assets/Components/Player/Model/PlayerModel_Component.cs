using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel_Component : MonoBehaviour
{

    public FixVector3 Position;
    public bool Rotation;
    public Fix64 HP;
    public int Frame;
    void Awake()
    {
        //Position = new FixVector3((Fix64)(-4),(Fix64)1,(Fix64)0);
        Position = new FixVector3((Fix64)transform.position.x, (Fix64)transform.position.y, (Fix64)transform.position.z);
    }

    public Vector3 GetPosition()
    {
        return new Vector3((float)Position.x,(float)Position.y,(float)Position.z);
    }
    public bool GetRotation()
    {
        return Rotation;
    }
    public void Move(Vector2 v)
    {
        v = v * 5f;
        Position.x += v.x;
        Position.y += v.y;
        if (v.x != 0)
        {
            Rotation = v.x < 0 ? true : false;
        }
    }
    public int GetFrame()
    {
        return Frame;
    }
}
