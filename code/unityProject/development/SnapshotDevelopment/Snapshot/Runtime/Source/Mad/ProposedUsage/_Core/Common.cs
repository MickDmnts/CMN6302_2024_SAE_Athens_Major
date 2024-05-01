using UnityEngine;

namespace ProposedArchitecture {

    /// <summary>
    /// @TODO: Summary
    /// </summary>
    [DefaultExecutionOrder(-500)]
    public class Common : MonoBehaviour {
        ///<summary>@TODO: Summary</summary>
        public static Common Instance;

        ///<summary>@TODO: Summary</summary>
        SaveManager _SaveManager;

        ///<summary>@TODO: Summary</summary>
        public SaveManager SaveManager => _SaveManager;

        void Awake() {
            Instance = this;
            _SaveManager = new SaveManager(this);
        }

        void OnDestroy() {
            Debug.Log($"Save Manager cleanup result: {SaveManager.Cleanup()}");
        }
    }
}
