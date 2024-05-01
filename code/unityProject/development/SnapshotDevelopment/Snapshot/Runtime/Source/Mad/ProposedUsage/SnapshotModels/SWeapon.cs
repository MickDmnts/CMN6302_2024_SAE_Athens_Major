using MessagePack;
using UnityEngine;

namespace ProposedArchitecture {

    [MessagePackObject]
    public struct SWeapon : ISnapshotModel {
        [Key(0)]
        public uint Smri { get; set; }
        [Key(1)]
        public int[] RefSmris { get; set; }
        [Key(2)]
        public int _Ammo;
        [Key(3)]
        public bool _Loaded;
        [Key(4)]
        public Vector3 _Position;
        [Key(5)]
        public Quaternion _Rotation;
    }
}
