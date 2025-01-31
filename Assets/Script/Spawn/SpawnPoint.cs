using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField,Header("PlayerObject")] GameObject _playerObject;

    [SerializeField,Header("MapCreate‚ÌObject‚ð“ü‚ê‚é")] private GameObject _mapCreateObject;

    private MapCreate _mapCreate;

    private void Start()
    {
        _mapCreate = _mapCreateObject.GetComponent<MapCreate>();
    }

    
}
