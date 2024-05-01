using UnityEngine;

namespace ProposedArchitecture {

    public class Player : MonoBehaviour, ISnapshot {
        uint _Smri;
        public uint Smri { get { return _Smri; } }

        [SerializeField] float _Health;
        [SerializeField] float _Stamina;
        [SerializeField] float _Shield;
        [SerializeField] bool _IsAlive;

        Weapon _Weapon;
        Inventory _Inventory;

        void Awake() {
            RegisterToSaveManager();

            _Weapon = GetComponent<Weapon>();
            _Inventory = GetComponent<Inventory>();
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
                    (int)_Weapon.Smri,
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
