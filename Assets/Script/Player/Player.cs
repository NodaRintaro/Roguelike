using Cysharp.Threading.Tasks;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private int _moveDelay = 100;

    private PlayerStatu _playerStatu;

    private PlayerMove _playerMove;
    private PlayerAttack _playerAttack;
    private TurnManager _turnManager;

    private int _angle;

    private void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerAttack = GetComponent<PlayerAttack>();
        _turnManager = FindFirstObjectByType<TurnManager>();
    }

    private async UniTask Update()
    {
        switch(_playerStatu)
        {
            //ÉvÉåÉCÉÑÅ[ÇÃà⁄ìÆèàóù
            case PlayerStatu.CanMove:
                #region
                if (Input.GetKey(KeyCode.W))
                {
                    ChangeStatu(PlayerStatu.Standby);

                    _angle = 0;
                    _playerMove.MoveRotate(_angle);

                    await UniTask.Delay(_moveDelay);
                    await _playerMove.GridMove(Vector3.forward);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    ChangeStatu(PlayerStatu.Standby);

                    _angle = 180;
                    _playerMove.MoveRotate(_angle);

                    await UniTask.Delay(_moveDelay);
                    ChangeStatu(PlayerStatu.Standby);
                    await _playerMove.GridMove(Vector3.back);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    ChangeStatu(PlayerStatu.Standby);

                    _angle = 270;
                    _playerMove.MoveRotate(_angle);

                    await UniTask.Delay(_moveDelay);
                    await _playerMove.GridMove(Vector3.left);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    ChangeStatu(PlayerStatu.Standby);

                    _angle = 90;
                    _playerMove.MoveRotate(_angle);

                    await UniTask.Delay(_moveDelay);
                    await _playerMove.GridMove(Vector3.right);
                }
                #endregion
                break;
        }
    }

    public void ChangeStatu(PlayerStatu playerStatu)
    {
        _playerStatu = playerStatu;
    }

    public override void TurnChange()
    {
        _playerStatu = PlayerStatu.CanMove;
    }

    public override void TurnEnd()
    {
        _turnManager.GoNextTurn(this);
    }

    public enum PlayerStatu
    {
        Standby,
        CanMove,
        CanAttack,
    }
}
