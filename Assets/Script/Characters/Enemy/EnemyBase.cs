using UnityEngine;

public class EnemyBase : ICharacter
{
    [SerializeField, Header("HP")] private int _hp;

    [SerializeField, Header("攻撃力")] private int _attack;

    [SerializeField, Header("防御力")] private int _defense;

    [SerializeField, Header("素早さ")] private int _speed;

    [SerializeField, Header("キャラのオブジェクト")] private GameObject _characterObject;

    public GameObject CharacterObject => _characterObject;

    public int HP => _hp;

    public int Attack => _attack;

    public int Defense => _defense;

    public int Speed => _speed;

    public  MoveState CharacterState {  get; private set; }

    public void StatusInitialize(CharacterData characterData, GameObject characterObject)
    {
        _characterObject = characterObject;
        _hp = characterData.BaseHP;
        _attack = characterData.BaseAttack;
        _defense = characterData.BaseDefense;
        _speed = characterData.BaseSpeed;
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
