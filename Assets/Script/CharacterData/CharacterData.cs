using UnityEngine;

public class CharacterData : MonoBehaviour 
{
    [SerializeField] private int _HP;

    [SerializeField] private int _speed;

    [SerializeField] private int _power;

    [SerializeField] private bool _canMove;

    public int HP => _HP;

    public int Speed => _speed;
    
    public int Power => _power;
    
    public bool CanMove => _canMove;
    
    public void TurnChange()
    {
        if (_canMove)
        {
            _canMove = false;
        }
        else
        {
            _canMove = true;
        }
    }
}
