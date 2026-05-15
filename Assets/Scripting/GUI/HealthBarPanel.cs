using UnityEngine;
using UnityEngine.UI;
public class HealthBarPanel : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private float maxHealth = 100;
    private float currentHealth = 100;
    public bool deafeated = false;
    [SerializeField] public CoreStructure Core_structure;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (fillImage == null)
            fillImage = GetComponent<Image>();
        UpdateFill();
    }
    public void UpdateFill()
    {
        if (fillImage != null)
            fillImage.fillAmount = currentHealth / maxHealth;
    }
    public void SetFillPercent(float percent)
    {
        if (fillImage != null)
            fillImage.fillAmount = Mathf.Clamp01(percent);
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = Core_structure.currentHealth;
        maxHealth = Core_structure.maxHealth;
        UpdateFill();
    }
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            
        }
    }
    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateFill();
        }
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
    }
    public void Heal(int amount)
    {
        CurrentHealth += amount;
    }

}
