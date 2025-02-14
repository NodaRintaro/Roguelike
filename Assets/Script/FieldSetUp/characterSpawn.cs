using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawn : MonoBehaviour
{
    [SerializeField,Header("�v���C���[�̃v���t�@�u")]
    private GameObject _playerObject;

    [SerializeField,Header("�G�̎�ނƏo���m��")]
    private List<EnemysData> _enemyList;

    [SerializeField,Header("�A�C�e���̎�ނƏo���m��")]
    private List<GameObject> _itemList;

    private List<GameObject> _actorsList = new();

    private MapCreate _mapCreate;

    public GameObject PlayerPrefab => _playerObject;

    public List<EnemysData> EnemyList => _enemyList;

    public List<GameObject> ItemList => _itemList;

    private void Start()
    {
        _mapCreate = GetComponent<MapCreate>();
    }

    /// <summary>
    /// �L�����𐶐�����
    /// </summary>
    /// <param name="spawnObject"></param>
    /// <param name="posX"></param>
    /// <param name="posZ"></param>
    public void SpawnActor(GameObject spawnObject, int posX, int posZ)
    {
        _actorsList.Add(Instantiate(spawnObject, new Vector3(posX * _mapCreate.GridSize, _mapCreate.GridSize, posZ * _mapCreate.GridSize), Quaternion.identity));
    }

    /// <summary>
    /// �L�����𐶐�����ʒu�������_���Ɍ��肷��
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
    /// �d�ݕt���m���v�Z�ɂ�鐶������G�L�����̑I��
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
