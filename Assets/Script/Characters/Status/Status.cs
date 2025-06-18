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
    /// �X�e�[�^�X�̏�����
    /// </summary>
    /// <param name="hp">�̗�</param>
    /// <param name="mp">MP</param>
    /// <param name="at">�U����</param>
    /// <param name="df">�h���</param>
    /// <param name="sp">�f����</param>
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

    #region �X�e�[�^�X�̑�������
    /// <summary>HP�̑�������</summary>
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

    /// <summary>MP�̑�������</summary>
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

    /// <summary>�ő�̗͂̑�������</summary>
    public void MaxHpPlus(int addnum) => _maxHp = TryZero(_maxHp + addnum) ? _maxHp = 0 : _maxHp + addnum;

    /// <summary>�ő�MP�̑�������</summary>
    public void MaxMpPlus(int addnum) => _maxMp = TryZero(_maxMp + addnum) ? _maxMp = 0 : _maxMp + addnum;

    /// <summary>�U���͂̑�������</summary>
    public void AttackPlus(int addnum) => _attack = TryZero(_attack + addnum) ? _attack = 0 : _attack + addnum;

    /// <summary>�h��͂̑�������</summary>
    public void DefensePlus(int addnum) => _defense = TryZero(_defense + addnum) ? _defense = 0 : _defense += addnum;

    /// <summary>�f�����̑�������</summary>
    public void SpeedPlus(int addnum) => _speed = TryZero(_speed + addnum) ? _speed = 0 : _speed += addnum;

    /// <summary>�l��0�ȉ����ǂ������`�F�b�N����</summary>
    /// <param name="num">0�ȉ����`�F�b�N����l</param>
    private bool TryZero(int num) { return (num <= 0) ? true : false; }
    #endregion
}
