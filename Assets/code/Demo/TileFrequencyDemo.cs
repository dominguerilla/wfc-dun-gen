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

    CubeGrid grid;
    Dictionary<CubeTile, int> tileFrequencies;

    // Start is called before the first frame update
    void Start()
    {
        this.grid = scanner.ScanAndCreateGrid(new Vector3(gridDimensions, gridDimensions, gridDimensions), distanceBetweenModules);
        tileFrequencies = grid.GetTileFrequencies(tileSizeDimensions);
        scanner.SpawnTiles(new List<CubeTile>(tileFrequencies.Keys), tileSpawnLocation.position, distanceBetweenModules);
        //scanner.PrintTileFrequencies(tileFrequencies);
    }

    private void Update()
    {
        GetMouseInput();
        RotateGrid();
    }

    void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                WFCModule module = hit.transform.root.GetComponent<WFCModule>();
                CubeTileComponent tileComponent = hit.transform.root.GetComponent<CubeTileComponent>();
                if (module)
                {
                    Debug.Log("Module ID: " + module.id);
                }

                if (tileComponent)
                {
                    Debug.Log($"CubeTile {tileComponent.cubeTile.tileIndex} sampled from {tileComponent.cubeTile.sampleCoordinates}, appears {tileFrequencies[tileComponent.cubeTile]} times");
                }
                
            }
        }
    }

    void RotateGrid()
    {

    }
}
