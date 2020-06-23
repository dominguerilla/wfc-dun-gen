using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTile 
{
    GameObject[][][] grid;
    public int dimension { get; private set; }
    public int tileIndex { get; private set; }
    public int gridHashCode { get; private set; }

    public CubeTile(GameObject[][][] grid, int index, int hashCode)
    {
        this.grid = grid;
        this.dimension = grid[0].Length;
        this.tileIndex = index;
        this.gridHashCode = hashCode;
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
}

public class CubeTileComparer : IEqualityComparer<CubeTile>
{
    public bool Equals(CubeTile t1, CubeTile t2)
    {

        if (t1.dimension != t2.dimension) return false;
        for (int x = 0; x < t1.dimension; x++)
        {
            for (int y = 0; y < t1.dimension; y++)
            {
                for (int z = 0; z < t2.dimension; z++)
                {
                    WFCModule module1 = t1.GetModule(x, y, z);
                    WFCModule module2 = t2.GetModule(x, y, z);
                    if (module1 == null && module2 != null) return false;
                    if (module1 != null && module2 == null) return false;
                    if (module1 == null && module2 == null) return true;
                    if (module1.id != module2.id)
                    {
                        return false;
                    }

                }
            }
        }
        return true;
    }

    public int GetHashCode(CubeTile obj)
    {
        return obj.gridHashCode;
    }
}
