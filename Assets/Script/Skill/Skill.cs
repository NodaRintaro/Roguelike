using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class Skill : ScriptableObject
{
    [SerializeField,Header("スキル名")]
    private string _skillName;

    [SerializeField,Header("威力")]
    private int _skillDamage;

    [SerializeField,Header("範囲")]
    private int _skillRange;

    public string SkillName => _skillName;
    public int SkillDamage => _skillDamage; 
    public int SkillRange => _skillRange;
}
