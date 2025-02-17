using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField, Header("キャラのヒットポイント")] private int _HP;

    [SerializeField, Header("キャラの攻撃力")] private int _attack;

    [SerializeField, Header("キャラの速度")] private int _speed;

    public int HP => _HP;

    public int Attack => _attack;

    public int Speed => _speed;

    /// <summary>
    /// パラメータを変化させる関数
    /// </summary>
    /// <param name="HPPlus">HPの変化する値</param>
    /// <param name="attackPlus">攻撃力の変化する値</param>
    /// <param name="speedPlus">速度の変化する値</param>
    public void StatusAffecting(int HPPlus, int attackPlus, int speedPlus)
    {
        _HP += HPPlus;
        _attack += attackPlus;
        _speed += speedPlus;
    }

    public virtual void TurnChange()
    {
        Debug.Log("ターンが回ってきました");
        TurnManager.Instance.GoNextTurn(Speed);
    }
}
