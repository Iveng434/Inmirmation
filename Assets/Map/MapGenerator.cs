using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class MapGenerator : MonoBehaviour
{
    [Header("Настройки Поля")]
    public int width = 20;
    public int height = 15;
    public float cellSize = 2f;

    [Header("Визуал")]
    public Tilemap groundTilemap;
    public Tile groundTile;

    [Header("Опционально: Сетка")]
    public Tilemap gridTilemap;
    public Tile gridTile;
    public bool showGrid = true;

    void Start()
    {
        GenerateMap();
        SetupGrid();
    }

    public void GenerateMap()
    {
        if (groundTilemap == null || groundTile == null)
        {
            Debug.LogError("MapGenerator: Please assign Ground Tilemap and Ground Tile in the Inspector!");
            return;
        }

        ClearMap();

        Grid grid = GetComponent<Grid>();
        if (grid != null)
        {
            grid.cellSize = new Vector3(cellSize, cellSize, 0);
        }

        int xOffset = width / 2;
        int yOffset = height / 2;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePos = new Vector3Int(x - xOffset, y - yOffset, 0);
                groundTilemap.SetTile(tilePos, groundTile);

                if (showGrid && gridTile != null && gridTilemap != null)
                {
                    gridTilemap.SetTile(tilePos, gridTile);
                }
            }
        }
    }

    void SetupGrid()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float fieldHeight = height * cellSize;
            if (mainCamera.orthographic)
            {
                mainCamera.orthographicSize = fieldHeight / 2 + 1;
            }
            mainCamera.transform.position = new Vector3(0, 0, -10);
        }
    }

    public Vector2Int GetGridPos(Vector3 worldPosition)
    {
        if (groundTilemap == null) return Vector2Int.zero;
        Vector3Int cellPos = groundTilemap.WorldToCell(worldPosition);
        return new Vector2Int(cellPos.x, cellPos.y);
    }

    public Vector3 GetCellWorldPosition(int x, int y)
    {
        if (groundTilemap == null) return Vector3.zero;
        Vector3Int tilePos = new Vector3Int(x, y, 0);
        return groundTilemap.GetCellCenterWorld(tilePos);
    }

    void ClearMap()
    {
        if (groundTilemap != null) groundTilemap.ClearAllTiles();
        if (gridTilemap != null) gridTilemap.ClearAllTiles();
    }
}