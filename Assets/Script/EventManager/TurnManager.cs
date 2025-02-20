using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// �v���C���[��NPC�̍s�����Ǘ�����Class
/// </summary>
public class TurnManager : MonoBehaviour
{
    [SerializeField, Header("�s���\�L�����N�^�[�̃��X�g")]
    public List<Character> _canMoveCharactersList = new();

    [SerializeField, Header("�s���ς݃L�����N�^�[�̃��X�g")]
    private List<Character> _alreadyActedCharacters = new();

    /// <summary>
    /// ���̃L�����ɍs����������
    /// </summary>
    /// <param name="speed"></param>
    public void GoNextTurn(Character currentCharacter)
    {
        Character NextMoveCharacter = null;
        foreach (var character in _canMoveCharactersList)
        {
            //�z����̎��̍s�����̃L�����N�^�[��T��
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

        //�����A���̍s�����̃L���������Ȃ���Έ�ԑ����L�����ɍs�������ڂ�
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

        //���̃^�[���ɍs������L�������s���ς݂�List�ɂ��炩���ߓ���Ă���
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

    public void AddCharactersList(Character addCharacter)
    {
        Debug.Log("���s���ł�");
        _canMoveCharactersList.Add(addCharacter);
    }


    public void RemoveCharacter(Character removeCharacter)
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