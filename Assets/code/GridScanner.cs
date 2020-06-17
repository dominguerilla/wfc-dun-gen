using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScanner : MonoBehaviour
{
    private void Start()
    {
        int tileSize = 10;
        CreateGrid(transform.position, new Vector3(4, 4, 4), tileSize);
    }

    public GameObject[][][] CreateGrid(Vector3 firstTileLocation, Vector3 dimensions, int tileSize)
    {
        GameObject[][][] grid = new GameObject[(int)dimensions.x][][];
        Vector3 scanStartLocation = firstTileLocation + (Vector3.forward * 5f);

        for (int x = 0; x < dimensions.x; x++)
        {
            grid[x] = new GameObject[(int)dimensions.y][];
            for (int y = 0; y < dimensions.y; y++)
            {
                grid[x][y] = new GameObject[(int)dimensions.z];
                for (int z = 0; z < dimensions.z; z++)
                {
                    Vector3 location = new Vector3(scanStartLocation.x + (tileSize * x), scanStartLocation.y + (tileSize * y), scanStartLocation.z + (tileSize * z));
                    GameObject tile = ScanForTile(location);
                    grid[x][y][z] = tile;
                    if (tile)
                    {
                        Debug.Log($"Found Tile {tile.name} at {x},{y},{z}!");
                    }
                }
            }
        }
        return null;
    }

    GameObject ScanForTile(Vector3 location)
    {
        int layerMask = 1 << 8;
        RaycastHit hit;
        if (Physics.Raycast(location, -Vector3.forward, out hit, 5f, layerMask, QueryTriggerInteraction.Collide))
        {
            return hit.transform.gameObject;
        }
        return null;
    }
}
