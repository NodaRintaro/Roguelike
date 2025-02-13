using UnityEngine;

public class FieldSetUp : MonoBehaviour
{
    [SerializeField,Header("マップを自動で生成してくれるクラス")] 
    private MapCreate _mapCreate;

    [SerializeField,Header("キャラをランダムな位置に生成してくれるクラス")] 
    private CharacterSpawn _characterSpawn;

    private void Start()
    {
        _characterSpawn = GetComponent<CharacterSpawn>();
        _mapCreate = GetComponent<MapCreate>();

        SetUpField();
    }

    public void SetUpField()
    {
        int randomPosX, randomPosZ;

        //マップの生成
        _mapCreate.MapGenerate();

        //プレイヤーをスポーンさせる
        _characterSpawn.RandomSpawnPos(out randomPosX, out randomPosZ);
        _characterSpawn.SpawnActor(_characterSpawn.PlayerPrefab, randomPosX, randomPosZ);



    }

}