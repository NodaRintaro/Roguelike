using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーやNPCの行動を管理するClass
/// </summary>
public class TurnManager : MonoBehaviour
{
    static TurnManager _TMinstance = new TurnManager();
    public static TurnManager Instance => _TMinstance;

    [Header("行動可能キャラクターのリスト")]
    public List<Character> _moveCharacters = new();

    /// <summary>
    /// 次のキャラに行動をさせる
    /// </summary>
    /// <param name="speed"></param>
    public void GoNextTurn(int speed)
    {
        Character currentNextMoveCharacter = null;
        foreach (var character in _moveCharacters)
        {
            //配列内の次の行動順のキャラクターを探す
            if(currentNextMoveCharacter == null)
            {
                if (speed >= character.Speed)
                {
                    currentNextMoveCharacter = character;
                }
            }
            else
            {
                if(speed >= character.Speed && currentNextMoveCharacter.Speed < character.Speed)
                {
                    currentNextMoveCharacter = character;
                }
            }
        }

        //もし、次の行動順のキャラがいなければ一番早いキャラに行動順が移る
        if (currentNextMoveCharacter == null)
        {
            foreach (var character in _moveCharacters)
            {
                if (currentNextMoveCharacter == null || currentNextMoveCharacter.Speed < character.Speed)
                {
                    currentNextMoveCharacter = character;
                }
            }
        }
        
        currentNextMoveCharacter.TurnChange();
    }
}