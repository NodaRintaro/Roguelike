using System;
using UnityEngine;

/// <summary>
/// 各キャラクターの基礎データを持つクラス
/// </summary>
[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [SerializeField, Header("初期HP")]
    private int _baseHP;

    [SerializeField, Header("初期MP")]
    private int _baseMP;

    [SerializeField, Header("初期の攻撃力")]
    private int _baseAttack;

    [SerializeField,Header("防御力")] 
    private int _baseDefense;

    [SerializeField, Header("初期の素早さ")]
    private int _baseSpeed;

    [SerializeField, Header("スポーン確率")]
    private int _spawnProbability;

    [SerializeField, Header("キャラのプレファブ")]
    private GameObject _characterPrefab;

    public int BaseHP => _baseHP;

    public int BaseMP => _baseMP;

    public int BaseAttack => _baseAttack;

    public int BaseDefense => _baseDefense;

    public int BaseSpeed => _baseSpeed;

    public GameObject CharacterPrefab => _characterPrefab;

    public int SpawnProbability => _spawnProbability;
}