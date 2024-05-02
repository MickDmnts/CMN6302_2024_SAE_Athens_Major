using System;
using UnityEngine;

namespace ProposedArchitecture {

    public class Player : MonoBehaviour, ISnapshot {
        ///<summary>Player health</summary>
        [SerializeField, Tooltip("Player health")] float _Health;
        ///<summary>Player stamina</summary>
        [SerializeField, Tooltip("Player stamina")] float _Stamina;
        ///<summary>Player shield value</summary>
        [SerializeField, Tooltip("Player shield value")] float _Shield;
        ///<summary>Is the player alive?</summary>
        [SerializeField, Tooltip("Is the player alive?")] bool _IsAlive;

        ///<summary>This ISnapshots SMRI</summary>
        [Header("Set Dynamically")]
        [SerializeField, Tooltip("This ISnapshots SMRI")] uint _Smri;

        ///<summary>Reference to the Player Inventory</summary>
        Inventory _Inventory;

        ///<summary>Returns this ISnapshots SMRI</summary>
        public uint Smri { get { return _Smri; } }

        void Start() {
            //From load startup
            if (Common.Instance.WorldLoader.FromLoad) { return; }

            //Normal startup
            _Inventory = FindObjectOfType<Inventory>();
            Weapon[] temp = FindObjectsOfType<Weapon>();
            for (int i = 0; i < temp.Length; i++) {
                temp[i].SetInventory(_Inventory);
                _Inventory.AddWeapon(temp[i]);
            }
        }

        ///<summary>Register the player to the save manager and set its SMRI</summary>
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
            SPlayer temp = new SPlayer() {
                Smri = this.Smri,
                //Cache the SMRIs here
                RefSmris = new int[]{
                    (int)_Inventory.Smri,
                },
                _Health = this._Health,
                _Stamina = this._Stamina,
                _Shield = this._Shield,
                _IsAlive = this._IsAlive,
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
        /// Sets the player fields from the incoming deserialized model
        /// </summary>
        /// <param name="_model">SPlayer model containing the deserialized data</param>
        public void LoadModel(ISnapshotModel _model) {
            SPlayer model = (SPlayer)_model;

            _Health = model._Health;
            _Stamina = model._Stamina;
            _Shield = model._Shield;
            _IsAlive = model._IsAlive;
            transform.position = model._Position;
            transform.rotation = model._Rotation;
        }

        /// <summary>
        /// Sets any reference the player may have, like its inventory.
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
            return typeof(SPlayer);
        }
    }
}
