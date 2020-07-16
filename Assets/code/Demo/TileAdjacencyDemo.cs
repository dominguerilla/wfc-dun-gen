using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class TileAdjacencyDemo : MonoBehaviour
{
    public float distanceBetweenTiles = 10f;
    public CubeTileComponent tile1;
    public Transform tile1StartLocation;
    public CubeTileComponent tile2;
    public Transform tile2StartLocation;

    public ADJACENT_DIRECTION direction;

    CubeTile cTile1, cTile2;

    // Start is called before the first frame update
    void Start()
    {
        cTile1 = tile1.CreateTileFromChildren(tile1StartLocation.position, 2, distanceBetweenTiles);
        cTile2 = tile2.CreateTileFromChildren(tile2StartLocation.position, 2, distanceBetweenTiles);
    }

    void CheckAdjacency(ADJACENT_DIRECTION direction)
    {
        bool isCompatible = cTile1.isCompatible(cTile2, direction);
        Debug.Log($"Is compatible in {direction}: {isCompatible}");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CheckAdjacency(direction);
        }
    }
}
