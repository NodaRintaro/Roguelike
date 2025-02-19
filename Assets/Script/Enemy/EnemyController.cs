using UnityEngine;

public class EnemyController : MonoBehaviour
{
    CharacterData _enemyData;
    TurnManager _turnManager;

    private void Start()
    {
        _enemyData = GetComponent<CharacterData>();
        _turnManager = FindFirstObjectByType<TurnManager>();
    }

    private void Update()
    {
        if (_enemyData.CanMove)
        {

            Debug.Log("‰´‚Ìƒ^[ƒ“I");
            Turnend();
            _turnManager.GoNextTurn(_enemyData);
        }
    }

    public void Turnend()
    {
        _enemyData.TurnChange();
    }
}
