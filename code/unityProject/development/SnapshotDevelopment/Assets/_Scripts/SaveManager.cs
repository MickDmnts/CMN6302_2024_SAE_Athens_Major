using UnityEngine;

public class SaveManager : MonoBehaviour {
    public bool _Save;

    void Update() {
        if (_Save) {
            _Save = false;

            int num = SnapshotWrapper.GetNum();
            Debug.Log(num);
        }
    }
}
