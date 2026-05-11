using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EnemyWindowController : MonoBehaviour, IPointerClickHandler
{
    [Header("Referencias de la Ventana")]
    public Image enemyImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;

    [Header("Estado Interno (No tocar)")]
    public EnemyData currentEnemy;
    public int currentHP;
    
    public bool isFrontRow = true; 

    public void SetupEnemy(EnemyData data, bool inFrontRow)
    {
        currentEnemy = data;
        currentHP = data.maxHP;
        isFrontRow = inFrontRow;

        if (enemyImage != null && data.enemySprite != null)
        {
            enemyImage.sprite = data.enemySprite;
            enemyImage.SetNativeSize(); 
        }

        if (nameText != null) 
        {
            nameText.text = data.enemyName;
        }

        UpdateHPText();
        
        gameObject.SetActive(true); 
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        if (currentHP < 0) currentHP = 0;

        UpdateHPText();
        Debug.Log($"💻 {currentEnemy.enemyName} recibió {damageAmount} de daño. HP: {currentHP}");

        if (currentHP == 0)
        {
            Die();
        }
    }

    private void UpdateHPText()
    {
        if (hpText != null)
        {
            hpText.text = $"HP: {currentHP}/{currentEnemy.maxHP}";
        }
    }

    private void Die()
    {
        Debug.Log($"💀 Proceso terminado: {currentEnemy.enemyName} ha sido eliminado.");
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CombatManager.Instance != null)
        {
            CombatManager.Instance.SelectTarget(this);
        }
    }
}