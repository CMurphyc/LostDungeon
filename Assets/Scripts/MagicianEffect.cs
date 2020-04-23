using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianEffect : MonoBehaviour
{
    GameObject baseCircle;
    GameObject effect;

    Transform pos;
    float Range;

    Queue<GameObject> effectList;
    Queue<int> effectFrame;

    public void init(GameObject a,GameObject b,Transform c)
    {
        baseCircle = a;
        effect = b;
        pos = c;
        Instantiate(a, c);
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        while (effectFrame.Count!=0) effectFrame.Dequeue();
        while (effectList.Count != 0) effectList.Dequeue();
    }
}
