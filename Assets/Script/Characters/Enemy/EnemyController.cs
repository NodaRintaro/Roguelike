using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyController : MonoBehaviour
{ 

    [SerializeField,Header("プレイヤー")]
    private GameObject _playerObject;

    [SerializeField] GameObject _turnManagerObj;

    CharacterData _enemy;
    TurnManager _turnManager;
    MapGenerator _mapGenerator;
    EnemyMove _enemyMove;

    private MoveType _moveType ;

    private void Awake()
    {
        _enemy = GetComponent<CharacterData>();
        _enemyMove = GetComponent<EnemyMove>();
        _turnManager = FindAnyObjectByType<TurnManager>();
        _mapGenerator = FindAnyObjectByType<MapGenerator>();
        _playerObject = GameObject.FindWithTag("Player");
    }

    
    private async UniTask OnMyTurn()
    {
        Debug.Log("敵キャラのターン！");

        switch (_moveType)
        {
            case MoveType.Attack:
                break;
            case MoveType.Walk:
                _enemyMove.OnWalk();
                break;
        }
        if(_turnManager != null)
           _turnManager.GoNextTurn(_enemy);
    }

    enum MoveType
    {
        Attack,
        Walk
    }

}
