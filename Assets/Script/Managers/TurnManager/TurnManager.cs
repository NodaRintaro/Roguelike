using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// 生成したキャラクターの行動順を決定し制御するClass
/// </summary>
public class TurnManager : MonoBehaviour
{
    [SerializeField, Header("経過ターン")] int _currentTurn = 0;

    [SerializeField, Header("最大行動回数")] int _maxMoveNum = 3;

    [SerializeReference, SubclassSelector, Header("現在行動中のキャラクター")]
    private ICharacter _currentMoveCharacter;

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

    private void Update()
    {
        if(_currentMoveCharacter != null)
        {
            if (_currentMoveCharacter.CharacterState == MoveState.Stay)
            {
                NextTurn();
            }
        }
    }


    //TODO:TurnManagerをメソッドクラスにする

    /// <summary>
    /// キャラの行動順を決める
    /// </summary>
    public void SetActionTurn(List<ICharacter>activeCharacters)
    {
        _currentTurn++;
        List<ICharacter> characters = new();
        List<ICharacter> alreadyMoveCharacters = new();
        foreach (var character in activeCharacters)
        { 
             characters.Add(character);
        }
        Debug.Log("charactersCount:" + characters.Count);

        SortSpeed(characters);

        int actionCount = 0;
        int bonusActionPoint = 40;
        while (characters.Count != alreadyMoveCharacters.Count && actionCount != _maxMoveNum)
        {
            foreach (ICharacter character in characters)
            {
                if(!alreadyMoveCharacters.Contains(character))
                    _charactersActionOrder.Enqueue(character);
                
                if(character.CharacterStatus.Speed - bonusActionPoint < 0)
                    alreadyMoveCharacters.Add(character);
            }

            bonusActionPoint += (bonusActionPoint / 2);
            actionCount++;
        }
        Debug.Log("QueueCount:" + _charactersActionOrder.Count);

        NextTurn();
    }

    /// <summary>
    /// 次のキャラに行動をさせる
    /// </summary>
    public void NextTurn()
    { 
        if (_charactersActionOrder.Count == 0)
        {
            //TODO:全員行動し終わったら次の行動順を制作
        }
        else
        {
            ICharacter nextActionCharacter = _charactersActionOrder.Dequeue();

            nextActionCharacter.StartAction();
            _currentMoveCharacter = nextActionCharacter;
        }
    }

    public void SetActionCharacters()
    {

    }

    /// <summary>
    /// キャラクター達を行動順にソートする
    /// </summary>
    /// <param name="characters"></param>
    public void SortSpeed(List<ICharacter> characters)
    {
        characters.Sort((x, y) => y.CharacterStatus.Speed.CompareTo(x.CharacterStatus.Speed));
    }
}