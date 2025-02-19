using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawn : MonoBehaviour
{
    [SerializeField,Header("プレイヤーのプレファブ")]
    private GameObject _playerObject;

    [SerializeField,Header("敵の種類と出現確率")]
    private List<EnemysGachaData> _enemyList;

    [SerializeField,Header("アイテムの種類と出現確率")]
    private List<GameObject> _itemList;

    private MapGenerator _mapCreate;

    private TurnManager _turnManager;

    private List<GameObject> _actorsList = new();

    public GameObject PlayerPrefab => _playerObject;

    public List<EnemysGachaData> EnemyList => _enemyList;

    public List<GameObject> ItemList => _itemList;

    private void Start()
    {
        _mapCreate = GetComponent<MapGenerator>();
        _turnManager = FindFirstObjectByType<TurnManager>();
    }

    /// <summary>
    /// キャラを生成する
    /// </summary>
    /// <param name="spawnObject"></param>
    /// <param name="posX"></param>
    /// <param name="posZ"></param>
    public void SpawnActor(GameObject spawnObject, int posX, int posZ)
    {
        GameObject generatedObj = Instantiate(spawnObject, new Vector3(posX * _mapCreate.GridSize, _mapCreate.GridSize, posZ * _mapCreate.GridSize), Quaternion.identity);
        CharacterData spawnCharaInstance = generatedObj.GetComponent<CharacterData>();
        _turnManager._canMoveCharactersList.Add(spawnCharaInstance);
        _actorsList.Add(generatedObj);
        Debug.Log("キャラを生成");
    }

    /// <summary>
    /// キャラを生成する位置をランダムに決定する
    /// </summary>
    /// <param name="Xpos"></param>
    /// <param name="Zpos"></param>
    public void RandomSpawnPos(out int Xpos, out int Zpos)
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
        Xpos = Random.Range(_mapCreate.RoomData[roomKey].xMinPos, _mapCreate.RoomData[roomKey].xMaxPos);
        Zpos = Random.Range(_mapCreate.RoomData[roomKey].zMinPos, _mapCreate.RoomData[roomKey].zMaxPos);

        //もし他のキャラと出現場所がかぶったらもう一度出現場所をランダムに選択する
        if (_actorsList != null)
        {
            foreach (var actors in _actorsList)
            {
                if(Xpos == actors.transform.position.x / _mapCreate.GridSize && Zpos == actors.transform.position.z / _mapCreate.GridSize)
                {
                    RandomSpawnPos(out Xpos, out Zpos);
                }
            }
        }
    }

    /// <summary>
    /// 重み付き確率計算による生成する敵キャラの選択
    /// </summary>
    /// <param name="GachaList"></param>
    /// <returns></returns>
    public GameObject SpawnGacha()
    {
        float totalNum = 0;
        foreach (var gachaContents in _enemyList)
        {
            totalNum += gachaContents.Probability;
        }

        float randomPoint = Random.value * totalNum;

        for (int i = 0; i < _enemyList.Count; i++)
        {
            if(randomPoint < _enemyList[i].Probability)
            {
                return _enemyList[i].EnemyPrefab;
            }
            else
            {
                randomPoint -= _enemyList[i].Probability;
            }
        }
        return _enemyList[_enemyList.Count - 1].EnemyPrefab;
    }
}
