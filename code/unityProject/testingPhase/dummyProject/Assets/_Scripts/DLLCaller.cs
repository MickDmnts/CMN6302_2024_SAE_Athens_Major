using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;
using System;

[Serializable]
public struct Model
{
    public int number;
    public bool myBool;
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
    /* [DllImport("dummyDll.dll")]
    static extern IntPtr getByteArray(); */

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

    /* byte[] GetByteArrayExternally()
    {
        IntPtr ptr = getByteArray();
        

        return getByteArray();
    } */

    public bool passNumber;
    [Space()]
    public int number;
    public bool myBool;
    [Space()]
    public int[] numbers;
    public bool serializeData;
    [Space()]
    public int index;
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

            string json = JsonUtility.ToJson(temp);
            CacheBytesExternally(Encoding.UTF8.GetBytes(json), json.Length);
        }

        /* if (retrieveSerialized)
        {
            retrieveSerialized = false;
            byte[] externalData = GetByteArrayExternally();
            string deserialized = Encoding.UTF8.GetString(externalData);
            serializedData = JsonUtility.FromJson<Model>(deserialized);
        } */
    }
}
