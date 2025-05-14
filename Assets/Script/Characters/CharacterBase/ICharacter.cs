using UnityEngine;

/// <summary>
/// Chracterのインターフェース
/// </summary>
public interface ICharacter
{
    public GameObject CharacterObject { get; }

    public int HP { get; }

    public int Attack { get; }

    public int Defense { get; }

    public int Speed { get; }

    public MoveState CharacterState { get; }

    public void StatusInitialize(CharacterData characterData, GameObject characterObject);
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
