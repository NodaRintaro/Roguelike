using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterData _playerData;
    private PlayerMove _playerMove;
    private PlayerAttack _playerAttack;
    private TurnManager _turnManager;

    private void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerData = GetComponent<CharacterData>();
        _turnManager = FindAnyObjectByType<TurnManager>();
    }

    private void Update()
    {
        if (_playerData.CanMove)
        {
            if (Input.GetKey(KeyCode.W))
            {
                _playerMove.GridMove(Vector3.forward);
                Turnend();
                _turnManager.GoNextTurn(_playerData);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _playerMove.GridMove(Vector3.back);
                Turnend();
                _turnManager.GoNextTurn(_playerData);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _playerMove.GridMove(Vector3.left);
                Turnend();
                _turnManager.GoNextTurn(_playerData);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _playerMove.GridMove(Vector3.right);
                Turnend();
                _turnManager.GoNextTurn(_playerData);
            }
        }
    }

    public void Turnend()
    {
        _playerData.TurnChange();
    }
}
