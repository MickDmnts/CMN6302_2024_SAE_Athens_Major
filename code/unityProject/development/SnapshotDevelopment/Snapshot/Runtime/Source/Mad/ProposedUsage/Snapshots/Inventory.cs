using UnityEngine;

namespace ProposedArchitecture {

    public class Inventory : MonoBehaviour, ISnapshot {
        uint _Smri;
        public uint Smri { get { return _Smri; } }

        void Awake() {
            RegisterToSaveManager();
        }

        public void RegisterToSaveManager() {
            _Smri = Common.Instance.SaveManager.RegisterModel(this);
        }

        public void CacheModel() {
            Common.Instance.SaveManager.CacheModel(ConstructModel());
        }

        public ISnapshotModel ConstructModel() {
            SPlayer temp = new SPlayer() {
                Smri = this.Smri,
                RefSmris = new int[0],
            };

            return temp;
        }

        void OnDestroy() {
            UnregisterToSaveManager();
        }

        public void UnregisterToSaveManager() {
            Common.Instance.SaveManager.UnregisterFromSnapshot(this);
        }
    }
}
