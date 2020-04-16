using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainModule : MonoBehaviour
{
    BattleManager _parentManager;


    public Dictionary<int, List<GameObject>> roomToStone = new Dictionary<int, List<GameObject>>();   // 房间号对应的石头列表
    public Dictionary<int, List<int>> roomToDoor = new Dictionary<int, List<int>>();   // 房间号对应的门列表
    public Dictionary<int, GameObject> doornumToDoor = new Dictionary<int, GameObject>() ;   // 门号对应门的实体
    public Dictionary<int, DoorData> doorToDoor = new Dictionary<int, DoorData>() ;   // 一个编号的门传送到的另一个门的编号
    public Dictionary<int, int> doorToRoom = new Dictionary<int, int>() ;   // 一个编号的门对应的房间编号

    public TerrainModule(BattleManager parent)
    {
        _parentManager = parent;
    }

    public void UpdateLogic(int frame)
    {



    }
    public void UpdateView()
    {



    }
}
