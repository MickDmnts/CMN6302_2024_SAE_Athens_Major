#if UNITY_EDITOR
using UnityEngine;
using System.ComponentModel;
using System.IO;

using Snapshot;
using MessagePack;

[MessagePackObject]
public struct ModelDummy {
    [Key(0)]
    public uint _Smri;
    [Key(1)]
    public bool _State;
    [Key(2)]
    public string _Sentence;
    [Key(3)]
    public int[] _RefSmris;
}

/// <summary>
/// Test class from early stages.
/// </summary>
public class DummySaveManager : MonoBehaviour {
    [Header("Smris tests")]
    public bool _GetSmri;
    public bool _DecreaseSmri;
    public bool _ResetSmri;
    public bool _DeleteDataOnSmri;
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
                _State = !_GetSmri,
                _RefSmris = new int[] { 0, 1, 2, 3 }
            };

            byte[] bytes = MessagePackSerializer.Serialize<ModelDummy>(data);
            int[] refSmris = data._RefSmris;

            SnapshotWrapper.CacheData(data._Smri, bytes, refSmris);
        }

        if (_RetrieveOnSmri) {
            _RetrieveOnSmri = false;

            byte[] data = SnapshotWrapper.GetData(_Smri);

            ModelDummy model = (ModelDummy)MessagePackSerializer.Deserialize(typeof(ModelDummy), data);
            Debug.Log($"Deserialized Smri:\n{model._Smri}\n" +
            $"Deserialized Sentence:\n{model._Sentence}\n" +
            $"Deserialized State:\n{model._State}\n" +
            $"Deserialized refs:\n{model._RefSmris}");

            for (int i = 0; i < model._RefSmris.Length; i++) {
                Debug.Log(model._RefSmris[i]);
            }
        }

        if (_DeleteDataOnSmri) {
            _DeleteDataOnSmri = false;

            Debug.Log($"Delete smri data of SMRI {_Smri}:{SnapshotWrapper.DeleteSmriData(_Smri)}");
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
