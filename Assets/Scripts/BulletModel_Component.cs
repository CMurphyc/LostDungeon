using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletModel_Component : MonoBehaviour
{
    private Fix64 Velocity;
    private FixVector2 Dirction;
    private FixVector2 Position;
    void Awake()
    {
        Position = new FixVector2(-4,1);
    }
    void Update()
    {
        Move();
    }
    public FixVector2 GetPosition()
    {
        return Position;
    }
    public FixVector2 GetDirction()
    {
        return Dirction;
    }
    public void SetDirction(Vector2 Dirction)
    {
        this.Dirction =new FixVector2((Fix64)Dirction.x,(Fix64)Dirction.y);
    }
    public void SetVelocity(float Velocity)
    {
        this.Velocity =(Fix64) Velocity;
    }
    public void SetPosition(Vector2 Position)
    {
        this.Position = new FixVector2((Fix64)Position.x, (Fix64)Position.y);
    }
    public void Move()
    {
        Position += Dirction * Velocity*(Fix64)Time.deltaTime;
    }
}
