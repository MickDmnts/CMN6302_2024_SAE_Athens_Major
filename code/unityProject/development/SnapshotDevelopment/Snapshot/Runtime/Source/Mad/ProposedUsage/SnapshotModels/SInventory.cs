using MessagePack;
using UnityEngine;

namespace ProposedArchitecture {

    [MessagePackObject]
    public struct SInventory : ISnapshotModel {
        [Key(0)]
        public uint Smri { get; set; }
        [Key(1)]
        public int[] RefSmris { get; set; }
        [Key(2)]
        public int _MaxItems;
        [Key(3)]
        public Vector3 _Position;
        [Key(4)]
        public Quaternion _Rotation;
    }
}
