using UnityEngine;
using MessagePack;
using System.ComponentModel;

[MessagePackObject]
public struct ModelDummy {
    [Key(0)]
    public uint _Smri;
    [Key(1)]
    public bool _State;
    [Key(2)]
    public string _Sentence;
}

public class SaveManager : MonoBehaviour {
    public bool _GetSmri;
    public bool _DecreaseSmri;
    public bool _ResetSmri;
    public bool _CacheModel;
    public bool _RetrieveOnSmri;
    public uint _Smri;

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
                _Data = MessagePackSerializer.Serialize(data),
            };

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
    }

    void OnDestroy() {
        Debug.Log("SMRI reset: " + SnapshotWrapper.ResetSmri());
        Debug.Log("Cache reset: " + SnapshotWrapper.ResetCache());
    }
}
