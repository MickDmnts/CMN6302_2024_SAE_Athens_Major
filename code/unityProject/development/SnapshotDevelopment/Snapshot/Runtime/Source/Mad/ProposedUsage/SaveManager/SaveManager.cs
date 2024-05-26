using System;
using System.Collections.Generic;
using System.IO;
using Snapshot;

namespace ProposedArchitecture {

    /// <summary>
    /// Responsible for handling the SnapshotWrapper, ISnapshot and ISnapshotModel instances.
    /// Hands out SMRIs.
    /// </summary>
    public class SaveManager {
        ///<summary>Reference to the Common instance</summary>
        Common _Common;
        ///<summary>Caches the ISnapshot references of the game</summary>
        List<ISnapshot> _Snapshots;
        ///<summary>Caches the ISnapshotModel references of the ISnapshots</summary>
        List<ISnapshotModel> _Models;

        ///<summary>Returns a read only collection of the Snapshot reference cache.</summary>
        public IReadOnlyList<ISnapshot> Snapshots => _Snapshots;

        ///<summary>Register to this event to get notified before the packing sequence starts.</summary>
        event Action OnSnapshotStart;
        ///<summary>Raises the OnSnapshotStart event</summary>
        void RaiseOnSnapshotStart() {
            OnSnapshotStart?.Invoke();
        }

        /// <summary>
        /// Creates a SaveManager instance
        /// </summary>
        /// <param name="_common">Reference to the Common instance</param>
        public SaveManager(Common _common) {
            this._Common = _common;
            this._Snapshots = new List<ISnapshot>();
            this._Models = new List<ISnapshotModel>();

            //Set the initial save directory
            SnapshotWrapper.SetSavePath(Path.Combine(GlobalProperties.SavePath, GlobalProperties.SaveFolderName));
        }

        /// <summary>
        /// Registers the passed ISnapshot for data caching upon packing.
        /// </summary>
        /// <param name="_snapshot">The snapshot instance</param>
        void RegisterToSnapshot(ISnapshot _snapshot) {
            OnSnapshotStart += _snapshot.CacheModel;
        }

        /// <summary>
        /// Unregisters the passed ISnapshot from data caching upon packing.
        /// The passed snapshot is also removed from the Snapshot list.
        /// </summary>
        /// <param name="_snapshot"></param>
        public void UnregisterFromSnapshot(ISnapshot _snapshot) {
            OnSnapshotStart -= _snapshot.CacheModel;
            _Snapshots.Remove(_snapshot);
        }

        /// <summary>
        /// Registers the passed ISnapshot instance to the serialization event handler and adds it to the ISnapshot reference list.
        /// </summary>
        /// <param name="_snapshot">The snapshot instance</param>
        /// <returns>The SMRI of the registered model.</returns>
        public uint RegisterModel(ISnapshot _snapshot) {
            _Snapshots.Add(_snapshot);
            RegisterToSnapshot(_snapshot);

            return SnapshotWrapper.GetSmri();
        }

        ///<summary>
        /// Kicks off the packing sequence. All the cached ISnapshot.CacheData methods get called and 
        /// their data are serialized and passed to the internal DLL library cache.
        /// The cached SaveManager models list gets cleared afterwards.
        ///</summary>
        public void Save() {
            RaiseOnSnapshotStart();

            byte[] bytes;
            int[] refSmris;

            for (int i = 0; i < _Models.Count; i++) {
                bytes = MessagePack.MessagePackSerializer.Serialize<object>(_Models[i]);
                refSmris = _Models[i].RefSmris;

                SnapshotWrapper.CacheData(_Models[i].Smri, bytes, refSmris);
            }

            _Models = new List<ISnapshotModel>();

            SnapshotWrapper.PackData();
        }

        /// <summary>
        /// Stores the passed model to the SaveManager data container list.
        /// </summary>
        /// <param name="_model">The snapshot data container instance</param>
        public void CacheModel(ISnapshotModel _model) {
            _Models.Add(_model);
        }

        /// <summary>
        /// Kicks off the Unpacking mechanism contained in the passed fileName.
        /// The deserialized data are stored inside the SaveManager ISnapshotModel cache for accessing.
        /// Each cached ISnapshot.LoadModel and ISnapshot.RetrieveReferences method gets called after each data retrieval from the dll cache. 
        /// </summary>
        /// <param name="_fileName"></param>
        public void LoadSaveFile(string _fileName) {
            if (SnapshotWrapper.SetLoadFileName(_fileName)) {
                SnapshotWrapper.UnpackData();

                byte[] bytes;
                ISnapshotModel model;
                foreach (ISnapshot snapshot in _Snapshots) {
                    bytes = SnapshotWrapper.GetData(snapshot.Smri);
                    model = (ISnapshotModel)MessagePack.MessagePackSerializer.Deserialize(snapshot.GetSnapshotModelType(), bytes);
                    snapshot.LoadModel(model);
                }

                foreach (ISnapshot snapshot in _Snapshots) {
                    snapshot.RetrieveReferences(SnapshotWrapper.GetRefSmris(snapshot.Smri));
                }
            }
        }

        /// <summary>
        /// Resets the dll library caches and SMRI.
        /// </summary>
        public bool Cleanup() {
            return SnapshotWrapper.ResetCache() && SnapshotWrapper.ResetSmri();
        }
    }
}
