using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PackConverter 
{
    public static FixVector3 Vector3ToFixVector3(Vector3 vec3)
    {
        return new FixVector3((Fix64)vec3.x, (Fix64)vec3.y, (Fix64)vec3.z);
    }

    public static Vector3 FixVector3ToVector3(FixVector3 fixvec3)
    {
        return new Vector3((float)fixvec3.x, (float)fixvec3.y, (float)fixvec3.z);
    }

    public static FixVector2 FixVector3ToFixVector2(FixVector3 fixvec3)
    {
        return new FixVector2(fixvec3.x, fixvec3.y);
    }
    public static FixVector3 FixVector2ToFixVector3(FixVector2 fixvec3)
    {
        return new FixVector3(fixvec3.x, fixvec3.y , (Fix64)0);
    }

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