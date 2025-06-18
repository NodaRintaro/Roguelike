using UnityEngine;
using DG.Tweening;

public class CharacterMove
{
    //TODO：プレイヤーの移動処理
    

    /// <summary>プレイヤーの移動処理</summary>
    /// <param name="moveVec">キャラクターを動かしたい方向のVector</param>
    /// <param name="characterTransform">動かしたいキャラクターのTransform</param>
    public void MoveCharacter(Vector2Int moveVec,Transform characterTransform,float moveTime)
    {
        Vector3 currentCharacterPos = characterTransform.position;
        Vector3 targetPos = new Vector3(currentCharacterPos.x + moveVec.x, currentCharacterPos.y, currentCharacterPos.z + moveVec.y);
        characterTransform.DOMove((targetPos),moveTime);
    }

    public bool TryMove(Transform characterTransform)
    {
        return true;
    }
}
