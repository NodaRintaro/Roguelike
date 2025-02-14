using Unity.VisualScripting;
using UnityEngine;

public class FieldSetUp : MonoBehaviour
{
    [SerializeField,Header("マップを自動で生成してくれるクラス")] 
    private MapCreate _mapCreate;

    [SerializeField,Header("キャラをランダムな位置に生成してくれるクラス")] 
    private CharacterSpawn _characterSpawn;

    [SerializeField,Header("最初にスポーンさせる敵の数")]
    private int _firstSpawnEnemysNum = 4;

    private void Start()
    {
        _characterSpawn = GetComponent<CharacterSpawn>();
        _mapCreate = GetComponent<MapCreate>();

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

}