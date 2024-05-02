using UnityEngine;

namespace ProposedArchitecture {

    /// <summary>
    /// Handles the world creation and order of SMRIs simultaneously.
    /// </summary>
    [DefaultExecutionOrder(-250)]
    public class WorldLoader : MonoBehaviour {
        ///<summary>The save file name to load from</summary>
        [SerializeField, Tooltip("The save file name to load from")] string _SaveFileName;
        ///<summary>Is the start-up a load procedure?</summary>
        [SerializeField, Tooltip("Is the start-up a load procedure?")] bool _FromLoad;
        ///<summary>The objects to load everytime</summary>
        [SerializeField, Tooltip("The objects to load everytime")] GameObject[] _Objects;

        ///<summary>Is the start-up a load procedure?</summary>
        public bool FromLoad => _FromLoad;

        void Start() {
            if (!_FromLoad) {
                DefaultStart();
            } else {
                FromLoadStart();
            }
        }

        ///<summary>Loads the game objects normally</summary>
        void DefaultStart() {
            GameObject temp;
            for (int i = 0; i < _Objects.Length; i++) {
                temp = Instantiate(_Objects[i], _Objects[i].transform.position, _Objects[i].transform.rotation);
                if (temp.TryGetComponent<ISnapshot>(out ISnapshot snapshot)) {
                    snapshot.RegisterToSaveManager();
                }
            }
        }

        ///<summary>Loads the objects and then calls the SaveManager unpacking method.</summary>
        void FromLoadStart() {
            GameObject temp;
            for (int i = 0; i < _Objects.Length; i++) {
                temp = Instantiate(_Objects[i], _Objects[i].transform.position, _Objects[i].transform.rotation);
                if (temp.TryGetComponent<ISnapshot>(out ISnapshot snapshot)) {
                    snapshot.RegisterToSaveManager();
                }
            }
            Common.Instance.SaveManager.LoadSaveFile(_SaveFileName);
        }
    }
}
