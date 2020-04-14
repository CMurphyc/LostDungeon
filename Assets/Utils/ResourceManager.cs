using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMgr : MonoBehaviour
{
    //这边主要是需要加载的一些资源（或者配置表）的容器，需要什么加什么
    // public static List<GameObject> skParticle;
    // public static SkillConfigs skConf;

    public GameObject GetInstance(string path)
    {
        return GameObject.Instantiate(GetResource<GameObject>(path));
    }
    public T GetResource<T>(string path) where T : Object
    {
        return Resources.Load(path) as T;
    }
    void Awake()
    {
        //添加需要加载（读取）的资源或配置的函数，这边照我下面写的sample的样子改改就ok
        // readskConfig();
    }

    private void readskConfig()
    {
        // skParticle = new List<GameObject>();
        // skConf = GetResource<SkillConfigs>("Config/SkillConfigs");
        // foreach(var it in skConf.skillConfigs)
        // {
        //     skParticle.Add(GetResource<GameObject>("SkillEffect/" + it.skillParticle));
        // }
    }
}