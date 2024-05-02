using System;
using UnityEngine;

namespace ProposedArchitecture {

    public class Weapon : MonoBehaviour, ISnapshot {
        ///<summary>This ISnapshots SMRI</summary>
        [Header("Set Dynamically")]
        [SerializeField, Tooltip("This ISnapshots SMRI")] uint _Smri;
        ///<summary>Weapon ammo value</summary>
        [SerializeField, Tooltip("Weapon ammo value")] int _Ammo;
        ///<summary>Is the weapon loaded?</summary>
        [SerializeField, Tooltip("Is the weapon loaded?")] bool _Loaded;

        ///<summary>Reference to the inventory this weapon belongs to.</summary>
        Inventory _Inventory;

        ///<summary>Returns this ISnapshots SMRI</summary>
        public uint Smri { get { return _Smri; } }

        void Start() {
            //From load startup
            if (Common.Instance.WorldLoader.FromLoad) { return; }

            //Normal startup
            _Ammo = UnityEngine.Random.Range(0, 100);
            _Loaded = UnityEngine.Random.Range(0, 100) < 50;
        }

        /// <summary>
        /// Sets the inventory this weapon belongs to.
        /// </summary>
        /// <param name="_inventory">The inventory reference</param>
        public void SetInventory(Inventory _inventory) {
            _Inventory = _inventory;
        }

        ///<summary>Register the weapon to the save manager and set its SMRI</summary>
        public void RegisterToSaveManager() {
            _Smri = Common.Instance.SaveManager.RegisterModel(this);
        }

        ///<summary>Dynamically called when its time to save</summary>
        public void CacheModel() {
            Common.Instance.SaveManager.CacheModel(ConstructModel());
        }

        /// <summary>
        /// Returns an ISnapshotModel with the player needed data.
        /// </summary>
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

        ///<summary>The ISnapshot should get unregistered if it gets destroyed</summary>
        void OnDestroy() {
            UnregisterToSaveManager();
        }

        ///<summary>Unregisters the reference from the save manager</summary>
        public void UnregisterToSaveManager() {
            Common.Instance.SaveManager.UnregisterFromSnapshot(this);
        }

        /// <summary>
        /// Sets the weapon fields from the incoming deserialized model
        /// </summary>
        /// <param name="_model">SWeapon model containing the deserialized data</param>
        public void LoadModel(ISnapshotModel _model) {
            SWeapon model = (SWeapon)_model;

            _Ammo = model._Ammo;
            _Loaded = model._Loaded;
            transform.position = model._Position;
            transform.rotation = model._Rotation;
        }

        /// <summary>
        /// Sets any reference the weapon may have, like its inventory.
        /// </summary>
        public void RetrieveReferences(int[] _refSmris) {
            for (int i = 0; i < _refSmris.Length; i++) {
                _Inventory = (Inventory)Common.Instance.SaveManager.Snapshots[_refSmris[i]];
            }
        }

        /// <summary>
        /// Returns the type of the ISnapshotModel this ISnapshot's data get represented.
        /// </summary>
        public Type GetSnapshotModelType() {
            return typeof(SWeapon);
        }
    }
}
