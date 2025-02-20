using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class FieldSetUpManager : MonoBehaviour
{
    [SerializeField,Header("マップを自動で生成してくれるクラス")] 
    private MapGenerator _mapCreate;

    [SerializeField,Header("キャラをランダムな位置に生成してくれるクラス")] 
    private CharacterSpawn _characterSpawn;

    [SerializeField,Header("最初にスポーンさせる敵の数")]
    private int _firstSpawnEnemysNum = 4;

    private TurnManager _turnManager;

    private void Start()
    {
        _characterSpawn = GetComponent<CharacterSpawn>();
        _mapCreate = GetComponent<MapGenerator>();
        _turnManager = FindFirstObjectByType<TurnManager>();

        FirstFieldSetUp();
    }

    /// <summary>
    /// ステージに入った際に最初に行うフィールドのセットアップ
    /// </summary>
    public void FirstFieldSetUp()
    {
        int randomPosX, randomPosZ;

        //マップの生成
        _mapCreate.MapGenerate();

        //プレイヤーをスポーンさせる
        _characterSpawn.RandomSpawnPos(out randomPosX, out randomPosZ);
        _characterSpawn.SpawnActor(_characterSpawn.PlayerPrefab, randomPosX, randomPosZ);

        for (int spawnCount = 0; spawnCount < _firstSpawnEnemysNum; spawnCount++)
        {
            _characterSpawn.RandomSpawnPos(out randomPosX, out randomPosZ);
            _characterSpawn.SpawnActor(_characterSpawn.SpawnGacha(), randomPosX, randomPosZ);
        }

        Debug.Log(_turnManager._canMoveCharactersList[Random.Range(1, _turnManager._canMoveCharactersList.Count - 1)].Speed);
        _turnManager.GoNextTurn(_turnManager._canMoveCharactersList[Random.Range(1,_turnManager._canMoveCharactersList.Count - 1)]);
    }

    public void EnemySpawn(int spawnNum)
    {
        int randomPosX, randomPosZ;
        for (int spawnCount = 0; spawnCount < spawnNum; spawnCount++)
        {
            _characterSpawn.RandomSpawnPos(out randomPosX, out randomPosZ);
            _characterSpawn.SpawnActor(_characterSpawn.SpawnGacha(), randomPosX, randomPosZ);
        }
    }
}