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

public struct PlayerAction 
{
    public AbilityData ability;
    public EnemyWindowController target;
}

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [Header("Estado del Combate")]
    public CombatPhase currentPhase = CombatPhase.Off;
    
    private bool playerReadyToExecute = false;

    public Dictionary<CharacterData, PlayerAction> queuedPlayerActions = new Dictionary<CharacterData, PlayerAction>();
    public CharacterData lastCharacterProgrammed = null;

    [Header("Sistema de Targeting")]
    public CharacterData pendingCharacter;
    public AbilityData pendingAbility;

    [Header("Spawns de Enemigos (UI)")]
    [Tooltip("Arrastra aquí Layout_1, Layout_2, Layout_3 y Layout_4 en orden")]
    public GameObject[] enemyLayouts;

    public EnemyGroupData currentEnemyGroup;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartCombat(EnemyGroupData enemyGroup = null)
    {
        Debug.Log("⚠️ INICIANDO RUTINA DE COMBATE ⚠️");
        if (GameManager.Instance != null) GameManager.Instance.SetState(GameState.Combat);
        
        currentEnemyGroup = enemyGroup;
        
        SpawnEnemies();

        StartCoroutine(CombatLoopRoutine());
    }

    private void SpawnEnemies()
    {
        foreach (var layout in enemyLayouts)
        {
            if (layout != null) layout.SetActive(false);
        }

        if (currentEnemyGroup == null || currentEnemyGroup.enemies == null || currentEnemyGroup.enemies.Count == 0)
        {
            Debug.LogWarning("No hay enemigos en el grupo.");
            return;
        }

        int enemyCount = currentEnemyGroup.enemies.Count;
        
        int layoutIndex = enemyCount - 1;
        
        if (layoutIndex >= 0 && layoutIndex < enemyLayouts.Length)
        {
            GameObject activeLayout = enemyLayouts[layoutIndex];
            activeLayout.SetActive(true);

            EnemyWindowController[] windows = activeLayout.GetComponentsInChildren<EnemyWindowController>(true);
            
            for (int i = 0; i < enemyCount; i++)
            {
                if (i < windows.Length)
                {
                    windows[i].SetupEnemy(currentEnemyGroup.enemies[i], true); 
                }
            }
        }
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
            
            foreach (KeyValuePair<CharacterData, PlayerAction> entry in queuedPlayerActions)
            {
                CharacterData attacker = entry.Key;
                PlayerAction action = entry.Value;

                if (action.target != null && action.target.gameObject.activeSelf)
                {
                    int damageCalculated = attacker.attack + action.ability.power; 
                    
                    Debug.Log($"⚔️ {attacker.characterName} usó [{action.ability.abilityName}] contra {action.target.currentEnemy.enemyName}!");
                    
                    action.target.TakeDamage(damageCalculated);
                }
                else
                {
                    Debug.Log($"⚠️ El ataque de {attacker.characterName} falló: el objetivo ya no existe.");
                }
            }
            yield return new WaitForSeconds(3f);

            currentPhase = CombatPhase.Resolution;
            Debug.Log("<color=green>--- FASE 4: RESOLUCIÓN ---</color>");
            Debug.Log("> Evaluando bajas...");
            if (CheckIfAllEnemiesAreDead())
            {
                Debug.Log("🏆 ¡Procesos eliminados! Has ganado el combate.");
                isBattleOver = true;
                EndCombat();
            }
            yield return new WaitForSeconds(1f);
            Debug.Log("------------------------------------");
        }
    }

    private bool CheckIfAllEnemiesAreDead()
    {
        foreach (var layout in enemyLayouts)
        {
            if (layout != null && layout.activeSelf)
            {
                EnemyWindowController[] aliveEnemies = layout.GetComponentsInChildren<EnemyWindowController>();
                
                if (aliveEnemies.Length == 0) return true;
                else return false;
            }
        }
        return true;
    }

    public void EndCombat()
    {
        currentPhase = CombatPhase.Off;
        if (GameManager.Instance != null) GameManager.Instance.SetState(GameState.Exploration);
        
        foreach (var layout in enemyLayouts)
        {
            if (layout != null) layout.SetActive(false);
        }
    }

    public void PreparePlayerAction(CharacterData character, AbilityData ability)
    {
        if (currentPhase == CombatPhase.Programming)
        {
            pendingCharacter = character;
            pendingAbility = ability;
            Debug.Log($"<color=cyan>[ESPERANDO OBJETIVO]</color> Selecciona a qué virus vas a atacar con {ability.abilityName}...");
        }
    }

    public void SelectTarget(EnemyWindowController enemyTarget)
    {
        if (currentPhase == CombatPhase.Programming && pendingCharacter != null && pendingAbility != null)
        {
            PlayerAction newAction = new PlayerAction { ability = pendingAbility, target = enemyTarget };
            
            queuedPlayerActions[pendingCharacter] = newAction; 
            
            Debug.Log($"<color=green>[PROGRAMADO]</color> {pendingCharacter.characterName} atacará a {enemyTarget.currentEnemy.enemyName} con {pendingAbility.abilityName}");

            lastCharacterProgrammed = pendingCharacter;

            pendingCharacter = null;
            pendingAbility = null;
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