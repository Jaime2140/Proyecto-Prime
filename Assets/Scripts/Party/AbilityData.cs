using UnityEngine;

[CreateAssetMenu(fileName = "Nueva Habilidad", menuName = "RPG/Habilidad")]
public class AbilityData : ScriptableObject
{
    public enum AbilityType { AtaqueBase, Soporte, Guardia, Ulti }
    public enum SupportEffect { Ninguno, Curar, BuffAtaque, BuffDefensa, DebuffEnemigo }
    public enum TargetType { UnEnemigo, TodosLosEnemigos, UnAliado, TodosLosAliados, UnoMismo }


    [Header("Información Básica")]
    public string abilityName;
    [TextArea] public string description;
    public AbilityType type;

    [Header("Efectos Principales")]
    public int power;
    public int ultiCost;
    public int ultiGain;

    [Header("Efectos de Soporte / Buffs")]
    public SupportEffect supportEffect;
    
    [Tooltip("Cuánto sube la estadística o cuánto cura extra")]
    public int effectModifierValue; 
    
    [Tooltip("A quién afecta esta habilidad de soporte")]
    public TargetType targetType;
}