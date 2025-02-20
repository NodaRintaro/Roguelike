using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField, Header("プレイヤータグ")]
    private string _playerTag;

    [SerializeField,Header("プレイヤーとの距離")]
    private Vector3 _offset;    // カメラとプレイヤーの距離

    [SerializeField,Header("プレイヤー")]
    private GameObject _player;  // プレイヤーオブジェクト（ターゲット）

    void Update()
    {
        if( _player == null )
        {
            _player = GameObject.FindWithTag(_playerTag);
        }

        // プレイヤーの位置にオフセットを追加してカメラを移動
        this.transform.position = _player.transform.position + _offset;
        transform.LookAt(_player.transform.position);
    }
}
