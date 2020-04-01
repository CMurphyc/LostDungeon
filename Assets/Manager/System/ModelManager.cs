using UnityEngine;
using UnityEditor;



public class ModelManager
{

    public RoomModule _RoomModule;
    public BagModule _BagModule;
    public ModelManager()
    {
        _RoomModule = new RoomModule();
        _BagModule = new BagModule();
    }




}