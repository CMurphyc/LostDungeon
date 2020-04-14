using UnityEngine;
using UnityEditor;



public class ModelManager
{

    public RoomModule _RoomModule;
    public BagModule _BagModule;
    public PlayerModule _PlayerModule;




    public ModelManager()
    {
        _RoomModule = new RoomModule(this);
        _BagModule = new BagModule();
        _PlayerModule = new PlayerModule();
    }




}