using UnityEngine;
using MessagePack;

namespace ProposedArchitecture {
    [MessagePackObject]
    public struct SPlayer : ISnapshotModel {
        [Key(0)]
        public uint Smri { get; set; }
        [Key(1)]
        public int[] RefSmris { get; set; }
        [Key(2)]
        public float _Health;
        [Key(3)]
        public float _Stamina;
        [Key(4)]
        public float _Shield;
        [Key(5)]
        public bool _IsAlive;
        [Key(6)]
        public Vector3 _Position;
        [Key(7)]
        public Quaternion _Rotation;
    }
}
