using NUnit.Framework;
using UnityEngine;

public class TowerManagerDirectTests
{
    private GameObject testHolder;
    private TowerManager towerManager;

    [SetUp]
    public void SetUp()
    {
        // Просто создаем пустой объект и вешаем на него менеджер башен
        testHolder = new GameObject("TestTowerManager");
        towerManager = testHolder.AddComponent<TowerManager>();
    }

    [TearDown]
    public void TearDown()
    {
        // Чистим за собой объект
        Object.DestroyImmediate(testHolder);
    }

    [Test]
    public void TowerManager_Component_InitializesCorrectly()
    {
        // Проверяем, что компонент успешно создался на объекте
        Assert.IsNotNull(towerManager, "Компонент TowerManager не был инициализирован!");
    }
}