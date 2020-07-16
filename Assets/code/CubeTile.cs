using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ADJACENT_DIRECTION
{
    FORWARD,
    BACK,
    LEFT,
    RIGHT,
    UP,
    DOWN
}

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
    /// <param name="sampleCoordinates">The coordinates in grid that this CubeTile is sampled from.</param>
    public CubeTile(GameObject[][][] grid, int index, int hashCode, Vector3 sampleCoordinates)
    {
        this.grid = grid;
        this.dimension = grid[0].Length;
        this.tileIndex = index;
        this.gridHashCode = hashCode;
        this.sampleCoordinates = sampleCoordinates;
    }

    public CubeTile(GameObject[][][] grid, int index, int hashCode)
    {
        this.grid = grid;
        this.dimension = grid[0].Length;
        this.tileIndex = index;
        this.gridHashCode = hashCode;
        this.sampleCoordinates = Vector3.zero;
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

    public bool isCompatible(CubeTile other, ADJACENT_DIRECTION direction)
    {
        if (other.dimension != this.dimension)
        {
            throw new System.Exception("CubeTiles do not match in dimensions!");
        }

        Vector3 thisStartIndices = Vector3.zero, otherStartIndices = Vector3.zero;
        Vector3 thisEndIndices = Vector3.zero, otherEndIndices = Vector3.zero;

        //TODO: double-check these.
        switch (direction)
        {
            case ADJACENT_DIRECTION.FORWARD:
                /*
                 compare all of this.grid[1...dimension][0...dimension][0...dimension]
                 with other.grid[0...dimension-1][0...dimension][0...dimension]
                 */
                thisStartIndices = new Vector3(1, 0, 0);
                thisEndIndices = new Vector3(dimension, dimension, dimension);
                otherStartIndices = new Vector3(0, 0, 0);
                otherEndIndices = new Vector3(dimension - 1, dimension, dimension);
                break;
            case ADJACENT_DIRECTION.BACK:
                /*
                 * compare all of this.grid[0...dimension-1][0...dimension][0...dimension]
                 * with other.grid[1...dimension][0...dimension][0...dimension]
                 */
                thisStartIndices = new Vector3(0, 0, 0);
                thisEndIndices = new Vector3(dimension-1, dimension, dimension);
                otherStartIndices = new Vector3(1, 0, 0);
                otherEndIndices = new Vector3(dimension, dimension, dimension);
                break;
            case ADJACENT_DIRECTION.LEFT:
                /*
                 * compare all of this.grid[0...dimension][0...dimension][0...dimension-1]
                 * with other.grid[0...dimension][0...dimension][1...dimension]
                 */
                thisStartIndices = new Vector3(0, 0, 0);
                thisEndIndices = new Vector3(dimension, dimension, dimension-1);
                otherStartIndices = new Vector3(0, 0, 1);
                otherEndIndices = new Vector3(dimension, dimension, dimension);
                break;
            case ADJACENT_DIRECTION.RIGHT:
                /*
                 * compare all of this.grid[0...dimension][0...dimension][1...dimension]
                 * with other.grid[0...dimension][0...dimension][0...dimension-1]
                 */
                thisStartIndices = new Vector3(0, 0, 1);
                thisEndIndices = new Vector3(dimension, dimension, dimension);
                otherStartIndices = new Vector3(0, 0, 0);
                otherEndIndices = new Vector3(dimension, dimension, dimension-1);
                break;
            case ADJACENT_DIRECTION.UP:
                /*
                 * compare all of this.grid[0...dimension][1...dimension][0...dimension]
                 * with other.grid[0...dimension][0...dimension-1][0...dimension]
                 */
                thisStartIndices = new Vector3(0, 1, 0);
                thisEndIndices = new Vector3(dimension, dimension, dimension);
                otherStartIndices = new Vector3(0, 0, 0);
                otherEndIndices = new Vector3(dimension, dimension-1, dimension);
                break;
            case ADJACENT_DIRECTION.DOWN:
                /*
                 * compare all of this.grid[0...dimension][0..dimension-1][0...dimension]
                 * with other.grid[0...dimension][1...dimension][0...dimension]
                 */
                thisStartIndices = new Vector3(0, 0, 0);
                thisEndIndices = new Vector3(dimension, dimension-1, dimension);
                otherStartIndices = new Vector3(0, 1, 0);
                otherEndIndices = new Vector3(dimension, dimension, dimension);
                break;
            default:
                throw new System.Exception("Invalid adjacent direction!");
        }

        /*
         * 
                thisStartIndices = new Vector3(0, 0, 0);
                thisEndIndices = new Vector3(dimension, dimension, dimension-1);
                otherStartIndices = new Vector3(0, 0, 1);
                otherEndIndices = new Vector3(dimension, dimension, dimension);

        x = 0
        y = 0
        z = 0
        otherX = 0
        otherY = 0
        otherZ = 1
         */



        int x = (int)thisStartIndices.x;
        int otherX = (int)otherStartIndices.x;
        for (; x < (int)thisEndIndices.x && otherX < (int)otherEndIndices.x; x++, otherX++)
        {
            int y = (int)thisStartIndices.y;
            int otherY = (int)otherStartIndices.y;
            for (; y < (int)thisEndIndices.y && otherY < (int)otherEndIndices.y; y++, otherY++)
            {
                int z = (int)thisStartIndices.z;
                int otherZ = (int)otherStartIndices.z;
                for (; z < (int)thisEndIndices.z && otherZ < (int)otherEndIndices.z; z++, otherZ++)
                {
                    WFCModule thisModule = GetModule(x, y, z);
                    WFCModule otherModule = other.GetModule(otherX, otherY, otherZ);
                    
                    if (!thisModule && !otherModule) continue;
                    if (thisModule && !otherModule) return false;
                    if (!thisModule && otherModule) return false;
                    if (thisModule.id != otherModule.id)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
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
