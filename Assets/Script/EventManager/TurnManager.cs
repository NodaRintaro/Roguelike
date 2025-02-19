using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// �v���C���[��NPC�̍s�����Ǘ�����Class
/// </summary>
public class TurnManager : MonoBehaviour
{
    [SerializeField, Header("�s���\�L�����N�^�[�̃��X�g")]
    public List<CharacterData> _canMoveCharactersList = new();

    [SerializeField, Header("�s���ς݃L�����N�^�[�̃��X�g")]
    private List<CharacterData> _alreadyActedCharacters = new();

    /// <summary>
    /// ���̃L�����ɍs����������
    /// </summary>
    /// <param name="speed"></param>
    public void GoNextTurn(CharacterData currentActCharacter)
    {
        CharacterData NextMoveCharacter = null;
        foreach (var character in _canMoveCharactersList)
        {
            //�z����̎��̍s�����̃L�����N�^�[��T��
            if(NextMoveCharacter == null)
            {
                if (currentActCharacter.Speed > character.Speed)
                {
                    NextMoveCharacter = character;
                }
            }
            else
            {
                if(currentActCharacter.Speed >= character.Speed && NextMoveCharacter.Speed < character.Speed)
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
        
        NextMoveCharacter.TurnChange();
        _canMoveCharactersList.Remove(NextMoveCharacter);
        _alreadyActedCharacters.Add(NextMoveCharacter);

        if(_canMoveCharactersList.Count == 0)
        {
            foreach(var character in _alreadyActedCharacters)
            {
                _canMoveCharactersList.Add(character);
            }
            _alreadyActedCharacters.Clear();
        }
    }

    public void AddCharactersList(CharacterData addCharacter)
    {
        Debug.Log("���s���ł�");
        _canMoveCharactersList.Add(addCharacter);
    }
}