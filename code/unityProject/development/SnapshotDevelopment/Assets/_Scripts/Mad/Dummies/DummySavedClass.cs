#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChildClass {
    [SerializeField] uint _OwnSmri;
    [SerializeField] bool _State;

    public ChildClass() {
        _OwnSmri = SnapshotWrapper.GetSmri();
        _State = UnityEngine.Random.Range(0, 1000) < 500;
    }
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

    void OnPackStart(){
        //@TODO: 
    }
}
#endif
