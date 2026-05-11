using System.Collections.Generic;
using UnityEngine;

public class EncounterSystem : MonoBehaviour
{
    [Header("Configuración de Encuentros")]
    [Range(0f, 100f)]
    public float encounterChance = 12f;
    public int safeSteps = 3; 

    [Header("Grupos de Enemigos Posibles")]
    [Tooltip("Arrastra aquí tus ScriptableObjects de EnemyGroupData")]
    public List<EnemyGroupData> possibleEncounters;

    private int currentSteps = 0;

    public void CheckEncounter()
    {
        currentSteps++;

        if (currentSteps <= safeSteps) return; 

        float roll = Random.Range(0f, 100f);
        
        if (roll <= encounterChance)
        {
            TriggerBattle();
        }
    }

    void TriggerBattle()
    {
        Debug.LogWarning("⚠️ ¡PANDILLA RIVAL ENCONTRADA! ⚠️ Iniciando combate...");
        currentSteps = 0;
        
        if (CombatManager.Instance != null)
        {
            EnemyGroupData selectedGroup = null;
            if (possibleEncounters.Count > 0)
            {
                int randomIndex = Random.Range(0, possibleEncounters.Count);
                selectedGroup = possibleEncounters[randomIndex];
            }

            CombatManager.Instance.StartCombat(selectedGroup);
        }
    }
}