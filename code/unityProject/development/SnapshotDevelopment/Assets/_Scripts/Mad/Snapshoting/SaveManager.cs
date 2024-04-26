using UnityEngine;
using MessagePack;

[MessagePackObject]
public struct ModelDummy {
    [Key(0)]
    public uint _Smri;
    [Key(1)]
    public bool _State;
    [Key(2)]
    public string _Sentence;
}

/// <summary>
/// @TODO: Summary
/// </summary>
public class SaveManager : MonoBehaviour {
    //@TODO: Summary
}
