using System;

namespace ProposedArchitecture {

    /// <summary>
    /// Marks a class as ISnapshot-able
    /// </summary>
    public interface ISnapshot {
        ///<summary>The class should have an SMRI field</summary>
        public uint Smri { get; }

        public void RegisterToSaveManager();
        public void CacheModel();
        public ISnapshotModel ConstructModel();
        public void UnregisterToSaveManager();
        public void LoadModel(ISnapshotModel _model);
        public void RetrieveReferences(int[] _refSmris);
        public Type GetSnapshotModelType();
    }
}
