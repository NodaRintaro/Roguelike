using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    [SerializeField,Header("プレイヤーの位置")]
    private Vector3 _playerPos;

    [SerializeField,Header("移動速度")]
    private float _playerSpeed = 5f;

    private MapGenerator _mapGenerator;
    private Character _playerData;
    private TurnManager _turnManager;

    private Ray _ray;
    private RaycastHit _hitDontWalkTile;

    private Animator _animator;

    private int _gridSize;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerPos = this.transform.position;
        _mapGenerator = FindFirstObjectByType<MapGenerator>();
        _playerData = GetComponent<Character>();
        _turnManager = FindFirstObjectByType<TurnManager>();
        _gridSize = _mapGenerator.GridSize;
    }

    private void Update()
    {
        _ray = new Ray(_playerPos, transform.forward);

        if (this.transform.position != _playerPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerPos, _playerSpeed * Time.deltaTime);
            _animator.SetBool("IsWalk", true);
        }
        else
        {
            _animator.SetBool("IsWalk", false);
        }
    }

    public void MoveRotate(int angle)
    {
        transform.eulerAngles = new Vector3(0, angle, 0);
    }

    public void GridMove(Vector3 moveVec)
    {
        if (!Physics.Raycast(_ray, out _hitDontWalkTile, _gridSize))
        {
            _playerPos += new Vector3(moveVec.x * _gridSize, 0, moveVec.z * _gridSize);
            _turnManager.GoNextTurn(_playerData);
            _playerData.TurnChange();
        }
        else
        {
            Debug.Log("進めないよ");
        }
    }
}
