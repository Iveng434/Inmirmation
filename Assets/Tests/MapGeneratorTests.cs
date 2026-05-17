using NUnit.Framework;
using UnityEngine;

public class MapGeneratorTests
{
    private GameObject testHolder;
    private MapGenerator mapGenerator;

    [SetUp]
    public void SetUp()
    {
        testHolder = new GameObject("TestMapGenerator");
        mapGenerator = testHolder.AddComponent<MapGenerator>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(testHolder);
    }

    [Test]
    public void MapGenerator_Component_InitializesCorrectly()
    {
        Assert.IsNotNull(mapGenerator, "Компонент MapGenerator не добавился на объект!");
    }

    [Test]
    public void MapGenerator_DefaultValues_AreValid()
    {
        // Проверяем, что базовый объект карты создается в нулевых координатах
        Assert.AreEqual(Vector3.zero, mapGenerator.transform.position, "Стартовая позиция генератора карты смещена!");
    }

    [Test]
    public void MapGenerator_GameObject_ActiveInHierarchy()
    {
        // Проверяем, что объект активен и не выключен в инспекторе
        Assert.IsTrue(mapGenerator.gameObject.activeSelf, "Объект генератора карты скрыт на сцене!");
    }
}