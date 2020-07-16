using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTileComponent : MonoBehaviour
{
    public CubeTile cubeTile { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Creates a single CubeTile out of all the WFCModule children it has
    /// </summary>
    /// <param name="firstTileLocation">The location of the WFCModule at 0,0,0 of the new CubeTile</param>
    /// <param name="tileSize">A single dimension of the intended CubeTile</param>
    /// TODO: Duplication from CubeGrid.ScanInputGrid()
    public CubeTile CreateTileFromChildren(Vector3 firstTileLocation, int tileSize, float distanceBetweenTiles)
    {
        Vector3 scanStartLocation = firstTileLocation + (Vector3.forward * 5f);
        GameObject[][][] scannedModules = new GameObject[tileSize][][];
        for (int x = 0; x < tileSize; x++)
        {
            scannedModules[x] = new GameObject[tileSize][];
            for (int y = 0; y < tileSize; y++)
            {
                scannedModules[x][y] = new GameObject[tileSize];
                for (int z = 0; z < tileSize; z++)
                {
                    Vector3 scanLocation = new Vector3(scanStartLocation.x + (x * distanceBetweenTiles), scanStartLocation.y + (y * distanceBetweenTiles), scanStartLocation.z + (z * distanceBetweenTiles));
                    GameObject module = ScanForModule(scanLocation);
                    scannedModules[x][y][z] = module;
                }
            }
        }

        return new CubeTile(scannedModules, -1, -1);
    }

    // TODO: Duplication from CubeGrid.ScanForModule()
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

    public void SetCubeTile(CubeTile cubeTile)
    {
        this.cubeTile = cubeTile;
    }
}
