#if UNITY_EDITOR
using ProposedArchitecture;
using Snapshot;
using UnityEngine;

public class DummySaveInitiator : MonoBehaviour {
    public bool _Save;
    public bool _RetrieveOnSmri;
    public uint _Smri;

    void Update() {
        if (_Save) {
            _Save = false;

            Common.Instance.SaveManager.Save();
        }

        if (_RetrieveOnSmri) {
            _RetrieveOnSmri = false;

            byte[] data = SnapshotWrapper.GetData(_Smri);
            
            SPlayer model = (SPlayer)MessagePack.MessagePackSerializer.Deserialize(typeof(SPlayer), data);
            Debug.Log($"Deserialized Smri:\n{model.Smri}\n" +
            $"Deserialized refs:\n{model.RefSmris}\n" +
            $"Deserialized health:\n{model._Health}\n" +
            $"Deserialized shield:\n{model._Shield}\n" +
            $"Deserialized stamina:\n{model._Stamina}\n" +
            $"Deserialized alive:\n{model._IsAlive}");
        }
    }
}
#endif
