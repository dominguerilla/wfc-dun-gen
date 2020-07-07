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

    public GameObject[][][] ScanAndCreateGrid(Vector3 dimensions, int distanceBetweenModules)
    {
        return CreateGrid(inputGridStartLocation.position, dimensions, distanceBetweenModules);
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

    public Dictionary<CubeTile, int>  GetTileFrequencies(GameObject[][][] grid, int tileSize)
    {
        Dictionary<CubeTile, int> tileFrequencies = new Dictionary<CubeTile, int>(new CubeTileComparer());
        int dimension = grid.Length;
        int index = 0;
        for (int x = 0; x < dimension; x++)
        {
            for (int y = 0; y < dimension; y++)
            {
                for (int z = 0; z < dimension; z++)
                {
                    CubeTile tile = CreateTile(grid, new Vector3(x, y, z), tileSize, index);
                    if (tile == null) continue;
                    if (tileFrequencies.ContainsKey(tile))
                    {
                        tileFrequencies[tile] += 1;
                    }
                    else
                    {
                        tileFrequencies[tile] = 1;
                    }
                    index++;
                }
            }
        }
        return tileFrequencies;
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

    CubeTile CreateTile(GameObject[][][] cubeGrid, Vector3 coordinates, int tileSize, int tileIndex)
    {
        int gridLength = cubeGrid[0].Length;
        int x = (int)coordinates.x;
        int y = (int)coordinates.y;
        int z = (int)coordinates.z;
        if (y + tileSize >= gridLength)
        {
            return null;
        }

        GameObject[][][] gridSample = new GameObject[tileSize][][];
        string moduleNames = "";

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

                    string moduleName = gridSample[tile_x][tile_y][tile_z] != null ? gridSample[tile_x][tile_y][tile_z].GetComponent<WFCModule>().id : " ";
                    moduleNames += " " + moduleName;

                    tile_z++;
                }
                tile_y++;
            }
            tile_x++;
        }

        int hashCode = moduleNames.GetHashCode();
        return new CubeTile(gridSample, tileIndex, hashCode, coordinates);
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

    public void SpawnTiles(Dictionary<CubeTile, int> tiles, Vector3 position, int distanceBetweenModules)
    {
        Vector3 offset = Vector3.zero;
        foreach(CubeTile tile in tiles.Keys) {
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
