using System;

namespace ProposedArchitecture {
    
    /// <summary>
    /// Stores global game values and constants
    /// </summary>
    public static class GlobalProperties {
        ///<summary>The save directory absolute path</summary>
        public static readonly string SavePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        ///<summary>The save folder name.</summary>
        public const string SaveFolderName = "SnapshotSaves";
    }
}
