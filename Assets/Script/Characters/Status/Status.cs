using UnityEngine;

public class Status
{
    private int _maxHp;

    private int _maxMp;

    private int _hp;

    private int _mp;

    private int _attack;

    private int _defense;

    private int _speed;

    public int MaxHP => _maxHp;

    public int MaxMP => _maxMp;

    public int HP => _hp;

    public int MP => _mp;

    public int Attack => _attack;

    public int Defense => _defense;

    public int Speed => _speed;

    /// <summary>
    /// ステータスの初期化
    /// </summary>
    /// <param name="hp">体力</param>
    /// <param name="mp">MP</param>
    /// <param name="at">攻撃力</param>
    /// <param name="df">防御力</param>
    /// <param name="sp">素早さ</param>
    public void InitStatus(int hp, int mp, int at, int df, int sp)
    {
        _maxHp = hp;
        _maxMp = mp;
        _hp = hp;
        _mp = mp;
        _attack = at;
        _defense = df;
        _speed = sp;
    }

    #region ステータスの増減処理
    /// <summary>HPの増減処理</summary>
    public void HpPlus(int addnum)
    {
        bool isDead = TryZero(_hp + addnum);
        bool isMax = _hp + addnum > _maxHp;

        _hp = (isDead, isMax) switch
        {
            (true, false) => 0,
            (false, true) => _maxHp,
            _ => _hp + addnum,
        };
    }

    /// <summary>MPの増減処理</summary>
    public void MpPlus(int addnum)
    {
        bool isDead = TryZero(_mp + addnum);
        bool isMax = _mp + addnum > _maxMp;

        _mp = (isDead, isMax) switch
        {
            (true, false) => 0,
            (false, true) => _maxMp,
            _ => _mp + addnum,
        };
    }

    /// <summary>最大体力の増減処理</summary>
    public void MaxHpPlus(int addnum) => _maxHp = TryZero(_maxHp + addnum) ? _maxHp = 0 : _maxHp + addnum;

    /// <summary>最大MPの増減処理</summary>
    public void MaxMpPlus(int addnum) => _maxMp = TryZero(_maxMp + addnum) ? _maxMp = 0 : _maxMp + addnum;

    /// <summary>攻撃力の増減処理</summary>
    public void AttackPlus(int addnum) => _attack = TryZero(_attack + addnum) ? _attack = 0 : _attack + addnum;

    /// <summary>防御力の増減処理</summary>
    public void DefensePlus(int addnum) => _defense = TryZero(_defense + addnum) ? _defense = 0 : _defense += addnum;

    /// <summary>素早さの増減処理</summary>
    public void SpeedPlus(int addnum) => _speed = TryZero(_speed + addnum) ? _speed = 0 : _speed += addnum;

    /// <summary>値が0以下かどうかをチェックする</summary>
    /// <param name="num">0以下かチェックする値</param>
    private bool TryZero(int num) { return (num <= 0) ? true : false; }
    #endregion
}
