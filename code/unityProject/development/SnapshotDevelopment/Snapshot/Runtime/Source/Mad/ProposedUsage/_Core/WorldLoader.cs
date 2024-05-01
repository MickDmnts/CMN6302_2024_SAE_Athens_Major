using UnityEngine;

namespace ProposedArchitecture {
    
    /// <summary>
    /// @TODO: Summary
    /// </summary>
    [DefaultExecutionOrder(-250)]
    public class WorldLoader : MonoBehaviour {
        ///<summary>@TODO: Summary</summary>
        [SerializeField] string _SaveFileName;
        ///<summary>@TODO: Summary</summary>
        [SerializeField] bool _FromLoad;
        ///<summary>@TODO: Summary</summary>
        [SerializeField] GameObject[] _Objects;

        public bool FromLoad => _FromLoad;

        void Start() {
            if (!_FromLoad) {
                DefaultStart();
            } else {
                FromLoadStart();
            }
        }

        ///<summary>@TODO: Summary</summary>
        void DefaultStart() {
            GameObject temp;
            for (int i = 0; i < _Objects.Length; i++) {
                temp = Instantiate(_Objects[i], _Objects[i].transform.position, _Objects[i].transform.rotation);
                if (temp.TryGetComponent<ISnapshot>(out ISnapshot snapshot)) {
                    snapshot.RegisterToSaveManager();
                }
            }
        }

        ///<summary>@TODO: Summary</summary>
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
