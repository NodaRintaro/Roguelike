using System;
using UnityEngine;

/// <summary>
/// Chracterのインターフェース
/// </summary>
public interface ICharacter
{
    public GameObject CharacterObject { get; }

    public Status CharacterStatus { get; }

    public MoveState CharacterState { get; }

    /// <summary>キャラクタのデータを初期化</summary>
    /// <param name="characterData">キャラクタのデータ</param>
    /// <param name="characterObject">キャラクタのオブジェクト</param>
    public void InitCharacterData(CharacterData characterData, GameObject characterObject);
    public void StartAction();
    public void FinishAction();
}

public enum MoveState
{
    None,
    Move,
    Stay,
    Finish
}
