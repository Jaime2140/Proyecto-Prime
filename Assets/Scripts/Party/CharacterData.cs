using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Personaje", menuName = "RPG/Personaje")]
public class CharacterData : ScriptableObject
{
    [Header("Datos Biográficos")]
    public string characterName;
    public string role;
    public Sprite portrait;

    [Header("Estadísticas Base")]
    public int maxHP;
    public int attack;
    public int defense;
    public int speed;

    [Header("Kit de Habilidades (Equipadas)")]
    public AbilityData baseAttack;
    public AbilityData skill;
    public AbilityData guard;
    public AbilityData ultimate;

    [Header("Exclusivo de Violet (Wildcard)")]
    [Tooltip("Solo Violet usa este espacio. Aquí se equipa la habilidad heredada del amigo que se fue.")]
    public AbilityData tributeCoverSlot; 
}