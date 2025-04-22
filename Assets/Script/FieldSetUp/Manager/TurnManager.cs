using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーやNPCの行動を管理するClass
/// </summary>
public class TurnManager : MonoBehaviour
{
    [Header("行動可能キャラクターのリスト")]
    public List<Characters> _canMoveCharactersList = new();

    [SerializeField, Header("行動済みキャラクターのリスト")]
    private List<Characters> _alreadyActedCharacters = new();

    /// <summary>
    /// 次のキャラに行動をさせる
    /// </summary>
    /// <param name="speed"></param>
    public void GoNextTurn(Characters currentCharacter)
    {
        Characters NextMoveCharacter = null;
        foreach (var character in _canMoveCharactersList)
        {
            //配列内の次の行動順のキャラクターを探す
            if(NextMoveCharacter == null)
            {
                if (currentCharacter.Speed > character.Speed)
                {
                    NextMoveCharacter = character;
                }
            }
            else
            {
                if(currentCharacter.Speed >= character.Speed && NextMoveCharacter.Speed < character.Speed)
                {
                    NextMoveCharacter = character;
                }
            }
        }

        //もし、次の行動順のキャラがいなければ一番早いキャラに行動順が移る
        if (NextMoveCharacter == null)
        {
            foreach (var character in _canMoveCharactersList)
            {
                if (NextMoveCharacter == null || NextMoveCharacter.Speed < character.Speed)
                {
                    NextMoveCharacter = character;
                }
            }
        }

        //次のターンに行動するキャラを行動済みのListにあらかじめ入れておく
        _canMoveCharactersList.Remove(NextMoveCharacter);
        _alreadyActedCharacters.Add(NextMoveCharacter);

        if (_canMoveCharactersList.Count == 0)
        {
            foreach(var character in _alreadyActedCharacters)
            {
                _canMoveCharactersList.Add(character);
            }
            _alreadyActedCharacters.Clear();
        }

        NextMoveCharacter.TurnChange();
    }

    public void AddCharactersList(Characters addCharacter)
    {
        Debug.Log("実行中です");
        _canMoveCharactersList.Add(addCharacter);
    }


    public void RemoveCharacter(Characters removeCharacter)
    {
        if(_canMoveCharactersList.Contains(removeCharacter))
        {
            _canMoveCharactersList.Remove(removeCharacter);
        }
        if (_alreadyActedCharacters.Contains(removeCharacter))
        {
            _alreadyActedCharacters.Remove(removeCharacter);
        }
    }
}