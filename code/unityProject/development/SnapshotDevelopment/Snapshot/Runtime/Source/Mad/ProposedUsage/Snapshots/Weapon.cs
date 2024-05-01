using UnityEngine;

namespace ProposedArchitecture {

    public class Weapon : MonoBehaviour, ISnapshot {
        uint _Smri;
        public uint Smri { get { return _Smri; } }
        [SerializeField] int _Ammo;
        [SerializeField] bool _Loaded;

        Inventory _Inventory;

        void Awake() {
            RegisterToSaveManager();
        }

        public void SetInventory(Inventory _inventory){
            _Inventory = _inventory;
        }

        public void RegisterToSaveManager() {
            _Smri = Common.Instance.SaveManager.RegisterModel(this);
        }

        public void CacheModel() {
            Common.Instance.SaveManager.CacheModel(ConstructModel());
        }

        public ISnapshotModel ConstructModel() {
            SWeapon temp = new SWeapon() {
                Smri = this.Smri,
                RefSmris = new int[]{
                    (int)_Inventory.Smri,
                },
                _Ammo = this._Ammo,
                _Loaded = this._Loaded
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
