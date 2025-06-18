using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //TODO:プレイヤーの入力処理を行う

    [SerializeField,Header("プレイヤーの移動時間")]
    private float _playerMoveTime = 1.0f;

    private Transform _playerTransform;

    private PlayerBase _playerBase;
    private CharacterMove _playerMove = new();

    /// <summary>プレイヤーのベースクラスを取得</summary>
    /// <param name="playerBase">取得したいベースクラス</param>
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
