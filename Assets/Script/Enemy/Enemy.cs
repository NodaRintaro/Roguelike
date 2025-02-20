using UnityEngine;
using Cysharp.Threading.Tasks;

public class Enemy : Character 
{ 

    [SerializeField,Header("プレイヤー")]
    private GameObject _playerObject;

    [SerializeField] GameObject _turnManagerObj;

    Character _enemy;
    TurnManager _turnManager;
    MapGenerator _mapGenerator;
    EnemyMove _enemyMove;

    private MoveType _moveType ;

    private void Awake()
    {
        _enemy = GetComponent<Character>();
        _enemyMove = GetComponent<EnemyMove>();
        _turnManager = FindAnyObjectByType<TurnManager>();
        _mapGenerator = FindAnyObjectByType<MapGenerator>();
        _playerObject = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if(HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        _turnManager.RemoveCharacter(this);
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

    public override async void TurnChange()
    {
        await OnMyTurn();
    }

    enum MoveType
    {
        Attack,
        Walk
    }

}
