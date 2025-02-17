using UnityEngine;

public class Player : Character
{
    [SerializeField,Header("åªç›ìÆÇØÇÈÇ©Ç«Ç§Ç©")]
    private bool _onTrun = false;

    private PlayerMove _playerMove;

    private PlayerAttack _playerAttack;

    private void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        if (_onTrun)
        {
            if (Input.GetKey(KeyCode.W))
            {
                _playerMove.GridMove(Vector3.forward);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _playerMove.GridMove(Vector3.back);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _playerMove.GridMove(Vector3.left);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _playerMove.GridMove(Vector3.right);
            }
        }
    }

    public override void TurnChange()
    {
        if (_onTrun)
        {
            _onTrun = false;
            TurnManager.Instance.GoNextTurn(Speed);
        }
        else
        {
            _onTrun = true;
        }   
    }
}
