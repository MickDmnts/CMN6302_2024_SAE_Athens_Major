using System.Collections.Generic;
using ProposedArchitecture;
using UnityEngine;

public class DummyInstanceCreator : MonoBehaviour {
    public int _InstancesToCreate;
    public GameObject _InventoryPrefab;
    public Inventory _Inventory;
    public GameObject _GameObject;
    public List<Weapon> _Weapons;

    void Start() {
        _InventoryPrefab = Instantiate(_InventoryPrefab);
        _Inventory = _InventoryPrefab.GetComponent<Inventory>();
        GameObject temp = null;
        for (int i = 0; i < _InstancesToCreate; i++) {
            temp = Instantiate(_GameObject);
            temp.GetComponent<Weapon>().RegisterToSaveManager();
            _Weapons.Add(temp.GetComponent<Weapon>());
            _Weapons[i].SetInventory(_Inventory);
        }
    }
}
