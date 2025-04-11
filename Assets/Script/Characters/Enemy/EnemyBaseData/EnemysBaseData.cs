using UnityEngine;

[CreateAssetMenu(fileName = "EnemysData", menuName = "Scriptable Objects/EnemysData")]
public class EnemysBaseData : ScriptableObject
{
    [SerializeField,Header("キャラの出現確率")] private int _probability = 5;

    public int Probability => _probability;

    [SerializeField,Header("キャラのプレファブ")] GameObject _enemyPrefab = null;

    public GameObject EnemyPrefab => _enemyPrefab;
}
