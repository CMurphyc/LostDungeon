using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BattleManager
{
    public int local_frame;

    SystemManager sys;

    public BattleManager()
    {
        local_frame = 0;
    }
    public BattleManager(SystemManager system)
    {
        local_frame = 0;
        sys = system;
    }
    void UpdateFrame()
    {
        UpdateLogicByFrame();

        UpdateView();
    }
    void UpdateLogicByFrame()
    {
        //to do (更新Model层)

        //人物碰撞检测

        //技能抛射物碰撞检测
        
        //人物移动位置更新

        //技能抛射物移动

        //NPC移动位置更新 （范围检测 AI寻路）

        //传送门范围判定

        //玩家 检测技能释放

        //NPC 检测技能释放

        //技能碰撞检测

     
      

        //动画帧事件检测触发

        if (sys._animation._AnimationController.ContainsKey(local_frame))
        {
            List<AnimationEventPack> temp = sys._animation._AnimationController[local_frame];
            for (int i = 0; i <temp.Count;i++)
            {
                Type Func = typeof(AnimationManager);

                MethodInfo method = Func.GetMethod(temp[i].EventName);
               
           
                if (method!=null)
                {
                    string ret = (string)method.Invoke(null, new object[]{ temp[i].obj});
                    Debug.Log("事件触发结果： " + ret);
                }
            }
        }

        local_frame++;
    }

    void UpdateView()
    {
        //HashSet<GameObject> hash = sys._pool._poolStorage[ObjectType.Player];
        //foreach (GameObject item in hash)
        //{
  
        //    item.GetComponent<PlayerView_Component>().RefreshView();
        //}

        //hash = sys._pool._poolStorage[ObjectType.NPC];
        //foreach (var item in hash)
        //{
        //    item.GetComponent<View_Component>().RefreshView();
        //}
        //hash = sys._pool._poolStorage[ObjectType.Portol];
        //foreach (var item in hash)
        //{
        //    item.GetComponent<View_Component>().RefreshView();
        //}
        //hash = sys._pool._poolStorage[ObjectType.Projectile];
        //foreach (var item in hash)
        //{
        //    item.GetComponent<View_Component>().RefreshView();
        //}

    }

}