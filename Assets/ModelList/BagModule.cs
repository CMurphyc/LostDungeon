using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item
{
    //道具ID
    public int ItemID;
    //道具数量
    public int ItemNumber;

}
public class BagModule  
{

    public Dictionary<int, List<Item>> PlayerBag = new Dictionary<int, List<Item>>();


    public void Free()
    {
        PlayerBag.Clear();
    }

    //添加道具
    public void AddItem(int UID, Item it)
    {
        if (!PlayerBag.ContainsKey(UID))
        {
            List<Item> newList = new List<Item>();
            PlayerBag.Add(UID, newList);
        }
        else
        {
            if (CheckItemExit(PlayerBag[UID], it.ItemID))
            {
                int ItemIndex = GetItemIndex(PlayerBag[UID], it.ItemID);
                if (ItemIndex != -1)
                {
                    PlayerBag[UID][ItemIndex].ItemNumber++;
                }
            }
            else
            {
                PlayerBag[UID].Add(it);
            }
        }
    }

    bool CheckItemExit(List<Item> it, int ItemID)
    {
        for (int i =0;i< it.Count;i++)
        {
            if (it[i].ItemID == ItemID)
            {

                return true;
            }
        }
        return false;
    }

    int GetItemIndex (List<Item> it, int ItemID)
    {
        for (int i = 0; i < it.Count; i++)
        {
            if (it[i].ItemID == ItemID)
            {

                return i;
            }
        }
        return -1;

    }
}


