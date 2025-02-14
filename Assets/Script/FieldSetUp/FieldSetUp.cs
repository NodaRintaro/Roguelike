using Unity.VisualScripting;
using UnityEngine;

public class FieldSetUp : MonoBehaviour
{
    [SerializeField,Header("�}�b�v�������Ő������Ă����N���X")] 
    private MapCreate _mapCreate;

    [SerializeField,Header("�L�����������_���Ȉʒu�ɐ������Ă����N���X")] 
    private CharacterSpawn _characterSpawn;

    [SerializeField,Header("�ŏ��ɃX�|�[��������G�̐�")]
    private int _firstSpawnEnemysNum = 4;

    private void Start()
    {
        _characterSpawn = GetComponent<CharacterSpawn>();
        _mapCreate = GetComponent<MapCreate>();

        FirstFieldSetUp();
    }

    /// <summary>
    /// �X�e�[�W�ɓ������ۂɍŏ��ɍs���t�B�[���h�̃Z�b�g�A�b�v
    /// </summary>
    public void FirstFieldSetUp()
    {
        int randomPosX, randomPosZ;

        //�}�b�v�̐���
        _mapCreate.MapGenerate();

        //�v���C���[���X�|�[��������
        _characterSpawn.RandomSpawnPos(out randomPosX, out randomPosZ);
        _characterSpawn.SpawnActor(_characterSpawn.PlayerPrefab, randomPosX, randomPosZ);

        for (int spawnCount = 0; spawnCount < _firstSpawnEnemysNum; spawnCount++)
        {
            _characterSpawn.RandomSpawnPos(out randomPosX, out randomPosZ);
            _characterSpawn.SpawnActor(_characterSpawn.SpawnGacha(), randomPosX, randomPosZ);
        }
    }

}