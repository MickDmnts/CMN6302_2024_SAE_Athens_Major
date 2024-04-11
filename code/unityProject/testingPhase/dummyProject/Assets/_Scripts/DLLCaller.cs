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
    static extern void setNum(uint msg);
    [DllImport("dummyDll.dll")]
    static extern uint getNum();
    [DllImport("dummyDll.dll")]
    static extern bool isItCool(bool state);
    [DllImport("dummyDll.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void cacheByteArray(byte[] data, int size);
    [DllImport("dummyDll.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern IntPtr getByteArray(out int size);

    uint GetNum()
    {
        return getNum();
    }

    void SetNum(uint num)
    {
        setNum(num);
    }

    bool GetBool(bool state)
    {
        return isItCool(state);
    }

    void CacheBytesExternally(byte[] data, int size)
    {
        cacheByteArray(data, size);
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
        if (passNumber)
        {
            passNumber = false;

            SetNum((uint)number);

            number = (int)GetNum();

            Debug.Log(number);

            Debug.Log(GetBool(myBool));
        }

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
            CacheBytesExternally(bytes, bytes.Length);
        }

        if (retrieveSerialized)
        {
            retrieveSerialized = false;

            byte[] externalData = GetByteArrayExternally();

            serializedData = (Model)MessagePackSerializer.Deserialize(typeof(Model), externalData);
        }
    }
}
