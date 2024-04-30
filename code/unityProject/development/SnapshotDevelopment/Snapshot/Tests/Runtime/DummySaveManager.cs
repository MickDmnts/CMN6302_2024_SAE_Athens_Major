#if UNITY_EDITOR
using UnityEngine;
using System.ComponentModel;
using System.IO;

using Snapshot;

public struct ModelDummy {
    public uint _Smri;
    public bool _State;
    public string _Sentence;
}

public class DummySaveManager : MonoBehaviour {
    [Header("Smris tests")]
    public bool _GetSmri;
    public bool _DecreaseSmri;
    public bool _ResetSmri;
    [Header("Model tests")]
    public bool _CacheModel;
    public uint _Smri;
    public bool _RetrieveOnSmri;
    [Header("Save path tests")]
    public string _SavePath;
    public bool _SetPath;
    public bool _GetPath;
    [Header("Packing")]
    public bool _Pack;
    [Header("Save file")]
    public string _SaveFileName;
    public bool _SetLoadFromFile;
    public bool _GetLoadFromFile;
    public bool _Unpack;

    [ReadOnly(true)]
    public int _CurrentSmri;

    void Update() {
        _CurrentSmri = SnapshotWrapper.GetCurrentSmri();

        if (_GetSmri) {
            _GetSmri = false;

            Debug.Log("Retrieved SMRI: " + SnapshotWrapper.GetSmri());
        }

        if (_DecreaseSmri) {
            _DecreaseSmri = false;

            SnapshotWrapper.DecreaseSmri();
            Debug.Log("Current SMRI: " + SnapshotWrapper.GetCurrentSmri());
        }

        if (_ResetSmri) {
            _ResetSmri = false;

            Debug.Log(SnapshotWrapper.ResetSmri());
        }

        if (_CacheModel) {
            _CacheModel = false;

            uint smri = SnapshotWrapper.GetSmri();
            ModelDummy data = new ModelDummy() {
                _Smri = smri,
                _Sentence = "Hello from Unity" + smri,
                _State = !_GetSmri
            };

            byte[] bytes = null;//SnapshotWrapper.StructToByteArray(data);

            SnapshotWrapper.CacheData(data._Smri, bytes.Length, bytes, new int[] { -1, -1, -1 }, 3);
        }

        if (_RetrieveOnSmri) {
            _RetrieveOnSmri = false;

            byte[] data = SnapshotWrapper.GetData(_Smri);

            ModelDummy model = new ModelDummy();//SnapshotWrapper.ByteArrayToStruct<ModelDummy>(data);
            Debug.Log($"Deserialized Smri:\n{model._Smri}\n" +
            $"Deserialized Sentence:\n{model._Sentence}\n" +
            $"Deserialized State:\n{model._State}");
        }

        if (_SetPath) {
            _SetPath = false;

            Debug.Log($"Set path:{SnapshotWrapper.SetSavePath(_SavePath)}");
        }

        if (_GetPath) {
            _GetPath = false;

            Debug.Log($"Save path: {SnapshotWrapper.GetSavePath()}");
        }

        if (_Pack) {
            _Pack = false;

            //@TODO: To be replaced from event
            GetComponent<DummySavedClass>().OnPackStart();
            Debug.Log($"Pack data: {SnapshotWrapper.PackData()}");
        }

        if (_SetLoadFromFile) {
            _SetLoadFromFile = false;

            Debug.Log($"Set load file name: {SnapshotWrapper.SetLoadFileName(_SaveFileName)}");
        }

        if (_GetLoadFromFile) {
            _GetLoadFromFile = false;

            Debug.Log($"Get load file name: {SnapshotWrapper.GetLoadFileName()}");
        }

        if (_Unpack) {
            _Unpack = false;

            Debug.Log($"Unpack from {Path.Combine(SnapshotWrapper.GetSavePath(), SnapshotWrapper.GetLoadFileName())}: {SnapshotWrapper.UnpackData()}");
        }
    }

    void OnDestroy() {
        Debug.Log("SMRI reset: " + SnapshotWrapper.ResetSmri());
        Debug.Log("Cache reset: " + SnapshotWrapper.ResetCache());
    }
}

#endif
