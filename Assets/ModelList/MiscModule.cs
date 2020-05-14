using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscModule 
{
    public bool needUpdatePing = false;
    public bool needHeartbeat = true;
    public bool NeedUpdatePing()
    {
        if (needUpdatePing)
        {
            needUpdatePing = false;
            return true;
        }
        return false;
    }

    public bool NeedHeartbeat()
    {
        if (needHeartbeat)
        {
            needHeartbeat = false;
            return true;
        }
        return false;
    }

}
