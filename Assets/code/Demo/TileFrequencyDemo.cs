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
        CubeGrid grid = scanner.ScanAndCreateGrid(new Vector3(gridDimensions, gridDimensions, gridDimensions), distanceBetweenModules);
        Dictionary<CubeTile, int> tileFrequencies = grid.GetTileFrequencies(tileSizeDimensions);
        scanner.SpawnTiles(new List<CubeTile>(tileFrequencies.Keys), tileSpawnLocation.position, distanceBetweenModules);
        //scanner.PrintTileFrequencies(tileFrequencies);
    }

    private void Update()
    {
        GetMouseInput();
    }

    void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                WFCModule module = hit.transform.root.GetComponent<WFCModule>();
                CubeTileComponent tileComponent = hit.transform.root.GetComponent<CubeTileComponent>();
                if (module)
                {
                    Debug.Log("Module ID: " + module.id);
                }

                if (tileComponent)
                {
                    Debug.Log("CubeTile gridHashCode: " + tileComponent.cubeTile.gridHashCode);
                }
                
            }
        }
    }
}
