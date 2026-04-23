using UnityEngine;

public class PartySlotController : MonoBehaviour
{
    [Header("Datos del Personaje")]
    public CharacterData characterData; 

    public void OnSlotClicked()
    {
        if (characterData == null)
        {
            Debug.LogError("Este recuadro no tiene un CharacterData asignado.");
            return;
        }

        if (ActionMenuController.Instance != null)
        {
            ActionMenuController.Instance.OpenMenu(characterData);
        }
    }
}