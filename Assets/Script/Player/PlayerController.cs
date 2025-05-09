using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerState _playerState;

    private PlayerMove _playerMove;
    private PlayerAttack _playerAttack;

    private int _angle;

    private void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    private async UniTask Update()
    {
        if (_playerState == PlayerState.Move)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _angle = 0;
                _playerMove.MoveRotate(_angle);

                await UniTask.Delay(100);
                _playerMove.GridMove(Vector3.forward);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                _angle = 180;
                _playerMove.MoveRotate(_angle);

                await UniTask.Delay(100);
                _playerMove.GridMove(Vector3.back);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                _angle = 270;
                _playerMove.MoveRotate(_angle);

                await UniTask.Delay(100);
                _playerMove.GridMove(Vector3.left);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _angle = 90;
                _playerMove.MoveRotate(_angle);

                await UniTask.Delay(100);
                _playerMove.GridMove(Vector3.right);
            }
        }
    }

    public enum PlayerState
    {
        Idle,
        Move,
        Attack
    }
}
