using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [SerializeField, Header("ダンジョンに出現する敵キャラのリスト")]
    private List<CharacterData> _enemysPool = new();

    [SerializeReference, SubclassSelector, Header("Field上に存在するキャラクターのリスト")]
    private List<ICharacter> _fieldCharacters = new();

    [SerializeField, Header("最初にスポーンさせる敵の数")]
    private int _firstSpawnEnemysNum = 4;

    public List<CharacterData> EnemysPool => _enemysPool;

    public List<ICharacter> FieldCharacters => _fieldCharacters;

    private MapGenerator _mapGenerater;
    private CharacterSpawner _spawnManager;
    private TurnManager _turnManager;


    private void Start()
    {
        _spawnManager = GetComponent<CharacterSpawner>();
        _mapGenerater = GetComponent<MapGenerator>();
        _turnManager = FindFirstObjectByType<TurnManager>();

        FieldInit();
    }

    /// <summary>
    /// ステージに入った際に最初に行うフィールドのセットアップ
    /// </summary>
    public void FieldInit()
    {
        //マップの生成
        _mapGenerater.MapGenerate();

        AddFieldCharacters(_spawnManager.PlayerSpawn());

        //敵キャラクターを生成
        for (int spawnCount = 0; spawnCount < _firstSpawnEnemysNum; spawnCount++)
        {
            _fieldCharacters.Add(_spawnManager.EnemySpawn(_spawnManager.RandomPickCharacter(_enemysPool)));
        }

        _turnManager.SetActionTurn(_fieldCharacters);
    }

    public void AddFieldCharacters(ICharacter character)
    {
        _fieldCharacters.Add(character);
    }
}