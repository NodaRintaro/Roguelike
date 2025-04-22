using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Serializable]
    public struct PreviousStatus
    {
        [SerializeField, Header("初期HP")]
        private int _baseHP;

        [SerializeField, Header("初期のMP")]
        private int _baseMP;

        [SerializeField, Header("初期の攻撃力")]
        private int _baseAttack;

        [SerializeField, Header("初期の素早さ")]
        private int _baseSpeed;

        public int BaseHP => _baseHP;

        public int BaseMP => _baseMP;

        public int BaseAttack => _baseAttack;

        public int BaseSpeed => _baseSpeed;

        public void AddBaseHP(int addBaseHP)
        {
            _baseHP += addBaseHP;
        }

        public void AddBaseMP(int addBaseMP)
        {
            _baseMP += addBaseMP;
        }
    }

    [SerializeField, Header("敵のプレファブ")] private GameObject _enemyPrefab;

    [SerializeField, Header("キャラの出現率")] private int _spawnProbability;

    [SerializeField, Header("HP")] private int _HP;

    [SerializeField, Header("MP")] private int _mp;

    [SerializeField, Header("攻撃力")] private int _speed;

    [SerializeField, Header("素早さ")] private int _attack;

    public GameObject EnemyPrefab => _enemyPrefab;

    public int SpawnProbability => _spawnProbability;

    public int HP => _HP;

    public int MP => _mp;

    public int Speed => _speed;
    
    public int Power => _attack;

    public void Damage(int damage)
    {
        _HP -= damage;
    }
}
