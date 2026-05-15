using UnityEngine;

public class MyTower : MonoBehaviour
{
    public float maxHealth = 50f;
    private float currentHealth;

    [Header("Атака")]
    public float attackRange = 3f;
    public float damage = 8f;
    public float attackCooldown = 1.2f;
    private float lastAttackTime;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            AttackNearestEnemy();
        }
    }

    void AttackNearestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float closestDist = Mathf.Infinity;
        Enemy closestEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist && dist <= attackRange)
            {
                closestDist = dist;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            closestEnemy.TakeDamage(damage);
            lastAttackTime = Time.time;
            Debug.Log($"Башня атакует врага! Урон {damage}");
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"Башня получила урон {amount}. Осталось HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Башня разрушена!");

        // Оповещаем всех врагов, чтобы обновили цель
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.CheckTarget();
        }

        Destroy(gameObject);
    }
}