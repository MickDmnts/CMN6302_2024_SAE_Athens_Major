using UnityEngine;
using System.Runtime.InteropServices;
using System;
using MessagePack;

[Serializable, MessagePackObject]
public struct Model
{
    [Key(0)]
    public int number;
    [Key(1)]
    public bool myBool;
    [Key(2)]
    public int[] numbers;
}

public class DLLCaller : MonoBehaviour
{
    [DllImport("dummyDll.dll")]
    static extern void setNum(Int32 msg);
    [DllImport("dummyDll.dll")]
    static extern Int32 getNum();
    [DllImport("dummyDll.dll")]
    static extern bool isItCool(bool state);
    [DllImport("dummyDll.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern Int32 cacheByteArray(byte[] data, Int32 size);
    [DllImport("dummyDll.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern IntPtr getByteArray(out int size);

    Int32 GetNum()
    {
        return getNum();
    }

    void SetNum(Int32 num)
    {
        setNum(num);
    }

    bool GetBool(bool state)
    {
        return isItCool(state);
    }

    Int32 CacheBytesExternally(byte[] data, int size)
    {
        return cacheByteArray(data, size);
    }

    byte[] GetByteArrayExternally()
    {
        int size = -1;
        IntPtr unmanagedArray = getByteArray(out size);

        Debug.Log(size);

        byte[] bytes = new byte[size];
        Marshal.Copy(unmanagedArray, bytes, 0, size);

        return bytes;
    }

    public bool passNumber;
    [Space()]
    public int number;
    public bool myBool;
    [Space()]
    public int[] numbers;
    public bool serializeData;
    [Space()]
    public bool retrieveSerialized;
    public Model serializedData;

    void Update()
    {
        passNumber = true;
        if (passNumber)
        {
            passNumber = false;

            SetNum((Int32)number);

            number = (Int32)GetNum();

            Debug.Log(number);

            myBool = GetBool(myBool);
        }   

        serializeData = true;
        if (serializeData)
        {
            serializeData = false;

            Model temp = new Model()
            {
                number = number,
                myBool = myBool,
                numbers = numbers
            };

            byte[] bytes = MessagePackSerializer.Serialize(typeof(Model), temp);
            int rc = CacheBytesExternally(bytes, bytes.Length);
            if (rc != 0)
            { Debug.LogError($"Serialization caching ended with code: {rc}"); }
            else
            { Debug.Log($"Serialization caching ended with code: {rc}"); }
        }

        retrieveSerialized = true;
        if (retrieveSerialized)
        {
            retrieveSerialized = false;

            byte[] externalData = GetByteArrayExternally();

            serializedData = (Model)MessagePackSerializer.Deserialize(typeof(Model), externalData);
        }
    }
}
