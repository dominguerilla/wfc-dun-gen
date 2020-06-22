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

    public WFCModule GetModule(int x, int y, int z)
    {
        GameObject obj = grid[x][y][z];
        if (obj)
        {
            WFCModule module = obj.GetComponent<WFCModule>();
            if (!module) {
                throw new System.InvalidOperationException($"No WFC module found in non-empty grid slot {x}, {y}, {z}!");
            }
            return module;
        }
        return null;
    }

    public override bool Equals(object obj)
    {        
        CubeTile otherTile = (CubeTile)obj;
        if (this.dimension != otherTile.dimension) return false;
        for (int x = 0; x < this.dimension; x++)
        {
            for (int y = 0; y < this.dimension; y++)
            {
                for (int z = 0; z < this.dimension; z++)
                {
                    WFCModule module1 = this.GetModule(x, y, z);
                    WFCModule module2 = otherTile.GetModule(x, y, z);
                    if (module1.id != module2.id)
                    {
                        return false;
                    }
                    
                }
            }
        }
        return true;
    }
}
