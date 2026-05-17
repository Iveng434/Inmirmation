using NUnit.Framework;
using UnityEngine;

public class EnemyTests
{
    private GameObject testHolder;
    private Enemy enemy;

    [SetUp]
    public void SetUp()
    {
        // Создаем виртуального врага для теста
        testHolder = new GameObject("TestEnemy");
        enemy = testHolder.AddComponent<Enemy>();

        // Вручную задаем здоровье, так как метод Start в EditMode-тестах не вызывается сам
        enemy.maxHealth = 40f;
        enemy.currentHealth = enemy.maxHealth;
    }

    [TearDown]
    public void TearDown()
    {
        // Удаляем объект после теста
        Object.DestroyImmediate(testHolder);
    }

    [Test]
    public void Enemy_TakeDamage_ReducesCurrentHealth()
    {
        // Arrange (Враг имеет 40 HP, нанесем 15 урона)
        float damageAmount = 15f;
        float expectedHealth = 40f - 15f; // Должно остаться 25

        // Act
        enemy.TakeDamage(damageAmount);

        // Assert
        Assert.AreEqual(expectedHealth, enemy.currentHealth, "Здоровье врага уменьшилось неверно после получения урона!");
    }
}