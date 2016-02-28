﻿using UnityEngine;
using System.Collections;

public class MapGenerate : MonoBehaviour {

	public float tilesize = 0.64f;

	private int row = 15;
	private int col = 20;
	private int[,] block = new int[15, 20];
	private int blocknum = 0;

	public void GenerateMap(int seed) {
		// Set seed
		Random.seed = seed;
			
		// Initialize the block array
		for (int i = 0; i < row; i++)
			for (int j = 0; j < col; j++)
				block [i, j] = 0;

		// Get the position of the tiles
		int r, c, l;
		r = Random.Range (0, 2);

		while (true) {
			c = Random.Range (0, 8);
			while (true) {
				l = Random.Range (2, 8);
				if (c + l >= col - 1)
					l = col - 1 - c;
				for (int i = 0; i < l; i++)
					block [r, c + i] = 1;
				c += l;
				blocknum += l;
				c += Random.Range (4, 9);
				if (c >= col - 1)
					break;
			}

			r += Random.Range (3, 5);
			if (r >= row - 1)
				break;
		}

		// Set the portals (portal = 6)
		int p1 = Random.Range (1, blocknum/2);
		int p2 = Random.Range (blocknum / 2 + 1, blocknum);
		GameObject PortalTile1 = (GameObject)Instantiate (Resources.Load ("PortalTile"));
		GameObject PortalTile2 = (GameObject)Instantiate (Resources.Load ("PortalTile"));

		int cnt = 0;
		for (int i = 0; i < row; i++) {
			for (int j = 0; j < col; j++) {
				if (block [i, j] == 1) {
					cnt ++;
					if (cnt == p1) {
						block [i, j] = 6;
						SetPosition (PortalTile1, i, j);
						//blocknum--;
					}
					if (cnt == p2) {
						block [i, j] = 6;
						SetPosition (PortalTile2, i, j);
						//blocknum--;
					}
				}
			}
		}
		PortalTile1.GetComponent<PortalTile>().getPosition (PortalTile2);
		PortalTile2.GetComponent<PortalTile>().getPosition (PortalTile1);

		// Set the other kind of tiles
		int type;
		for (int i = 0; i < row; i++)
			for (int j = 0; j < col; j++) {
				if (block [i, j] == 1) {
					type = Random.Range (0, 14);
					switch (type) {
					case 2: // StoneTile
						GameObject stonetile = (GameObject)Instantiate (Resources.Load ("StoneTile"));
						SetPosition (stonetile, i, j);
						break;
					case 3: // IceTile
						GameObject icetile = (GameObject)Instantiate (Resources.Load ("SpringTile"));
						SetPosition (icetile, i, j);
						break;
					case 4: // SpringTile
						GameObject springtile = (GameObject)Instantiate (Resources.Load ("StuckTile"));
						SetPosition (springtile, i, j);
						break;
					case 5: // TrapTile
						GameObject traptile = (GameObject)Instantiate (Resources.Load ("TrapTile"));
						SetPosition (traptile, i, j);
						break;
					default: // WoodTile
						GameObject woodtile = (GameObject)Instantiate (Resources.Load ("WoodTile"));
						SetPosition (woodtile, i, j);
						break;
					}
				}
			}
	}

	// Set the position for the gameobject
	private void SetPosition (GameObject Tile, int i, int j) {
		float x, y;

		if (col % 2 == 1)
			x = (j - (col - 1) / 2) * tilesize;
		else
			x = (j - col / 2) * tilesize;

		if (row % 2 == 1)
			y = (i - (row - 1) / 2) * tilesize;
		else
			y = (i - row / 2) * tilesize;

		Tile.transform.position = new Vector2 (x, y);
	}
}
