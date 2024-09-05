using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��敪���@�ɂ��Map��������
/// </summary>
public class MapGenerater : MonoBehaviour
{
    //�}�b�v�S�̂̑傫�������߂�
    [SerializeField,Header("�}�b�v�S�̂̉���")] static public int _xLength = 50;//�}�b�v�S�̂̉���
    [SerializeField,Header("�}�b�v�S�̂̏c��")] static public int _zLength = 50;//�}�b�v�S�̂̏c��
    [SerializeField,Header("���G���A�̐�")] int _areaNum = 4;//���G���A�̐�

    //�����̑傫���̌��߂邽�߂͈̔�
    [SerializeField,Header("��������G���A�傫���̍ŏ��l")] int _roomSizeMin = 3;//��������G���A�傫���̍ŏ��l
    [SerializeField,Header("��������G���A�傫���̍ő�l")] int _roomSizeMax = 7;//��������G���A�傫���̍ő�l

    private int _randomPosX, _randomPosZ, _randomRoomSize;//���Əc�̒��S�_�Ƃ֕����̑傫��

    public void MapGenerate()
    {
        int areaSize;//��������G���A�̑傫��

        int currentMaxAreaSize = 1;//����x���W�̍ő�l
        int keepMinAreaSize = 1;//�O���x���W�̍ő�l

        areaSize = _xLength / _areaNum;//��������傫�������߂�

        for(int i = 0; i < _areaNum; i++)
        {
            keepMinAreaSize = currentMaxAreaSize;//�O��̍ő�l��ۑ�����
            if(i == 0)
            {
                currentMaxAreaSize = areaSize;
                _randomPosX = Random.Range(_roomSizeMin + keepMinAreaSize, currentMaxAreaSize);
            }//�ŏ��̋�悾�����ꍇ
            else if(i == _areaNum - 1)
            {
                currentMaxAreaSize = _xLength - 1;
                Debug.Log($"{"�O��̍ő�̕�:" + currentMaxAreaSize} {"����̍ő�̕�:" + keepMinAreaSize}");
                _randomPosX = Random.Range(keepMinAreaSize, currentMaxAreaSize);
            }
            else
            {
                currentMaxAreaSize += areaSize;
                Debug.Log($"{"�O��̍ő�̕�:" + currentMaxAreaSize} {"����̍ő�̕�:" + keepMinAreaSize}");
            }
        }
    }
}
