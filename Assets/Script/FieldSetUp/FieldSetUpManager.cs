using Unity.VisualScripting;
using UnityEngine;

public class FieldSetUpManager : MonoBehaviour
{
    [SerializeField,Header("マップを自動で生成してくれるクラス")] 
    private MapGenerator _mapCreate;

    [SerializeField,Header("キャラをランダムな位置に生成してくれるクラス")] 
    private CharacterSpawn _characterSpawn;

    [SerializeField,Header("最初にスポーンさせる敵の数")]
    private int _firstSpawnEnemysNum = 4;

    private void Start()
    {
        _characterSpawn = GetComponent<CharacterSpawn>();
        _mapCreate = GetComponent<MapGenerator>();

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