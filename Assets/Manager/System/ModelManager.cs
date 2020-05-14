using UnityEngine;
using UnityEditor;



public class ModelManager
{

    public RoomModule _RoomModule;
    public BagModule _BagModule;
    public PlayerModule _PlayerModule;

    public RoomListModule _RoomListModule;
    public JoyStickModule _JoyStickModule;
    public MiscModule _MiscModule;

    public ModelManager()
    {
        _RoomModule = new RoomModule(this);
        _BagModule = new BagModule();
        _PlayerModule = new PlayerModule();
        _RoomListModule = new RoomListModule();
        _JoyStickModule = new JoyStickModule();
        _MiscModule = new MiscModule();
    }




}