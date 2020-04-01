using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 public class PoolManager 
{

    public Dictionary<ObjectType, HashSet<GameObject>> _poolStorage;

    public PoolManager()
    {
        _poolStorage = new Dictionary<ObjectType, HashSet<GameObject>>();
    }


    void Add_Object ( GameObject obj, ObjectType obj_type)
    {
        if (_poolStorage.ContainsKey(obj_type))
        {
            _poolStorage[obj_type].Add(obj);
        }
        else
        {
            HashSet<GameObject> New_Add = new HashSet<GameObject>();
            New_Add.Add(obj);
            _poolStorage.Add(obj_type, New_Add);
        }
    }


    void Release_Object()
    {
        _poolStorage.Clear();
    }

}
