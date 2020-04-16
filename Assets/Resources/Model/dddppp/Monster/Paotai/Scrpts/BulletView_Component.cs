using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletView_Component : MonoBehaviour
{
    public int v=5;
    void Update()
    {
            transform.Translate(new Vector3(Time.deltaTime * v, 0, 0));
    }
}
