using UnityEngine;
using UnityEngine.Events;

public class CoreStructure : MonoBehaviour
{
    [Header("Параметры здоровья")]
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public float currentHealth;

    

    [Header("События")]
    public UnityEvent OnCoreDestroyed;

    [Header("Тестовый режим")]
    public bool testMode = true;
    public bool destroyOnClick = true;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"Ядро создано! Здоровье: {currentHealth}/{maxHealth}");
    }

    void Update()
    {
        // Тестовое разрушение по клавише R
        if (testMode && Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(999f);
        }
    }

    // Обработка кликов мышкой
    void OnMouseDown()
    {
        if (testMode && destroyOnClick)
        {
            TakeDamage(999f);
        }
    }

    // Получение урона
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log($"Ядро получило урон {damage}. Осталось HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Смерть ядра
    void Die()
    {
        Debug.Log("ЯДРО РАЗРУШЕНО! ИГРОК ПРОИГРАЛ.");

        // Вызываем событие разрушения (покажет панель поражения)
        OnCoreDestroyed?.Invoke();

        // Уничтожаем объект
        Destroy(gameObject);
    }
}