using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyController : MonoBehaviour
{ 

    [SerializeField,Header("プレイヤー")]
    private GameObject _playerObject;

    ICharacter _enemy;
    TurnManager _turnManager;
    MapGenerator _mapGenerator;
    EnemyMove _enemyMove;

    private MoveType _moveType ;

    private void Awake()
    {
        _enemy = GetComponent<ICharacter>();
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
    }

    enum MoveType
    {
        Attack,
        Walk
    }

}
