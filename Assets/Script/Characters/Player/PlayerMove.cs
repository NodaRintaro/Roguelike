using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerMove : MonoBehaviour
{
    [SerializeField,Header("プレイヤーの移動先の位置")]
    private Vector3 _targetMovePosition;

    [SerializeField,Header("移動速度")]
    private float _playerMoveSpeed = 5f;

    private Ray _ray;
    private RaycastHit _hitDontWalkTile;

    private Animator _playerAnimator;

    private void Start()
    {
        _playerAnimator = GetComponent<Animator>();
        _targetMovePosition = this.transform.position;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (this.transform.position != _targetMovePosition)
        {
            _ray = new Ray(this.transform.position, transform.forward);
            transform.position = Vector3.MoveTowards(transform.position, _targetMovePosition, _playerMoveSpeed * Time.deltaTime);
            _playerAnimator.SetBool("IsWalk", true);
        }
        else
        {
            _playerAnimator.SetBool("IsWalk", false);
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
            _targetMovePosition += new Vector3(moveVec.x * MapGenerator.GridSize, 0, moveVec.z * MapGenerator.GridSize);
            await UniTask.WaitUntil(() => _targetMovePosition == transform.position);
        }
        else
        {
            Debug.Log("進めないよ");
        }
    }
}
