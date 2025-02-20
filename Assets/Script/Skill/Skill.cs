using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class Skill : ScriptableObject
{
    [SerializeField,Header("�X�L����")]
    private string _skillName;

    [SerializeField,Header("�З�")]
    private int _skillDamage;

    [SerializeField,Header("�͈�")]
    private int _skillRange;

    public string SkillName => _skillName;
    public int SkillDamage => _skillDamage; 
    public int SkillRange => _skillRange;
}
