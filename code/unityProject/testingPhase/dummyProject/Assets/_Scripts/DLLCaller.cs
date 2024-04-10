using UnityEngine;
using System.Runtime.InteropServices;

public class DLLCaller : MonoBehaviour
{
    [DllImport("dummyDll.dll", CallingConvention = CallingConvention.Cdecl)]
    static extern void setNum(uint msg);
    [DllImport("dummyDll.dll")]
    static extern uint getNum();
    [DllImport("dummyDll.dll")]
    static extern bool isItCool(bool state);

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

    public bool passNumber;
    public int number;
    public bool myBool;

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
    }
}
