using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PackConverter 
{
    public static byte[] intToBytes(int value)
    {
        byte[] src = new byte[4];
        src[3] = (byte)((value >> 24) & 0xFF);
        src[2] = (byte)((value >> 16) & 0xFF);
        src[1] = (byte)((value >> 8) & 0xFF);//高8位
        src[0] = (byte)(value & 0xFF);//低位
        return src;
    }

    public static int bytesToInt(byte[] bytes, int size = 4)
    {
        int a = bytes[0] & 0xFF;
        a |= ((bytes[1] << 8) & 0xFF);
        a |= ((bytes[2] << 16) & 0xFF);
        a |= ((bytes[3] << 24) & 0xFF);
        return a;
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="data">要序列化的对象</param>
    /// <returns>返回存放序列化后的数据缓冲区</returns>
    public static byte[] Serialize(object data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream rems = new MemoryStream();
        formatter.Serialize(rems, data);
        return rems.GetBuffer();
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="data">数据缓冲区</param>
    /// <returns>对象</returns>
    public static object Deserialize(byte[] data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream rems = new MemoryStream(data);
        data = null;
        return formatter.Deserialize(rems);
    }
}