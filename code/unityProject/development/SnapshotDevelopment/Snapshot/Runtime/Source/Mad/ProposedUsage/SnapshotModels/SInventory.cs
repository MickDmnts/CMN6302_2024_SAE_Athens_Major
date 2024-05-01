using MessagePack;

namespace ProposedArchitecture {

    [MessagePackObject]
    public struct SInventory : ISnapshotModel {
        [Key(0)]
        public uint Smri { get; set; }
        [Key(1)]
        public int[] RefSmris { get; set; }
        [Key(2)]
        public int _ItemCount;
    }
}
