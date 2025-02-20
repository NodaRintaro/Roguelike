using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class Skill : ScriptableObject
{
    [SerializeField,Header("ƒXƒLƒ‹–¼")]
    private string _skillName;

    [SerializeField,Header("ˆÐ—Í")]
    private int _skillDamage;

    [SerializeField,Header("”ÍˆÍ")]
    private int _skillRange;

    public string SkillName => _skillName;
    public int SkillDamage => _skillDamage; 
    public int SkillRange => _skillRange;
}
