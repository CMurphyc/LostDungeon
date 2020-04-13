using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletView_Component : MonoBehaviour
{
    
    void Update()
    {
        RefreshView();
    }
    public void RefreshView()
    {
        UpdatePosition();
    }
    private void UpdatePosition()
    {
        FixVector2 FixPos =GetComponent<BulletModel_Component>().GetPosition();
        Vector3 FloatPos = new Vector3((float)FixPos.x, (float)FixPos.y,0);
        transform.position = FloatPos;
        FixVector2 FixDir = GetComponent<BulletModel_Component>().GetDirction();
        transform.eulerAngles = new Vector3(0,0,Mathf.Atan2((float)FixDir.y,(float)FixDir.x)*180/Mathf.PI);
    }
}
