using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatPhase
{
    Off,
    Analysis,
    Programming, 
    Execution,   
    Resolution
}

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [Header("Estado del Combate")]
    public CombatPhase currentPhase = CombatPhase.Off;
    
    private bool playerReadyToExecute = false;

    public Dictionary<CharacterData, AbilityData> queuedPlayerActions = new Dictionary<CharacterData, AbilityData>();
    
    public CharacterData lastCharacterProgrammed = null;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartCombat()
    {
        Debug.Log("⚠️ INICIANDO RUTINA DE COMBATE (COMPROMISO SECUENCIAL) ⚠️");
        if (GameManager.Instance != null) GameManager.Instance.SetState(GameState.Combat);
        StartCoroutine(CombatLoopRoutine());
    }

    private IEnumerator CombatLoopRoutine()
    {
        Debug.Log("Cargando datos...");
        yield return new WaitForSeconds(1f);

        bool isBattleOver = false;

        while (!isBattleOver)
        {
            currentPhase = CombatPhase.Analysis;
            Debug.Log("<color=cyan>--- FASE 1: ANÁLISIS ---</color>");
            Debug.Log("> Enemigos deciden sus ataques...");
            yield return new WaitForSeconds(1.5f); 

            currentPhase = CombatPhase.Programming;
            Debug.Log("<color=yellow>--- FASE 2: PROGRAMACIÓN ---</color>");
            Debug.Log("> Programa las acciones. Recuerda: al programar un nuevo aliado, el anterior queda bloqueado.");
            
            playerReadyToExecute = false;
            queuedPlayerActions.Clear(); 
            
            lastCharacterProgrammed = null; 

            yield return new WaitUntil(() => playerReadyToExecute == true); 

            currentPhase = CombatPhase.Execution;
            Debug.Log("<color=red>--- FASE 3: EJECUCIÓN ---</color>");
            Debug.Log("> ¡Ejecutando todas las acciones programadas de golpe!");
            yield return new WaitForSeconds(2f);

            currentPhase = CombatPhase.Resolution;
            Debug.Log("<color=green>--- FASE 4: RESOLUCIÓN ---</color>");
            Debug.Log("> Evaluando bajas...");
            yield return new WaitForSeconds(1f);
            Debug.Log("------------------------------------");
        }
    }

    public void EndCombat()
    {
        currentPhase = CombatPhase.Off;
        if (GameManager.Instance != null) GameManager.Instance.SetState(GameState.Exploration);
    }

    public void QueuePlayerAction(CharacterData character, AbilityData ability)
    {
        if (currentPhase == CombatPhase.Programming)
        {
            if (queuedPlayerActions.ContainsKey(character))
            {
                queuedPlayerActions[character] = ability; 
                Debug.Log($"<color=yellow>[REPROGRAMADO]</color> {character.characterName} ahora usará: {ability.abilityName}");
            }
            else
            {
                queuedPlayerActions.Add(character, ability);
                Debug.Log($"<color=green>[PROGRAMADO]</color> {character.characterName} usará: {ability.abilityName}");
            }

            lastCharacterProgrammed = character;
        }
    }

    public void ExecuteAllActions()
    {
        if (currentPhase == CombatPhase.Programming)
        {
            Debug.Log("> Iniciando secuencia de choque...");
            playerReadyToExecute = true;
        }
    }
}