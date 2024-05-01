using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProposedArchitecture {

    ///<summary>@TODO: Summary</summary>
    public class Inventory : MonoBehaviour, ISnapshot {
        ///<summary>@TODO: Summary</summary>
        [SerializeField] uint _Smri;
        ///<summary>@TODO: Summary</summary>
        [SerializeField] int _MaxItems;
        ///<summary>@TODO: Summary</summary>
        [SerializeField] List<Weapon> _Weapons;

        ///<summary>@TODO: Summary</summary>
        public uint Smri { get { return _Smri; } }

        void Start() {
            if (Common.Instance.WorldLoader.FromLoad) { return; }

            _MaxItems = UnityEngine.Random.Range(0, 20);
        }

        /// <summary>
        /// @TODO: Summary
        /// </summary>
        /// <param name="_weapon"></param>
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
                _MaxItems = this._MaxItems,
                _Position = transform.position,
                _Rotation = transform.rotation,
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

        public void LoadModel(ISnapshotModel _model) {
            SInventory model = (SInventory)_model;
            _MaxItems = model._MaxItems;
            transform.position = model._Position;
            transform.rotation = model._Rotation;
        }

        public void RetrieveReferences(int[] _refSmris) {
            _Weapons = new List<Weapon>();
            for (int i = 0; i < _refSmris.Length; i++) {
                _Weapons.Add((Weapon)Common.Instance.SaveManager.Snapshots[_refSmris[i]]);
            }
        }

        public Type GetSnapshotModelType() {
            return typeof(SInventory);
        }
    }
}
