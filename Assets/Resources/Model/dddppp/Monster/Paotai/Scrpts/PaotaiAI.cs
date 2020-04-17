using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaotaiAI : MonoBehaviour
{
    int Counter = 0;
    //Queue<int> NextActFrame;
    public Transform PaoTrans;
    public Transform ShotTrans;
    public GameObject target;
    public int AttackFrameLoop;
    void Start()
    {
        //NextActFrame = new Queue<int>();
        AttackFrameLoop = 30;
    }
    public void UpdateLogic(int Frame)
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= 3)
        {
            Vector2 dir = target.transform.position - transform.position;
            float euler = Mathf.Atan2(dir.y, dir.x) * 180 / Mathf.PI;
            while (euler < 0) euler += 360;
            if (Mathf.Abs(PaoTrans.eulerAngles.z - euler) < 2)
            {
                if (Counter >= 20)
                {
                    AttackLogic(ShotTrans.position, ShotTrans.rotation);
                    Counter = 0;
                }
            }
            else
            {
                TurnLogic(dir);
            }
        }
        else
        {
            TurnLogic(new Vector2(1, 0));
        }
    }
    void Update()
    {
        Counter++;
        if(target!=null&&Vector2.Distance(target.transform.position,transform.position)<=3)
        {
            Vector2 dir = target.transform.position - transform.position;
            float euler = Mathf.Atan2(dir.y, dir.x) * 180 / Mathf.PI;
            while (euler < 0) euler += 360;
            if (Mathf.Abs(PaoTrans.eulerAngles.z- euler) <2)
            {
                if (Counter >= 20)
                {
                    AttackLogic(ShotTrans.position, ShotTrans.rotation);
                    Counter = 0;
                }
            }
            else
            {
                TurnLogic(dir);
            }
        }
        else
        {
            TurnLogic(new Vector2(1, 0));
        }
    }
    /// <summary>
    /// 攻击子弹
    /// </summary>
    /// <param name="ShotPosition">子弹生成位置</param>
    /// <param name="BulletRotation">子弹方向</param>
    void AttackLogic(Vector3 ShotPosition,Quaternion BulletRotation)
    {
        Instantiate(Resources.Load("dddppp/Monster/Paotai/Prefab/Bullet"), ShotPosition, BulletRotation);
    }
    void TurnLogic(Vector2 dir)
    {
        float A = PaoTrans.eulerAngles.z;
        float B = Mathf.Atan2(dir.y, dir.x) * 180 / Mathf.PI;
        while (A < 0) A += 360;
        while (B < 0) B += 360;
        if (A>B)
        {
            if(A-B<360-(A-B))A--;
            else A++;
        }
        else
        {
            if ((B- A) < 360 - (B - A))A++;
            else A--;
        }
        while (A > 360) A -= 360;
        while (A < 0) A += 360;
        if (A > 90 && A < 270) PaoTrans.GetComponent<SpriteRenderer>().flipY = true;
        else PaoTrans.GetComponent<SpriteRenderer>().flipY = false;
        PaoTrans.eulerAngles = new Vector3(0, 0, A);
    }
}


