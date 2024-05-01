#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

using Snapshot;

struct ChildClassModel {
    public uint _Smri;
    public bool _State;
}

[Serializable]
public class ChildClass {
    [SerializeField] uint _OwnSmri;
    [SerializeField] bool _State;

    public uint Smri => _OwnSmri;

    public ChildClass() {
        _OwnSmri = SnapshotWrapper.GetSmri();
        _State = UnityEngine.Random.Range(0, 1000) < 500;
    }

    public void OnPackStart() {
        ChildClassModel data = new ChildClassModel() {
            _Smri = _OwnSmri,
            _State = _State
        };

        byte[] bytes = null;//SnapshotWrapper.StructToByteArray(data);
        SnapshotWrapper.CacheData(_OwnSmri, bytes.Length, bytes, null, 0);
    }
}

struct SaveClassModel {
    public uint _Smri;
    public int[] _RefSmris;
    public int _RefInstances;
    public string _Sentence;
}

public class DummySavedClass : MonoBehaviour {
    [SerializeField] uint _OwnSmri;
    [SerializeField] int _RefInstances;
    [SerializeField] List<ChildClass> _Refs;
    [SerializeField] string _Sentence;

    void Awake() {
        _OwnSmri = SnapshotWrapper.GetSmri();
        _Sentence = "Hello from smri " + _OwnSmri;
    }

    void Start() {
        for (int i = 0; i < _RefInstances; i++) {
            _Refs.Add(new ChildClass());
        }
    }

    public void OnPackStart() {
        SaveClassModel data = new SaveClassModel() {
            _Smri = _OwnSmri,
            _RefInstances = _RefInstances,
            _Sentence = _Sentence,
        };

        byte[] bytes = null; //SnapshotWrapper.StructToByteArray(data);
        int[] refSmris = new int[_Refs.Count];
        for (int i = 0; i < refSmris.Length; i++) {
            _Refs[i].OnPackStart();
            refSmris[i] = (int)_Refs[i].Smri;
        }
        SnapshotWrapper.CacheData(_OwnSmri, bytes.Length, bytes, refSmris, refSmris.Length);
    }
}
#endif
