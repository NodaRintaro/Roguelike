using UnityEngine;

public class Character : MonoBehaviour 
{
    [SerializeField] private int _HP;

    [SerializeField] private int _speed;

    [SerializeField] private int _power;

    public int HP => _HP;

    public int Speed => _speed;
    
    public int Power => _power;
    
    public void Damage(int hp)
    {
        _HP += hp;
    }
    
    public virtual void TurnChange()
    {
        //このキャラクターのターンになったら呼ばれる
    }

    public virtual void TurnEnd() 
    {
        //ターン終了時に呼び出す
    }
}
