using UnityEngine;

public class EnemyBase : ICharacter
{
    /// <summary>プレイヤーのステータス</summary>
    private Status _enemyStatus = default;

    [SerializeField, Header("キャラのオブジェクト")] private GameObject _characterObject;

    public Status CharacterStatus => _enemyStatus;

    public GameObject CharacterObject => _characterObject;

    public  MoveState CharacterState {  get; private set; }

    /// <summary>キャラクタの初期化</summary>
    /// <param name="characterData">キャラクタのデータ</param>
    /// <param name="characterObject">キャラクタのオブジェクト</param>
    public void InitCharacterData(CharacterData characterData, GameObject characterObject)
    {
        //生成したキャラクターのオブジェクトを初期化
        SetEnemyObj(characterObject);

        //ステータスの初期化
        SetStatus(characterData);
    }

    /// <summary>ステータスを設定</summary>
    /// <param name="characterData">キャラクタのData</param>
    private void SetStatus(CharacterData charaData)
    {
        _enemyStatus = new();
        _enemyStatus.InitStatus(charaData.BaseHP, charaData.BaseMP, charaData.BaseAttack, charaData.BaseDefense, charaData.BaseSpeed);
    }

    /// <summary>キャラクタのObjectを初期化</summary>
    /// <param name="playerObject">キャラクタのオブジェクト</param>
    private void SetEnemyObj(GameObject enemyObject)
    {
        _characterObject = enemyObject;
    }

    public void StartAction()
    {
        Debug.Log("行動します");
        FinishAction();
    }

    public void FinishAction()
    {
        CharacterState = MoveState.Stay;
        Debug.Log("行動終了");
    }
}
