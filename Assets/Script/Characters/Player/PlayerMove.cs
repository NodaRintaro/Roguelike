using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.SocialPlatforms;
using static PlayerContrller;

public class PlayerMove : MonoBehaviour
{
    [SerializeField,Header("プレイヤーの位置")]
    private Vector3 _playerPos;

    [SerializeField,Header("移動速度")]
    private float _playerSpeed = 5f;

    private PlayerContrller _player;

    private Ray _ray;
    private RaycastHit _hitDontWalkTile;

    private Animator _animator;

    private void Start()
    {
        _player = GetComponent<PlayerContrller>();
        _animator = GetComponent<Animator>();
        _playerPos = this.transform.position;
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

    public async UniTask GridMove(Vector3 moveVec)
    {
        if (!Physics.Raycast(_ray, out _hitDontWalkTile, MapGenerator.GridSize))
        {
            _playerPos += new Vector3(moveVec.x * MapGenerator.GridSize, 0, moveVec.z * MapGenerator.GridSize);
            await UniTask.WaitUntil(() => _playerPos == transform.position);
            _player.TurnEnd();
        }
        else
        {
            _player.ChangeStatu(PlayerStatu.CanMove);
            Debug.Log("進めないよ");
        }
    }
}
