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
        SaveLoader _SaveLoader;

        ///<summary>@TODO: Summary</summary>
        public SaveManager SaveManager => _SaveManager;
        ///<summary>@TODO: Summary</summary>
        public SaveLoader SaveLoader => _SaveLoader;

        void Awake() {
            Instance = this;
            _SaveManager = new SaveManager(this);
            _SaveLoader = new SaveLoader();
        }

        void OnDestroy() {
            Debug.Log($"Save Manager cleanup result: {SaveManager.Cleanup()}");
        }
    }
}
