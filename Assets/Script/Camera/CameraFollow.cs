using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField, Header("�v���C���[�^�O")]
    private string _playerTag;

    [SerializeField,Header("�v���C���[�Ƃ̋���")]
    private Vector3 _offset;    // �J�����ƃv���C���[�̋���

    [SerializeField,Header("�v���C���[")]
    private GameObject _player;  // �v���C���[�I�u�W�F�N�g�i�^�[�Q�b�g�j

    void Update()
    {
        if( _player == null )
        {
            _player = GameObject.FindWithTag(_playerTag);
        }

        // �v���C���[�̈ʒu�ɃI�t�Z�b�g��ǉ����ăJ�������ړ�
        this.transform.position = _player.transform.position + _offset;
        transform.LookAt(_player.transform.position);
    }
}
