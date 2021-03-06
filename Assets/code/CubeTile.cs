﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTile 
{
    GameObject[][][] grid;
    public int dimension { get; private set; }
    public int tileIndex { get; private set; }
    public int gridHashCode { get; private set; }
    public Vector3 sampleCoordinates { get; private set; }

    /// <summary>
    /// Creates a CubeTile.
    /// </summary>
    /// <param name="grid">The grid this tile will be sampled from.</param>
    /// <param name="index">The tile index identifying this CubeTile. Should be unique among all CubeTiles.</param>
    /// <param name="hashCode">The hashed names of all the modules contained in this tile. Should be unique among all CubeTiles.</param>
    public CubeTile(GameObject[][][] grid, int index, int hashCode)
    {
        this.grid = grid;
        this.dimension = grid[0].Length;
        this.tileIndex = index;
        this.gridHashCode = hashCode;
    }

    /// <summary>
    /// Creates a CubeTile.
    /// </summary>
    /// <param name="grid">The grid this tile will be sampled from.</param>
    /// <param name="index">The tile index identifying this CubeTile. Should be unique among all CubeTiles.</param>
    /// <param name="hashCode">The hashed names of all the modules contained in this tile. Should be unique among all CubeTiles.</param>
    /// <param name="sampleCoordinates">The coordinates in grid that this CubeTile is sampled from.</param>
    public CubeTile(GameObject[][][] grid, int index, int hashCode, Vector3 sampleCoordinates)
    {
        this.grid = grid;
        this.dimension = grid[0].Length;
        this.tileIndex = index;
        this.gridHashCode = hashCode;
        this.sampleCoordinates = sampleCoordinates;
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
