using UnityEngine;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    public List<GameObject> towerPrefabs;
    public MapGenerator mapGen;
    private int selectedTowerIndex = 0;
    public LayerMask obstacleLayer;

    private float minX = -20f;
    private float maxX = 20f;
    private float minY = -20f;
    private float maxY = 20f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedTowerIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedTowerIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedTowerIndex = 2;

        if (Input.GetMouseButtonDown(1))
        {
            PlaceTower();
        }
    }

    void PlaceTower()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        worldPos.z = 0;

        Collider2D hit = Physics2D.OverlapCircle(worldPos, 1.5f, obstacleLayer);

        if (hit != null && hit.GetComponent<MyTower>() != null)
        {
            Debug.Log("Улучшение временно отключено");
            return;
        }

        if (mapGen == null) return;
        Vector2Int gridPos = mapGen.GetGridPos(worldPos);
        Vector3 spawnPos = mapGen.GetCellWorldPosition(gridPos.x, gridPos.y);

        if (spawnPos.x < minX || spawnPos.x > maxX || spawnPos.y < minY || spawnPos.y > maxY)
        {
            Debug.Log("За границами карты!");
            return;
        }

        if (selectedTowerIndex >= 0 && selectedTowerIndex < towerPrefabs.Count)
        {
            Instantiate(towerPrefabs[selectedTowerIndex], spawnPos, Quaternion.identity);
        }
    }
}