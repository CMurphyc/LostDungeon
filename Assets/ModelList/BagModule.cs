using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Item
{
    public int ItemID;

    public int ItemNumber;

}
public class BagModule  
{
    //队友背包
    public Dictionary<string, List<Item>> TeamPlayerBag;
    //当前玩家
    public List<Item> PlayerBag;
}
