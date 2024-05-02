using ProposedArchitecture;
using UnityEngine;

/// <summary>
/// Test class from early stages.
/// </summary>
public class DummySaveInitiator : MonoBehaviour {
    public bool _Save;

    void Update() {
        if (_Save) {
            _Save = false;

            Common.Instance.SaveManager.Save();
        }
    }
}
