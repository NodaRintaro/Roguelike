using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    //�}�b�v�S�̂̑傫�������߂�
    [SerializeField,Header("�}�b�v�S�̂̉���")] static public int _x = 50;//�}�b�v�S�̂̉���
    [SerializeField,Header("�}�b�v�S�̂̏c��")] static public int _z = 50;//�}�b�v�S�̂̏c��
    [SerializeField,Header("���G���A�̐�")] int _areaNum = 4;//���G���A�̐�

    //�����̑傫���̌��߂邽�߂͈̔�
    [SerializeField,Header("��������G���A�傫���̍ŏ��l")] int _mapSizeMin = 3;//��������G���A�傫���̍ŏ��l
    [SerializeField,Header("��������G���A�傫���̍ő�l")] int _mapSizeMax = 7;//��������G���A�傫���̍ő�l

    [SerializeField, Header("����x���W�̍ő�l")] private int _keepCurrentMaxArea = 1;
    [SerializeField, Header("�O���x���W�̍ő�l")] private int _keepMinAreaSize = 1;
}
