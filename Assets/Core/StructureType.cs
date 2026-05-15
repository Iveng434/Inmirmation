using UnityEngine;

[System.Serializable]
public enum StructureTypeEnum
{
    Forest,
    Sea,
    Mountains,
    HighMountains,
    None
}

[System.Serializable]
public class StructureType
{
    public StructureTypeEnum type;

    // Поля, которые мы нашли по ошибкам в консоли
    public GameObject prefab;
    public int count;
    public int priority; // ВОТ ОНО! Добавь эту строчку.

    // Все остальные параметры для генерации
    public float minDistanceBetweenAny;
    public float minDistanceBetweenSameType;
    public float customInnerRadius;
    public float customOuterRadius;
    public bool useCustomGeneration;
    public bool useCustomArea;
    public float minDistanceFromCrystal;
    public float maxDistanceFromCrystal;
}