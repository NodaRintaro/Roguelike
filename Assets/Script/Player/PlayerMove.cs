using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField,Header("プレイヤーの位置")]
    private Vector3 _playerPos;

    private MapGenerator _mapGenerator;

    private Player _player;

    private void Start()
    {
        _playerPos = this.transform.position;
        _mapGenerator = GetComponent<MapGenerator>();
        _player = GetComponent<Player>();
    }

    public void GridMove(Vector3 moveVec)
    {
        _playerPos += moveVec * _mapGenerator.GridSize;
    }
}
