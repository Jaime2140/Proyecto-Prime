using UnityEngine;

public class EncounterSystem : MonoBehaviour
{
    [Header("Configuración de Encuentros")]
    [Tooltip("Probabilidad del 0 al 100 de que salga un enemigo al dar un paso")]
    [Range(0f, 100f)]
    public float encounterChance = 12f;

    [Tooltip("Pasos de 'gracia' donde es imposible que salgan enemigos")]
    public int safeSteps = 3; 

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
            CombatManager.Instance.StartCombat();
        }
        else
        {
            Debug.LogError("No se encontró el CombatManager en la escena.");
        }
    }
}