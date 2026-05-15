using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StructureGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform crystal;
    [SerializeField] private MapGenerator mapGenerator;

    [Header("Structure Types")]
    [SerializeField] private List<StructureType> structureTypes;

    [Header("Global Settings")]
    [SerializeField] private GenerationMode mode = GenerationMode.TileBased;
    [SerializeField] private float globalMinDistanceFromCrystal = 5f;
    [SerializeField] private float globalMinDistanceBetweenStructures = 2f;

    [Header("Debug")]
    [SerializeField] private bool showDebugGizmos = true; // CS0414 Fixed below

    private List<GameObject> generatedStructures = new List<GameObject>();
    private Tilemap groundTilemap;
    private Dictionary<StructureType, List<Vector3>> placedPositions = new Dictionary<StructureType, List<Vector3>>();
    private List<Vector3Int> cachedValidTiles = new List<Vector3Int>();

    public enum GenerationMode
    {
        TileBased,
        FreePlacement
    }

    private void Start()
    {
        if (mapGenerator == null)
            mapGenerator = Object.FindAnyObjectByType<MapGenerator>(); // FIX: CS0618

        if (mapGenerator != null)
        {
            groundTilemap = mapGenerator.groundTilemap;
            CacheValidTiles();
        }

        GenerateStructures();
    }

    private void CacheValidTiles()
    {
        if (groundTilemap == null) return;
        cachedValidTiles.Clear();
        BoundsInt bounds = groundTilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                if (groundTilemap.HasTile(cellPos))
                {
                    cachedValidTiles.Add(cellPos);
                }
            }
        }
    }

    [ContextMenu("Generate Structures")]
    public void GenerateStructures()
    {
        ClearStructures();
        placedPositions.Clear();

        var sortedTypes = structureTypes.OrderByDescending(t => t.priority).ToList();

        foreach (var structureType in sortedTypes)
        {
            GenerateStructuresOfType(structureType);
        }
    }

    private void GenerateStructuresOfType(StructureType type)
    {
        if (type.prefab == null) return;

        placedPositions[type] = new List<Vector3>();
        int successCount = 0;

        for (int i = 0; i < type.count; i++)
        {
            Vector3? position = TryFindPositionForType(type);

            if (position.HasValue)
            {
                SpawnStructure(position.Value, type.prefab);
                placedPositions[type].Add(position.Value);
                successCount++;
            }
        }
    }

    private Vector3? TryFindPositionForType(StructureType type)
    {
        for (int attempt = 0; attempt < 30; attempt++)
        {
            Vector3 candidate;

            if (mode == GenerationMode.TileBased)
                candidate = GetRandomTilePosition();
            else
                candidate = GetRandomPositionInRing(type);

            if (IsPositionValidForType(candidate, type))
            {
                return candidate;
            }
        }
        return null;
    }

    private bool IsPositionValidForType(Vector3 position, StructureType type)
    {
        if (crystal == null) return false;

        float minDist = (type.useCustomGeneration && type.minDistanceFromCrystal > 0)
            ? type.minDistanceFromCrystal
            : globalMinDistanceFromCrystal;

        float distToCrystal = Vector3.Distance(position, crystal.position);
        if (distToCrystal < minDist) return false;

        if (type.useCustomArea && (distToCrystal < type.customInnerRadius || distToCrystal > type.customOuterRadius))
            return false;

        if (mapGenerator != null)
        {
            float halfWidth = (mapGenerator.width * mapGenerator.cellSize) / 2f;
            float halfHeight = (mapGenerator.height * mapGenerator.cellSize) / 2f;
            if (Mathf.Abs(position.x) > halfWidth || Mathf.Abs(position.y) > halfHeight)
                return false;
        }

        float minDistToOthers = (type.useCustomGeneration && type.minDistanceBetweenAny > 0)
            ? type.minDistanceBetweenAny
            : globalMinDistanceBetweenStructures;

        foreach (var existing in generatedStructures)
        {
            if (existing != null && Vector3.Distance(position, existing.transform.position) < minDistToOthers)
                return false;
        }

        if (type.minDistanceBetweenSameType > 0 && placedPositions.ContainsKey(type))
        {
            foreach (var existingPos in placedPositions[type])
            {
                if (Vector3.Distance(position, existingPos) < type.minDistanceBetweenSameType)
                    return false;
            }
        }

        return true;
    }

    private Vector3 GetRandomTilePosition()
    {
        if (cachedValidTiles.Count == 0) return Vector3.zero;

        Vector3Int randomCell = cachedValidTiles[Random.Range(0, cachedValidTiles.Count)];
        return groundTilemap.GetCellCenterWorld(randomCell);
    }

    private Vector3 GetRandomPositionInRing(StructureType type)
    {
        float innerRadius = (type.useCustomGeneration && type.useCustomArea)
            ? type.customInnerRadius
            : globalMinDistanceFromCrystal;

        float outerRadius = (type.useCustomGeneration && type.useCustomArea)
            ? type.customOuterRadius
            : Mathf.Max(mapGenerator.width, mapGenerator.height) * mapGenerator.cellSize / 1.5f;

        float angle = Random.Range(0f, Mathf.PI * 2);
        float radius = Mathf.Sqrt(Random.Range(innerRadius * innerRadius, outerRadius * outerRadius));

        return new Vector3(
            crystal.position.x + Mathf.Cos(angle) * radius,
            crystal.position.y + Mathf.Sin(angle) * radius,
            0
        );
    }

    private void SpawnStructure(Vector3 position, GameObject prefab)
    {
        GameObject structure = Instantiate(prefab, position, Quaternion.identity);
        structure.transform.SetParent(transform);
        generatedStructures.Add(structure);
    }

    private void ClearStructures()
    {
        foreach (var structure in generatedStructures)
        {
            if (structure != null) DestroyImmediate(structure);
        }
        generatedStructures.Clear();
    }

    private void OnDrawGizmos()
    {
        // FIX: CS0414 - showDebugGizmos is now used
        if (!showDebugGizmos || crystal == null) return;

        foreach (var type in structureTypes)
        {
            if (type.useCustomGeneration && type.useCustomArea)
            {
                Gizmos.color = new Color(1f, 1f, 1f, 0.15f);
                DrawCircle(crystal.position, type.customInnerRadius, type.customOuterRadius);
            }
        }

        Gizmos.color = Color.red;
        DrawCircle(crystal.position, globalMinDistanceFromCrystal, globalMinDistanceFromCrystal);
    }

    private void DrawCircle(Vector3 center, float innerRadius, float outerRadius)
    {
        int segments = 36;
        float angleStep = 360f / segments;

        for (int r = 0; r <= 1; r++)
        {
            float radius = r == 0 ? innerRadius : outerRadius;
            Vector3 prevPoint = center + Vector3.right * radius;

            for (int i = 1; i <= segments; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 newPoint = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                Gizmos.DrawLine(prevPoint, newPoint);
                prevPoint = newPoint;
            }
        }
    }
}