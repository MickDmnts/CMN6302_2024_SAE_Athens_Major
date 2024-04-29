#if UNITY_EDITOR
using UnityEngine;
using MessagePack;
using System.ComponentModel;

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

    [ReadOnly(true)]
    public int _CurrentSmri;

    void Update() {
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
                _Sentence = "Hello from Unity",
                _State = !_GetSmri
            };

            DataContainer container = new DataContainer() {
                _Smri = data._Smri,
                _Data = MessagePackSerializer.Serialize(data)
            };
            container._DataSize = container._Data.Length;

            SnapshotWrapper.CacheData(container);
        }

        if (_RetrieveOnSmri) {
            _RetrieveOnSmri = false;

            DataContainer container = (DataContainer)SnapshotWrapper.GetData(_Smri);

            ModelDummy model = (ModelDummy)MessagePackSerializer.Deserialize(typeof(ModelDummy), container._Data);
            Debug.Log($"Deserialized Smri:\n{model._Smri}\n" +
            $"Deserialized Sentence:\n{model._Sentence}\n" +
            $"Deserialized State:\n{model._State}");
        }

        _CurrentSmri = SnapshotWrapper.GetCurrentSmri();
        _Smri = (uint)_CurrentSmri;

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
    }

    void OnDestroy() {
        Debug.Log("SMRI reset: " + SnapshotWrapper.ResetSmri());
        Debug.Log("Cache reset: " + SnapshotWrapper.ResetCache());
    }
}

#endif
