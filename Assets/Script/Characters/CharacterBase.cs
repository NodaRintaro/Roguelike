using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField, Header("体力")] private int _hp;

    [SerializeField, Header("攻撃力")] private int _attack;

    [SerializeField, Header("防御力")] private int _defense;

    [SerializeField, Header("行動速度")] private int _speed;

    [SerializeField, Header("行動速度")] private int _hitPoint;

    public int Speed => _speed;

    public int HP => _hp;

    public virtual void TurnChange()
    {
        //このキャラクターのターンになったら呼ばれる
    }

    public virtual void TurnEnd() 
    {
        //ターン終了時に呼び出す
    }
}
