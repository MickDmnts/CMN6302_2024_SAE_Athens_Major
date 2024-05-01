using System;
using UnityEngine;

namespace ProposedArchitecture {

    ///<summary>@TODO: Summary</summary>
    public class Player : MonoBehaviour, ISnapshot {
        ///<summary>@TODO: Summary</summary>
        [SerializeField] uint _Smri;
        ///<summary>@TODO: Summary</summary>
        [SerializeField] float _Health;
        ///<summary>@TODO: Summary</summary>
        [SerializeField] float _Stamina;
        ///<summary>@TODO: Summary</summary>
        [SerializeField] float _Shield;
        ///<summary>@TODO: Summary</summary>
        [SerializeField] bool _IsAlive;

        ///<summary>@TODO: Summary</summary>
        Inventory _Inventory;

        ///<summary>@TODO: Summary</summary>
        public uint Smri { get { return _Smri; } }

        void Start() {
            if (Common.Instance.WorldLoader.FromLoad) { return; }

            _Inventory = FindObjectOfType<Inventory>();
            Weapon[] temp = FindObjectsOfType<Weapon>();
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
            SPlayer model = (SPlayer)_model;

            _Health = model._Health;
            _Stamina = model._Stamina;
            _Shield = model._Shield;
            _IsAlive = model._IsAlive;
            transform.position = model._Position;
            transform.rotation = model._Rotation;
        }

        public void RetrieveReferences(int[] _refSmris) {
            for (int i = 0; i < _refSmris.Length; i++) {
                _Inventory = (Inventory)Common.Instance.SaveManager.Snapshots[_refSmris[i]];
            }
        }

        public Type GetSnapshotModelType() {
            return typeof(SPlayer);
        }
    }
}
