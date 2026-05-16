using System;
using UnityEngine;
using UnityEngine.UI;

public class test_button : MonoBehaviour
{
    public HealthBarPanel healthBar;
    public Button test_1;
    public int damage = 15;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update()
    {
            
    }
    private void FixedUpdate()
    {
        test_1.onClick.AddListener(takeddamage);
    }
    void takeddamage()
    {
        healthBar.TakeDamage(damage);
    }
}
