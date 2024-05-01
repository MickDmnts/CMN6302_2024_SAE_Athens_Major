using System.Collections.Generic;
using UnityEngine;

namespace ProposedArchitecture {

    public class Inventory : MonoBehaviour, ISnapshot {
        uint _Smri;
        public uint Smri { get { return _Smri; } }

        [SerializeField] int _ItemCount;
        [SerializeField] List<Weapon> _Weapons;

        void Awake() {
            RegisterToSaveManager();
        }

        public void AddWeapon(Weapon _weapon) {
            _Weapons.Add(_weapon);
        }

        public void RegisterToSaveManager() {
            _Smri = Common.Instance.SaveManager.RegisterModel(this);
        }

        public void CacheModel() {
            Common.Instance.SaveManager.CacheModel(ConstructModel());
        }

        public ISnapshotModel ConstructModel() {
            SInventory temp = new SInventory() {
                Smri = this.Smri,
                RefSmris = new int[_Weapons.Count],
                _ItemCount = this._ItemCount,
            };

            for (int i = 0; i < temp.RefSmris.Length; i++) {
                temp.RefSmris[i] = (int)_Weapons[i].Smri;
            }

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
