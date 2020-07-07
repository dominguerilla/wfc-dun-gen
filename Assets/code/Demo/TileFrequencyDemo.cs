using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFrequencyDemo : MonoBehaviour
{
    [SerializeField] GridScanner scanner;
    [SerializeField] Transform tileSpawnLocation;
    [SerializeField] int gridDimensions = 6;
    [SerializeField] int tileSizeDimensions = 2;
    [SerializeField] int distanceBetweenModules = 10;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[][][] grid = scanner.ScanAndCreateGrid(new Vector3(gridDimensions, gridDimensions, gridDimensions), distanceBetweenModules);
        Dictionary<CubeTile, int> tileFrequencies = scanner.GetTileFrequencies(grid, tileSizeDimensions);
        scanner.SpawnTiles(tileFrequencies, tileSpawnLocation.position, distanceBetweenModules);
        scanner.PrintTileFrequencies(tileFrequencies);
    }
}
