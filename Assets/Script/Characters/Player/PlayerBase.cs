using UnityEngine;

public class PlayerBase : ICharacter
{
    /// <summary>プレイヤーのステータス</summary>
    private Status _playerStatus = default; 

    /// <summary>プレイヤーの操るゲームオブジェクト</summary>
    [SerializeField,Header("プレイヤーのオブジェクト")] private GameObject _characterObject;

    public Status CharacterStatus => _playerStatus;

    public GameObject CharacterObject => _characterObject;

    public MoveState CharacterState { get; private set; }

    /// <summary>プレイヤーの初期化</summary>
    /// <param name="characterData">プレイヤーのデータ</param>
    /// <param name="characterObject">プレイヤーのオブジェクト</param>
    public void InitCharacterData(CharacterData characterData, GameObject characterObject)
    {
        //生成したキャラクターのオブジェクトを初期化
        SetPlayerObj(characterObject);

        //ステータスの初期化
        SetStatus(characterData);
    }

    /// <summary>ステータスを設定</summary>
    /// <param name="characterData">プレイヤーのData</param>
    private void SetStatus(CharacterData charaData)
    {
        _playerStatus = new();
        _playerStatus.InitStatus(charaData.BaseHP, charaData.BaseMP, charaData.BaseAttack, charaData.BaseDefense, charaData.BaseSpeed);
    }

    /// <summary>プレイヤーのObjectを初期化</summary>
    /// <param name="playerObject">プレイヤーのオブジェクト</param>
    private void SetPlayerObj(GameObject playerObject)
    {
        _characterObject = playerObject; 
        PlayerController playerController = playerObject.AddComponent<PlayerController>();
        playerController.GetPlayerBase(this);
    }

    /// <summary>ターン開始時の処理</summary>
    public void StartAction()
    {
        CharacterState = MoveState.Move;
    }

    /// <summary>ターン終了時の処理</summary>
    public void FinishAction()
    {
        CharacterState = MoveState.Stay;
    }
}
