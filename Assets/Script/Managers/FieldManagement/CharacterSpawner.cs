using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField, Header("PlayerData")] CharacterData _playerData;

    [SerializeField,Header("キャラをスポーンさせる際のY座標")]
    private int _spawnHeight = 0;

    private MapGenerator _mapCreate;
    private FieldManager _fieldManager;
    
    private void Start()
    {
        _mapCreate = GetComponent<MapGenerator>();
        _fieldManager = GetComponent<FieldManager>();
    }

    /// <summary>
    /// Playerを生成する
    /// </summary>
    public ICharacter PlayerSpawn()
    {
        Vector3 spawnPos = RandomSpawnPos();

        GameObject spawnCharaObj =
            Instantiate(_playerData.CharacterPrefab, spawnPos, Quaternion.identity);

        PlayerBase character = new PlayerBase();

        character.InitCharacterData(_playerData, spawnCharaObj);

        return character;
    }

    /// <summary>
    /// 敵を生成する
    /// </summary>
    public ICharacter EnemySpawn(CharacterData spawnCharacter)
    {
        Vector3 spawnPos = RandomSpawnPos();

        GameObject spawnCharaObj = 
            Instantiate(spawnCharacter.CharacterPrefab, spawnPos, Quaternion.identity);

        //敵の情報データを作る
        EnemyBase character = new EnemyBase();

        character.InitCharacterData(spawnCharacter, spawnCharaObj);

        return character;
    }

    /// <summary>
    /// キャラを生成する位置をランダムに決定する
    /// </summary>
    private Vector3 RandomSpawnPos()
    {
        string roomKey = null;
        int count = 0;
        int randomRoomNum = Random.Range(0, _mapCreate.RoomData.Count);

        foreach(string key in _mapCreate.KeyList)
        {
            if(count == randomRoomNum)
            {
                roomKey = key;
                break;
            }
            count++;
        }

        int spawnPosX = Random.Range(_mapCreate.RoomData[roomKey].xMinPos, _mapCreate.RoomData[roomKey].xMaxPos);
        int spawnPosZ = Random.Range(_mapCreate.RoomData[roomKey].zMinPos, _mapCreate.RoomData[roomKey].zMaxPos);

        //もし他のキャラと出現場所がかぶったらもう一度出現場所をランダムに選択する
        foreach (var fieldChara in _fieldManager.FieldCharacters)
        {
            while (spawnPosX == fieldChara.CharacterObject.transform.position.x && spawnPosZ == fieldChara.CharacterObject.transform.position.z)
            {
                spawnPosX = Random.Range(_mapCreate.RoomData[roomKey].xMinPos, _mapCreate.RoomData[roomKey].xMaxPos);
                spawnPosZ = Random.Range(_mapCreate.RoomData[roomKey].zMinPos, _mapCreate.RoomData[roomKey].zMaxPos);
            }
        }　

        return new Vector3(spawnPosX * MapGenerator.GridSize, _spawnHeight, spawnPosZ * MapGenerator.GridSize);
    }

    /// <summary>
    /// 重み付き確率計算による生成する敵キャラの選択
    /// </summary>
    public CharacterData RandomPickCharacter(List<CharacterData> spawnList)
    {
        float totalNum = 0;
        foreach (var gachaContents in spawnList)
        {
            totalNum += gachaContents.SpawnProbability;
        }

        float randomPoint = Random.value * totalNum;

        for (int i = 0; i < spawnList.Count; i++)
        {
            if(randomPoint < spawnList[i].SpawnProbability)
            {
                return spawnList[i];
            }
            else
            {
                randomPoint -= spawnList[i].SpawnProbability;
            }
        }

        return spawnList[spawnList.Count - 1];
    }
}
