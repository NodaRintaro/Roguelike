using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// �v���C���[��NPC�̍s�����Ǘ�����Class
/// </summary>
public class TurnManager : MonoBehaviour
{
    static TurnManager _TMinstance = new TurnManager();
    public static TurnManager Instance => _TMinstance;

    [Header("�s���\�L�����N�^�[�̃��X�g")]
    public List<Character> _moveCharacters = new();

    /// <summary>
    /// ���̃L�����ɍs����������
    /// </summary>
    /// <param name="speed"></param>
    public void GoNextTurn(int speed)
    {
        Character currentNextMoveCharacter = null;
        foreach (var character in _moveCharacters)
        {
            //�z����̎��̍s�����̃L�����N�^�[��T��
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

        //�����A���̍s�����̃L���������Ȃ���Έ�ԑ����L�����ɍs�������ڂ�
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