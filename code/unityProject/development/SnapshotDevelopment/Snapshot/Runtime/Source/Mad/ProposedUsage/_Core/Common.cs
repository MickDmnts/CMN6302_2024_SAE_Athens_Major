using UnityEngine;

namespace ProposedArchitecture {

    /// <summary>
    /// Manager hub
    /// </summary>
    [DefaultExecutionOrder(-500)]
    public class Common : MonoBehaviour {
        ///<summary>Returns the Common singleton</summary>
        public static Common Instance;

        ///<summary>Reference to the WorldLoader</summary>
        WorldLoader _WorldLoader;
        ///<summary>Reference to the SaveManager</summary>
        SaveManager _SaveManager;

        ///<summary>Returns the WorldLoader reference</summary>
        public WorldLoader WorldLoader => _WorldLoader;
        ///<summary>Returns the SaveManager reference</summary>
        public SaveManager SaveManager => _SaveManager;

        void Awake() {
            Instance = this;
            
            _WorldLoader = GetComponent<WorldLoader>();
            _SaveManager = new SaveManager(Instance);
        }

        //Cleanup when the game closes.
        void OnDestroy() {
            Debug.Log($"Save Manager cleanup result: {SaveManager.Cleanup()}");
        }
    }
}
