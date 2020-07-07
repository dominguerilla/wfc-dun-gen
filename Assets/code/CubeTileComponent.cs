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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCubeTile(CubeTile cubeTile)
    {
        this.cubeTile = cubeTile;
    }
}
