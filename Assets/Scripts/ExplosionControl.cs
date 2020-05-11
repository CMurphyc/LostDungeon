using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionControl : MonoBehaviour
{
    private int startFrame;
    private int endFrame;
    private Animator an;
    public void init(int frame,int c,Vector3 x)
    {
        startFrame = frame;
        endFrame = frame + c;
        this.transform.position = x;
        an = this.GetComponent<Animator>();
    }

    public int getEndFrame()
    {
        return endFrame;
    }

    public void updateLogic(int frame)
    {
        
        if (startFrame == frame&&an!=null)
        {
            Debug.Log(frame);
            an.SetBool("explosion",true);
        }
    }
}
