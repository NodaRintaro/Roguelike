using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [SerializeField, Header("ダンジョンに出現する敵キャラのリスト")]
    private List<CharacterData> _enemysPool = new();

    [SerializeReference, SubclassSelector, Header("Field上に存在するキャラクターのリスト")]
    private List<ICharacter> _activeCharactersList = new();

    [SerializeField, Header("最初にスポーンさせる敵の数")]
    private int _firstSpawnEnemysNum = 4;

    [SerializeReference, SubclassSelector, Header("現在行動中のキャラクター")]
    private ICharacter _currentMoveCharacter;

    public List<CharacterData> EnemysPool => _enemysPool;

    public List<ICharacter> ActiveCharactersList => _activeCharactersList;

    private MapGenerator _mapGenerater;
    private CharacterSpawner _spawnManager;
    private TurnManager _turnManager;

    private void Start()
    {
        _spawnManager = GetComponent<CharacterSpawner>();
        _mapGenerater = GetComponent<MapGenerator>();
        _turnManager = FindFirstObjectByType<TurnManager>();

        FieldInitialization();
    }

    private void Update()
    {
        if (_currentMoveCharacter.CharacterState == MoveState.Stay)
        {
            _turnManager.NextActionOrder();
        }
    }

    public void ChangeMoveCharacter(ICharacter nextMoveCharacter)
    {
        _currentMoveCharacter = nextMoveCharacter;
    }

    /// <summary>
    /// ステージに入った際に最初に行うフィールドのセットアップ
    /// </summary>
    public void FieldInitialization()
    {
        //マップの生成
        _mapGenerater.MapGenerate();

        //プレイヤーを配置
        _activeCharactersList.Add(_spawnManager.PlayerSpawn());

        //敵キャラクターを生成
        for (int spawnCount = 0; spawnCount < _firstSpawnEnemysNum; spawnCount++)
        {
            _activeCharactersList.Add(_spawnManager.EnemySpawn(_spawnManager.RandomPickCharacter(_enemysPool)));
        }

        _turnManager.SetActionOrder();
    }
}