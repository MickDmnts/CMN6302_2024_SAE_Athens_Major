using System;
using UnityEngine;

namespace ProposedArchitecture {

    ///<summary>@TODO: Summary</summary>
    public class Weapon : MonoBehaviour, ISnapshot {
        ///<summary>@TODO: Summary</summary>
        [SerializeField] uint _Smri;
        ///<summary>@TODO: Summary</summary>
        [SerializeField] int _Ammo;
        ///<summary>@TODO: Summary</summary>
        [SerializeField] bool _Loaded;

        ///<summary>@TODO: Summary</summary>
        Inventory _Inventory;

        ///<summary>@TODO: Summary</summary>
        public uint Smri { get { return _Smri; } }

        void Start() {
            if (Common.Instance.WorldLoader.FromLoad) { return; }

            _Ammo = UnityEngine.Random.Range(0, 100);
            _Loaded = UnityEngine.Random.Range(0, 100) < 50;
        }

        /// <summary>
        /// @TODO: Summary
        /// </summary>
        /// <param name="_inventory"></param>
        public void SetInventory(Inventory _inventory) {
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
                _Loaded = this._Loaded,
                _Position = transform.position,
                _Rotation = transform.rotation,
            };

            return temp;
        }

        void OnDestroy() {
            UnregisterToSaveManager();
        }

        public void UnregisterToSaveManager() {
            Common.Instance.SaveManager.UnregisterFromSnapshot(this);
        }

        public void LoadModel(ISnapshotModel _model) {
            SWeapon model = (SWeapon)_model;

            _Ammo = model._Ammo;
            _Loaded = model._Loaded;
            transform.position = model._Position;
            transform.rotation = model._Rotation;
        }

        public void RetrieveReferences(int[] _refSmris) {
            for (int i = 0; i < _refSmris.Length; i++) {
                _Inventory = (Inventory)Common.Instance.SaveManager.Snapshots[_refSmris[i]];
            }
        }

        public Type GetSnapshotModelType() {
            return typeof(SWeapon);
        }
    }
}
