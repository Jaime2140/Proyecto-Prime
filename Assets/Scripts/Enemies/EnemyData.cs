using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Enemigo", menuName = "RPG/Enemigo")]
public class EnemyData : ScriptableObject
{
    [Header("Datos Visuales")]
    public string enemyName;
    public Sprite enemySprite;

    [Header("Estadísticas Base")]
    public int maxHP;
    public int speed;

    [Header("Inteligencia Artificial (Ataques)")]
    [Tooltip("El enemigo elegirá un ataque al azar de esta lista cada turno.")]
    public List<AbilityData> abilities;
}