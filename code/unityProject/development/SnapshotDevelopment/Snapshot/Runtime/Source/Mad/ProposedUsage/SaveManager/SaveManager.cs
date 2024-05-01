using System;
using System.Collections.Generic;
using System.IO;
using Snapshot;

namespace ProposedArchitecture {

    /// <summary>
    /// @TODO: Summary
    /// </summary>
    public class SaveManager {
        ///<summary>@TODO: Summary</summary>
        Common _Common;

        ///<summary>@TODO: Summary</summary>
        List<ISnapshotModel> _Models;

        ///<summary>@TODO: Summary</summary>
        event Action OnSnapshotStart;
        ///<summary>@TODO: Summary</summary>
        void RaiseOnSnapshotStart() {
            OnSnapshotStart?.Invoke();
        }

        /// <summary>
        /// @TODO: Summary
        /// </summary>
        /// <param name="_common"></param>
        public SaveManager(Common _common) {
            this._Common = _common;
            this._Models = new List<ISnapshotModel>();

            SnapshotWrapper.SetSavePath(Path.Combine(GlobalProperties.SavePath, GlobalProperties.SaveFolderName));
        }

        /// <summary>
        /// @TODO: Summary
        /// </summary>
        /// <param name="_snapshot"></param>
        void RegisterToSnapshot(ISnapshot _snapshot) {
            OnSnapshotStart += _snapshot.CacheModel;
        }

        /// <summary>
        /// @TODO: Summary
        /// </summary>
        /// <param name="_snapshot"></param>
        public void UnregisterFromSnapshot(ISnapshot _snapshot) {
            OnSnapshotStart -= _snapshot.CacheModel;
        }

        /// <summary>
        /// @TODO: Summary
        /// </summary>
        /// <param name="_snapshot"></param>
        /// <returns></returns>
        public uint RegisterModel(ISnapshot _snapshot) {
            RegisterToSnapshot(_snapshot);

            return SnapshotWrapper.GetSmri();
        }

        ///<summary>@TODO: Summary</summary>
        public void Save() {
            RaiseOnSnapshotStart();

            byte[] bytes;
            int[] refSmris;

            for (int i = 0; i < _Models.Count; i++) {
                bytes = MessagePack.MessagePackSerializer.Serialize<object>(_Models[i]);
                refSmris = _Models[i].RefSmris;

                SnapshotWrapper.CacheData(_Models[i].Smri, bytes.Length, bytes, refSmris, _Models[i].RefSmris.Length);
            }

            _Models = new List<ISnapshotModel>();

            SnapshotWrapper.PackData();
        }

        /// <summary>
        /// @TODO: Summary
        /// </summary>
        /// <param name="_model"></param>
        public void CacheModel(ISnapshotModel _model) {
            _Models.Add(_model);
        }

        /// <summary>
        /// @TODO: Summary
        /// </summary>
        public bool Cleanup() {
            return SnapshotWrapper.ResetCache() && SnapshotWrapper.ResetSmri();
        }
    }
}
