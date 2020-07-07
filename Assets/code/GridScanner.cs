using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridScanner : MonoBehaviour
{
    [SerializeField]
    Transform inputGridStartLocation;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(inputGridStartLocation.position, 0.5f);
    }

    public CubeGrid ScanAndCreateGrid(Vector3 dimensions, int distanceBetweenModules)
    {
        return new CubeGrid(inputGridStartLocation.position, dimensions, distanceBetweenModules);
    }

    void SpawnTile(Vector3 position, CubeTile cubeTile, int distanceBetweenModules)
    {
        int sideLength = cubeTile.dimension;
        GameObject tileObj = new GameObject(cubeTile.gridHashCode.ToString());
        for (int x = 0; x < sideLength; x++)
        {
            for (int y = 0; y < sideLength; y++)
            {
                for (int z = 0; z < sideLength; z++)
                {
                    WFCModule module = cubeTile.GetModule(x, y, z);
                    if (module  != null)
                    {
                        Vector3 spawnOffset = new Vector3(x * distanceBetweenModules, y * distanceBetweenModules, z * distanceBetweenModules);
                        GameObject spawnedModule = GameObject.Instantiate(module.gameObject, position + spawnOffset, module.transform.rotation);
                        spawnedModule.transform.SetParent(tileObj.transform, true);
                    }
                }
            }
        }
    }

    public void SpawnTiles(List<CubeTile> tiles, Vector3 position, int distanceBetweenModules)
    {
        Vector3 offset = Vector3.zero;
        foreach(CubeTile tile in tiles) {
            SpawnTile(position + offset, tile, distanceBetweenModules);
            offset += new Vector3(0, 0, (distanceBetweenModules * 2) + 10);
        }
    }

    public void PrintTileFrequencies(Dictionary<CubeTile, int> tileFrequencies)
    {
        foreach (KeyValuePair<CubeTile, int> kv in tileFrequencies)
        {
            Debug.Log($"Tile {kv.Key.tileIndex} frequency: {kv.Value}");
        }
    }
}
