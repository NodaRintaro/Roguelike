using UnityEngine;

[CreateAssetMenu(fileName = "EnemysData", menuName = "Scriptable Objects/EnemysData")]
public class EnemysData : ScriptableObject
{
    [SerializeField,Header("�L�����̏o���m��")] private int _probability = 5;

    public int Probability => _probability;

    [SerializeField,Header("�L�����̃v���t�@�u")] GameObject _enemyPrefab = null;

    public GameObject EnemyPrefab => _enemyPrefab;
}
