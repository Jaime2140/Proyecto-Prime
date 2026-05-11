using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Grupo Enemigo", menuName = "RPG/Grupo Enemigo")]
public class EnemyGroupData : ScriptableObject
{
    [Header("Información del Encuentro")]
    public string groupName;

    [Header("Integrantes")]
    [Tooltip("La cantidad de enemigos aquí (1 a 5) le dirá al código qué Layout de SpawnPoints debe encender.")]
    public List<EnemyData> enemies;
}