using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField,Header("プレイヤーの位置")]
    private Vector3 _playerPos;

    private MapGenerator _mapGenerator;

    private void Start()
    {
        _playerPos = this.transform.position;
        _mapGenerator = FindFirstObjectByType<MapGenerator>();
    }

    public void GridMove(Vector3 moveVec)
    {
        int gridSize = _mapGenerator.GridSize;
        _playerPos += new Vector3(moveVec.x * gridSize,0,moveVec.z * gridSize);
        this.transform.position = _playerPos;
    }
}
