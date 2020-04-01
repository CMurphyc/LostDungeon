using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class AnimationManager 
{
    SystemManager sys;
    public AnimationManager(SystemManager system)
    {

        sys = system;
    }

    // 触发帧号  ---- 对应List <Obj 和触发函数名>
    public Dictionary<int, List<AnimationEventPack>> _AnimationController = new Dictionary<int, List<AnimationEventPack>>();

    

    public void  Add_AnimationEvent(int Exit_Frame,GameObject obj, string EventName)
    {
        AnimationEventPack Pack;
        Pack.obj = obj;
        Pack.EventName = EventName;
        if (!_AnimationController.ContainsKey(Exit_Frame))
        {
            List<AnimationEventPack> temp = new List<AnimationEventPack>();
            temp.Add(Pack);
            _AnimationController.Add(Exit_Frame, temp);
        }
        else
        {
            _AnimationController[Exit_Frame].Add(Pack);

        }
    }

    

    ///////////////////////////////////////////////////////////

    public void Boss1_Attack()
    {

    }

    
}
