using UnityEngine;
using TMPro;

public class ActionMenuController : MonoBehaviour
{
    public static ActionMenuController Instance;

    [Header("Referencias UI")]
    public GameObject actionMenuPanel;
    
    [Header("Textos de los Botones")]
    public TextMeshProUGUI textMove1;
    public TextMeshProUGUI textMove2;
    public TextMeshProUGUI textMove3;
    public TextMeshProUGUI textMove4;

    private CharacterData currentCharacter;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start() { CloseMenu(); }

    public void OpenMenu(CharacterData character)
    {
        if (CombatManager.Instance.currentPhase != CombatPhase.Programming) return;

        if (CombatManager.Instance.queuedPlayerActions.ContainsKey(character))
        {
            if (CombatManager.Instance.lastCharacterProgrammed != character)
            {
                Debug.LogWarning($"[SISTEMA BLOQUEADO] La acción de {character.characterName} quedó sellada al darle una orden a otro aliado.");
                return;
            }
        }

        currentCharacter = character;

        RectTransform rect = actionMenuPanel.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(105f, 96.5f);

        if (character.baseAttack != null) textMove1.text = character.baseAttack.abilityName;
        if (character.skill != null) textMove2.text = character.skill.abilityName;
        if (character.guard != null) textMove3.text = character.guard.abilityName;
        if (character.ultimate != null) textMove4.text = character.ultimate.abilityName;

        actionMenuPanel.SetActive(true);
    }

    public void CloseMenu() { actionMenuPanel.SetActive(false); }

    public void OnAction1Selected() { SendActionToQueue(currentCharacter.baseAttack); }
    public void OnAction2Selected() { SendActionToQueue(currentCharacter.skill); }
    public void OnAction3Selected() { SendActionToQueue(currentCharacter.guard); }
    public void OnAction4Selected() { SendActionToQueue(currentCharacter.ultimate); }

    private void SendActionToQueue(AbilityData selectedAbility)
    {
        if (selectedAbility == null) return; 

        CloseMenu();

        if (CombatManager.Instance != null)
        {
            CombatManager.Instance.PreparePlayerAction(currentCharacter, selectedAbility);
        }
    }
}