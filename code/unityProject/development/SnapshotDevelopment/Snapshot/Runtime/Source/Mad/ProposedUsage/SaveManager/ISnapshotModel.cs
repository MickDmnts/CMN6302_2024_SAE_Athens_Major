namespace ProposedArchitecture {

    public interface ISnapshotModel{
        ///<summary>The class/struct should have an SMRI field</summary>
        public uint Smri {get; set;}
        ///<summary>The class/struct should have an int array to store its references SMRIs</summary>
        public int[] RefSmris {get; set;}
    }
}
