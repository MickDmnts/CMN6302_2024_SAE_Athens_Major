using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProposedArchitecture {

    public class Inventory : MonoBehaviour, ISnapshot {

        ///<summary>This ISnapshots SMRI</summary>
        [Header("Set Dynamically")]
        [SerializeField, Tooltip("This ISnapshots SMRI")] uint _Smri;
        ///<summary>Max items the inventory can hold</summary>
        [SerializeField, Tooltip("Max items the inventory can hold")] int _MaxItems;
        ///<summary>Inventory weapons</summary>
        [SerializeField, Tooltip("Inventory weapons")] List<Weapon> _Weapons;

        ///<summary>Returns this ISnapshots SMRI</summary>
        public uint Smri { get { return _Smri; } }

        void Start() {
            //From load startup
            if (Common.Instance.WorldLoader.FromLoad) { return; }

            //Normal startup
            _MaxItems = UnityEngine.Random.Range(0, 20);
        }

        /// <summary>
        /// Adds the passed weapon in the inventory
        /// </summary>
        /// <param name="_weapon">The weapon to add</param>
        public void AddWeapon(Weapon _weapon) {
            _Weapons.Add(_weapon);
        }

        ///<summary>Register the inventory to the save manager and set its SMRI</summary>
        public void RegisterToSaveManager() {
            _Smri = Common.Instance.SaveManager.RegisterModel(this);
        }

        ///<summary>Dynamically called when its time to save</summary>
        public void CacheModel() {
            Common.Instance.SaveManager.CacheModel(ConstructModel());
        }

        /// <summary>
        /// Returns an ISnapshotModel with the inventory needed data.
        /// </summary>
        public ISnapshotModel ConstructModel() {
            SInventory temp = new SInventory() {
                Smri = this.Smri,
                RefSmris = new int[_Weapons.Count],
                _MaxItems = this._MaxItems,
                _Position = transform.position,
                _Rotation = transform.rotation,
            };

            //Cache the SMRIs here
            for (int i = 0; i < temp.RefSmris.Length; i++) {
                temp.RefSmris[i] = (int)_Weapons[i].Smri;
            }

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
        /// Sets the playe fields from the incoming deserialized model
        /// </summary>
        /// <param name="_model">SInventory model containing the deserialized data</param>
        public void LoadModel(ISnapshotModel _model) {
            SInventory model = (SInventory)_model;
            _MaxItems = model._MaxItems;
            transform.position = model._Position;
            transform.rotation = model._Rotation;
        }

        /// <summary>
        /// Sets any reference the inventory may have, like its weapons.
        /// </summary>
        public void RetrieveReferences(int[] _refSmris) {
            _Weapons = new List<Weapon>();
            for (int i = 0; i < _refSmris.Length; i++) {
                _Weapons.Add((Weapon)Common.Instance.SaveManager.Snapshots[_refSmris[i]]);
            }
        }

        /// <summary>
        /// Returns the type of the ISnapshotModel this ISnapshot's data get represented.
        /// </summary>
        public Type GetSnapshotModelType() {
            return typeof(SInventory);
        }
    }
}
