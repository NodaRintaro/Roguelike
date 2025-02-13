using UnityEngine;

public class FieldSetUp : MonoBehaviour
{
    [SerializeField,Header("�}�b�v�������Ő������Ă����N���X")] 
    private MapCreate _mapCreate;

    [SerializeField,Header("�L�����������_���Ȉʒu�ɐ������Ă����N���X")] 
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

        //�}�b�v�̐���
        _mapCreate.MapGenerate();

        //�v���C���[���X�|�[��������
        _characterSpawn.RandomSpawnPos(out randomPosX, out randomPosZ);
        _characterSpawn.SpawnActor(_characterSpawn.PlayerPrefab, randomPosX, randomPosZ);



    }

}