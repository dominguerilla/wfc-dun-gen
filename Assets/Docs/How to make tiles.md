# How to make tiles

1. Each tile prefab should consist of at least 3 nested game objects:
	- an outer empty object that serves as the prefab
	- a second empty object (and the only child of the outer object) called the Pivot Object
	- whatever physical meshes/gameobjects to visually show the tile nested within the Pivot Object
	- so, for example, a Floor tile prefab might look like this in the hierarchy:
		-> Floor
			-> Pivot Object
				-> Cube
	- the whole tile prefab should be 10x10x10 units in size
2. The outer gameobject should be tagged 'Tile' and have its Unity pivot (not to be confused with the Pivot Object!) be at the back-bottom-left corner if facing the tile prefab from the front
3. The Pivot should be at the local position of (5,5,5), have the local rotation of (0,0,0), and at (1,1,1) scale
	- the idea being that we can rotate the physical representation of the tile by rotating its Pivot gameobject, NOT the outer gameobject itself
