using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 40f;
    public float currentHealth;
    public float speed = 2f;
    public float damage = 10f;
    public float attackCooldown = 1f;
    public float attackRange = 1.2f;

    private float lastAttackTime;
    private Transform target;
    private CoreStructure core;
    private MyTower targetTower;
    private float searchTimer;

    // Границы карты (20x15 клеток, cell size = 2)
    private float minX = -20f;
    private float maxX = 20f;
    private float minY = -15f;
    private float maxY = 15f;

    void Start()
    {
        currentHealth = maxHealth;
        FindTarget();
    }

    void Update()
    {
        searchTimer += Time.deltaTime;
        if (searchTimer >= 2f)
        {
            searchTimer = 0f;
            FindTarget();
        }

        if (target == null)
        {
            FindTarget();
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > attackRange)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
            transform.position = newPos;
        }
        else if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void FindTarget()
    {
        MyTower[] towers = FindObjectsOfType<MyTower>();
        float closestDistance = Mathf.Infinity;
        MyTower closestTower = null;

        foreach (MyTower tower in towers)
        {
            if (tower == null) continue;
            float dist = Vector3.Distance(transform.position, tower.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestTower = tower;
            }
        }

        if (closestTower != null)
        {
            targetTower = closestTower;
            target = closestTower.transform;
            core = null;
            return;
        }

        GameObject coreObj = GameObject.FindGameObjectWithTag("Crystal");
        if (coreObj != null)
        {
            core = coreObj.GetComponent<CoreStructure>();
            if (core != null)
            {
                target = coreObj.transform;
                targetTower = null;
                Debug.Log("Враг идёт к ядру!");
                return;
            }
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        if (targetTower != null)
            targetTower.TakeDamage(damage);
        else if (core != null)
            core.TakeDamage(damage);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    public void CheckTarget()
    {
        FindTarget();
    }
}