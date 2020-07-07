# Setting up an input Grid

1. Make sure snapping to 3D grid is on in Unity.
2. Create an empty GameObject with a Grid Scanner component.
3. Create an empty child object for it and set it at the point of the grid considered to be (0,0,0).
4. Set this empty child object as the Input Grid Start Location of the Grid Scanner. It should show up in Scene view as a red wireframe sphere.
5. Start placing tiles. Ensure that the first tile's sphere collider is in the same location as the Input Grid Start Location.
6. As you place more tiles, make sure that the distance between each tile's sphere collider is distanceBetweenModules units away from each other. 
