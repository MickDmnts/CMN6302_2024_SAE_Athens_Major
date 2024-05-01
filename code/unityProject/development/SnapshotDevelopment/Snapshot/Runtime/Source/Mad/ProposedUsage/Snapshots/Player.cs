using UnityEngine;

namespace ProposedArchitecture {

    public class Player : MonoBehaviour, ISnapshot {
        uint _Smri;
        public uint Smri { get { return _Smri; } }

        [SerializeField] float _Health;
        [SerializeField] float _Stamina;
        [SerializeField] float _Shield;
        [SerializeField] bool _IsAlive;

        Inventory _Inventory;

        void Awake() {
            RegisterToSaveManager();

            _Inventory = GetComponent<Inventory>();
            Weapon[] temp = GetComponents<Weapon>();
            for (int i = 0; i < temp.Length; i++) {
                temp[i].SetInventory(_Inventory);
                _Inventory.AddWeapon(temp[i]);
            }
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
                RefSmris = new int[]{
                    (int)_Inventory.Smri,
                },
                _Health = this._Health,
                _Stamina = this._Stamina,
                _Shield = this._Shield,
                _IsAlive = this._IsAlive,
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
