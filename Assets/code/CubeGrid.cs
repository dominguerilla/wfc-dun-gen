using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGrid 
{
    GameObject[][][] grid;
    Vector3 dimensions;
    int distanceBetweenModules;

    public CubeGrid(Vector3 firstTileLocation, Vector3 dimensions, int distanceBetweenModules)
    {
        this.grid = new GameObject[(int)dimensions.x][][];
        this.dimensions = dimensions;
        this.distanceBetweenModules = distanceBetweenModules;

        ScanInputGrid(firstTileLocation);
    }

    void ScanInputGrid(Vector3 firstTileLocation)
    {
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
    }

    public Dictionary<CubeTile, int> GetTileFrequencies(int tileSize)
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
                    CubeTile tile = CreateTile(new Vector3(x, y, z), tileSize, index);
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

    CubeTile CreateTile(Vector3 coordinates, int tileSize, int tileIndex)
    {
        int gridLength = grid[0].Length;
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
                    gridSample[tile_x][tile_y][tile_z] = grid[desiredX][desiredY][desiredZ];

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

}
