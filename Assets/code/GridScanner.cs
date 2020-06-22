using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScanner : MonoBehaviour
{
    [SerializeField]
    Transform inputGridStartLocation;

    [SerializeField]
    Transform tileSpawnLocation;

    private void Start()
    {
        int distanceBetweenModules = 10;
        GameObject[][][] grid = CreateGrid(inputGridStartLocation.position, new Vector3(4, 4, 4), distanceBetweenModules);
        List<CubeTile> tiles = GetCubeTiles(grid, 2);
        SpawnTiles(tiles, tileSpawnLocation.position, distanceBetweenModules);
    }

    GameObject[][][] CreateGrid(Vector3 firstTileLocation, Vector3 dimensions, int distanceBetweenModules)
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
                    Vector3 location = new Vector3(scanStartLocation.x + (distanceBetweenModules * x), scanStartLocation.y + (distanceBetweenModules * y), scanStartLocation.z + (distanceBetweenModules * z));
                    GameObject module = ScanForModule(location);
                    grid[x][y][z] = module;
                }
            }
        }
        return grid;
    }

    List<CubeTile> GetCubeTiles(GameObject[][][] grid, int tileSize)
    {
        List<CubeTile> tiles = new List<CubeTile>();
        int dimension = grid.Length;
        for (int x = 0; x < dimension; x++)
        {
            for (int y = 0; y < dimension; y++)
            {
                for (int z = 0; z < dimension; z++)
                {
                    CubeTile tile = CreateTile(grid, x, y, z, tileSize);
                    if(tile != null) tiles.Add(tile);
                }
            }
        }
        return tiles;
    }

    GameObject ScanForModule(Vector3 location)
    {
        int layerMask = 1 << 8;
        RaycastHit hit;
        if (Physics.Raycast(location, -Vector3.forward, out hit, 5f, layerMask, QueryTriggerInteraction.Collide))
        {
            return hit.transform.gameObject;
        }
        return null;
    }

    CubeTile CreateTile(GameObject[][][] cubeGrid, int x, int y, int z, int tileSize)
    {
        int gridLength = cubeGrid[0].Length;
        if (y + tileSize >= gridLength)
        {
            return null;
        }

        GameObject[][][] gridSample = new GameObject[tileSize][][];
        
        int tile_x = 0;
        for (int grid_x = x; grid_x < x + tileSize; grid_x++)
        {
            gridSample[tile_x] = new GameObject[tileSize][];
            int tile_y = 0;
            for (int grid_y = y; grid_y < y + tileSize; grid_y++)
            {
                gridSample[tile_x][tile_y] = new GameObject[tileSize];
                int tile_z = 0;
                for (int grid_z = z; grid_z < z + tileSize; grid_z++)
                {
                    int desiredX = grid_x % gridLength;
                    int desiredZ = grid_z % gridLength;
                    int desiredY = grid_y; // don't wrap around the Y dimension
                    gridSample[tile_x][tile_y][tile_z] = cubeGrid[desiredX][desiredY][desiredZ];
                    tile_z++;
                }
                tile_y++;
            }
            tile_x++;
        }

        return new CubeTile(gridSample);
    }

    void SpawnTile(Vector3 position, CubeTile cubeTile, int distanceBetweenModules)
    {
        int sideLength = cubeTile.dimension;
        
        for (int x = 0; x < sideLength; x++)
        {
            for (int y = 0; y < sideLength; y++)
            {
                for (int z = 0; z < sideLength; z++)
                {
                    WFCModule module = cubeTile.GetModule(x, y, z);
                    if (module)
                    {
                        Vector3 spawnOffset = new Vector3(x * distanceBetweenModules, y * distanceBetweenModules, z * distanceBetweenModules);
                        GameObject.Instantiate(module.gameObject, position + spawnOffset, module.transform.rotation);
                    }
                }
            }
        }
    }

    void SpawnTiles(List<CubeTile> tiles, Vector3 position, int distanceBetweenModules)
    {
        Vector3 offset = Vector3.zero;
        foreach(CubeTile tile in tiles) {
            SpawnTile(position + offset, tile, distanceBetweenModules);
            offset += new Vector3(0, 0, (distanceBetweenModules * 2) + 1);
        }
    }

    void PrintGrid(GameObject[][][] grid, Vector3 dimensions)
    {
        if (grid == null)
        {
            Debug.Log("Grid is empty!");
            return;
        }
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                for (int z = 0; z < dimensions.z; z++)
                {
                    GameObject obj = grid[x][y][z];
                    if (obj)
                    {
                        Debug.Log($"Cell [{x}][{y}][{z}]: {obj}");
                    }
                }
            }
        }
    }
}
