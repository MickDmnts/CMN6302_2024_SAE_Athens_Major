using System;
using System.Runtime.InteropServices;
using MessagePack;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct DataContainer
{
    public uint Smri;
    public byte[] Data;
}

public class Wrapper
{
    [DllImport("dummyDll.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern Int16 cacheModel(DataContainer _model, Int32 _dataSize);

    [DllImport("dummyDll.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern DataContainer getModelData(UInt32 _dataSize);
}

[MessagePackObject]
public struct ModelNew
{
    [Key(0)]
    public uint smri;
    [Key(1)]
    public bool state;
    [Key(2)]
    public string sentence;
}

public class DLLWrapper : MonoBehaviour
{
    public bool _PushToDll;
    public bool _PullFromDll;

    public uint _Smri;
    public string _Sentence;
    public bool _State;

    void Update()
    {
        if (_PushToDll)
        {
            _PushToDll = false;

            //Represents a model
            ModelNew data = new ModelNew()
            {
                smri = _Smri,
                sentence = _Sentence,
                state = _State
            };

            DataContainer model = new DataContainer()
            {
                Smri = _Smri,
                Data = MessagePackSerializer.Serialize(data),
            };

            Int16 result = Wrapper.cacheModel(model, model.Data.Length);
            if (result == 0)
            {
                Debug.Log("Serialization successful");
            }
            else
            {
                Debug.LogWarning("Serialization failed...");
            }
        }

        if (_PullFromDll)
        {
            _PullFromDll = false;

            DataContainer container = Wrapper.getModelData(_Smri);
            ModelNew model = (ModelNew)MessagePackSerializer.Deserialize(typeof(ModelNew), container.Data);
            _Smri = model.smri;
            _Sentence = model.sentence;
            _State = model.state;
        }
    }
}
