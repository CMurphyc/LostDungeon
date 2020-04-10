using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel_Component : MonoBehaviour
{

    FixVector3 Position;
    FixVector3 Rotation;
    Fix64 HP;
    int Frame;
    void Awake()
    {
        Position = new FixVector3((Fix64)(-4),(Fix64)1,(Fix64)0);
    }

    public FixVector3 GetPosition()
    {
        return Position;
    }
    public FixVector3 GetRotation()
    {
        return Rotation;
    }
    public void Move(Vector2 v)
    {
        Position.x += v.x;
        Position.y += v.y;
    }
    public int GetFrame()
    {
        return Frame;
    }
}
