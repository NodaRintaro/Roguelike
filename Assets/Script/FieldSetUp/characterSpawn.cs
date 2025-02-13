using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawn : MonoBehaviour
{
    [SerializeField,Header("プレイヤーのプレファブ")]
    private GameObject _playerObject;

    [Header("敵の種類")]
    public List<GameObject> _enemyList;

    [Header("アイテムの種類")]
    public List<GameObject> _item;

    private List<GameObject> _actorsList = new();

    private MapCreate _mapCreate;

    public GameObject PlayerPrefab => _playerObject;

    private void Start()
    {
        _mapCreate = GetComponent<MapCreate>();
    }

    /// <summary>
    /// キャラクターを生成する
    /// </summary>
    /// <param name="spawnObject"></param>
    /// <param name="posX"></param>
    /// <param name="posZ"></param>
    public void SpawnActor(GameObject spawnObject, int posX, int posZ)
    {
        _actorsList.Add(Instantiate(spawnObject, new Vector3(posX * _mapCreate.GridSize, 1, posZ * _mapCreate.GridSize), Quaternion.identity));
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
        if(roomKey == null)
        {
            Debug.Log("keyが割り当てられていません");
        }
        Xpos = Random.Range(_mapCreate.RoomData[roomKey].xMinPos, _mapCreate.RoomData[roomKey].xMaxPos);
        Zpos = Random.Range(_mapCreate.RoomData[roomKey].zMinPos, _mapCreate.RoomData[roomKey].zMaxPos);
    }
}
