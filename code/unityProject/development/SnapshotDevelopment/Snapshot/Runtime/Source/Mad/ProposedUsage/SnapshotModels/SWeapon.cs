using MessagePack;

namespace ProposedArchitecture {

    [MessagePackObject]
    public struct SWeapon : ISnapshotModel {
        [Key(0)]
        public uint Smri { get; set; }
        [Key(1)]
        public int[] RefSmris { get; set; }
    }
}