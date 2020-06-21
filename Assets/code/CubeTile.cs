using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTile 
{
    GameObject[][][] grid;
    public int dimension { get; private set; }

    public CubeTile(GameObject[][][] grid)
    {
        this.grid = grid;
        this.dimension = grid[0].Length;
    }

    public GameObject GetModule(int x, int y, int z)
    {
        return grid[x][y][z];
    }
}
