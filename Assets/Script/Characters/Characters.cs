using UnityEngine;

public class Characters : MonoBehaviour
{
    [SerializeField,Header("行動速度")]private int _speed;

    [SerializeField, Header("行動速度")] private int _hitPoint;

    public int Speed => _speed;

    public int HP => _hitPoint;

    public virtual void TurnChange()
    {
        //このキャラクターのターンになったら呼ばれる
    }

    public virtual void TurnEnd() 
    {
        //ターン終了時に呼び出す
    }
}
