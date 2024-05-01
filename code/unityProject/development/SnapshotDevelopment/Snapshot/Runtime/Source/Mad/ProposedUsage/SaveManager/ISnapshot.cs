using System;

namespace ProposedArchitecture {

    /// <summary>
    /// @TODO: Summary
    /// </summary>
    public interface ISnapshot {
        ///<summary>@TODO: Summary</summary>
        public uint Smri { get; }

        ///<summary>@TODO: Summary</summary>
        public void RegisterToSaveManager();

        ///<summary>@TODO: Summary</summary>
        public void CacheModel();

        ///<summary>@TODO: Summary</summary>
        public ISnapshotModel ConstructModel();

        ///<summary>@TODO: Summary</summary>
        public void UnregisterToSaveManager();

        ///<summary>@TODO: Summary</summary>
        public void LoadModel(ISnapshotModel _model);

        ///<summary>@TODO: Summary</summary>
        public void RetrieveReferences(int[] _refSmris);

        ///<summary>@TODO: Summary</summary>
        public Type GetSnapshotModelType();
    }
}
