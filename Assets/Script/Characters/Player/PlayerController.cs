using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //TODO:�v���C���[�̓��͏������s��

    [SerializeField,Header("�v���C���[�̈ړ�����")]
    private float _playerMoveTime = 1.0f;

    private Transform _playerTransform;

    private PlayerBase _playerBase;
    private CharacterMove _playerMove = new();

    /// <summary>�v���C���[�̃x�[�X�N���X���擾</summary>
    /// <param name="playerBase">�擾�������x�[�X�N���X</param>
    public void GetPlayerBase(PlayerBase playerBase) 
    {
        _playerBase = playerBase;
    }

    void Start()
    {
        _playerTransform = this.transform;
    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        _playerMove.MoveCharacter(callbackContext.ReadValue<Vector2Int>(),_playerTransform,_playerMoveTime);
        _playerBase.FinishAction();
    }
}
