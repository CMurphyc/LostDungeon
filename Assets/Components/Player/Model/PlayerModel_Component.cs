using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel_Component : MonoBehaviour
{

    FixVector3 Position;
    bool Rotation;
    Fix64 HP;
    int Frame;
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
