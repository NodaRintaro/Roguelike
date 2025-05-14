using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成したキャラクターの行動順を決定し制御するClass
/// </summary>
public class TurnManager : MonoBehaviour
{
    [SerializeField, Header("経過ターン")] int _currentTurn = 0;

    [SerializeField, Header("最大行動回数")] int _maxMoveNum = 3;

    public int CurrentTurn => _currentTurn;

    /// <summary>
    /// 行動順を表示するQueue
    /// </summary>
    private Queue<ICharacter> _charactersActionOrder = new();

    private FieldManager _fieldManager;

    private void Start()
    {
        _fieldManager = FindAnyObjectByType<FieldManager>();
    }

    /// <summary>
    /// キャラの行動順を決める
    /// </summary>
    public void SetActionOrder()
    {
        _currentTurn++;
        List<ICharacter> characters = new();
        List<ICharacter> alreadyMoveCharacters = new();
        foreach (var character in _fieldManager.ActiveCharactersList)
        { 
             characters.Add(character);
        }
        Debug.Log("charactersCount:" + characters.Count);

        characters.Sort((x,y) => y.Speed.CompareTo(x.Speed));

        int actionCount = 0;
        int bonusActionPoint = 40;
        while (characters.Count != alreadyMoveCharacters.Count && actionCount != _maxMoveNum)
        {
            foreach (ICharacter character in characters)
            {
                if(!alreadyMoveCharacters.Contains(character))
                    _charactersActionOrder.Enqueue(character);
                
                if(character.Speed - bonusActionPoint < 0)
                    alreadyMoveCharacters.Add(character);
            }

            bonusActionPoint += (bonusActionPoint / 2);
            actionCount++;
        }
        Debug.Log("QueueCount:" + _charactersActionOrder.Count);

        NextActionOrder();
    }

    /// <summary>
    /// 次のキャラに行動をさせる
    /// </summary>
    public void NextActionOrder()
    { 
        if (_charactersActionOrder.Count == 0)
        {
            SetActionOrder();
        }
        else
        {
            ICharacter nextActionCharacter = _charactersActionOrder.Dequeue();

            nextActionCharacter.StartAction();
            _fieldManager.ChangeMoveCharacter(nextActionCharacter);
        }
    }
}