using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    //�}�b�v�S�̂̑傫�������߂�
    [SerializeField] static public int _x = 50;//�}�b�v�S�̂̉���
    [SerializeField] static public int _z = 50;//�}�b�v�S�̂̏c��
    [SerializeField] int _areaNum = 4;//���G���A�̐�

    //�����̑傫���̌��߂邽�߂͈̔�
    [SerializeField] int _mapSizeMin = 3;//��������G���A�傫���̍ŏ��l
    [SerializeField] int _mapSizeMax = 7;//��������G���A�傫���̍ő�l
}
