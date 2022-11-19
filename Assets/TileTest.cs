using UnityEngine;
using UnityEngine.Tilemaps;


public class TileTest : MonoBehaviour {

    public Tile t1;
    public Tilemap bpTileMap;

    public int seed;


    void Start () {
        Tilemap tilemap = GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        BoundsInt b = new BoundsInt(8,0,0,8,8,0);
        //Vector3Int[] t = new Vector3Int[64];
        TileBase[] room1 = new TileBase[13*13];
        TileBase[] room2 = new TileBase[13*13];
        TileBase[] room3 = new TileBase[13*13];
        TileBase[] room4 = new TileBase[16*22];

        Debug.Log(bounds);
        //TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        int[,] map = Generator.generate(seed);

        for (int i = 0; i < 13*13; i++)
        {
            room1[i] = bpTileMap.GetTile(new Vector3Int(i / 13,i % 13,0));
            room2[i] = bpTileMap.GetTile(new Vector3Int(13 + i / 13,i % 13,0));
            room3[i] = bpTileMap.GetTile(new Vector3Int(26 + i / 13,i % 13,0));
            
            //Debug.Log("i:" + i + " tile: "+room1[i]);
        }

        for (int i = 0; i < 16*22; i++)
        {
            room4[i] = bpTileMap.GetTile(new Vector3Int(39 + i / 16,i % 22,0));
        }


/*         int c = 0;
        for (int x = 8; x < 8+8; x++) {
            for (int y = 25; y < 25+8; y++) {

                t[c] = new Vector3Int(x,y,0);
                c++;
            }
        }

        TileBase[] tile2 = tilemap.GetTilesBlock(b);
        tilemap.SetTiles(t,tile2); */

        for (int x = 0; x < 13*20; x += 13) {
            for (int y = 0; y < 13*20; y += 13) {
               
                //TileBase tile = allTiles[x + y * bounds.size.x];
                //TileBase tile2 = tilemap.GetTile(new Vector3Int(Mathf.Clamp(x,0,7) + 8,Mathf.Clamp(y,0,7),0));
                //TileBase tile = allTiles[x + y * bounds.size.x];
/*                 TileBase tile3 = tilemap.GetTile(new Vector3Int(x,y,0));
                if(tile3 != null)
                {
                   Debug.Log("tile found in: x=" + x + " y=" + y);
                } */

                Vector3Int[] positions = new Vector3Int[13*13];
                Vector3Int[] positions2 = new Vector3Int[16*22];

                for(int i = 0; i<13*13; i++)
                {
                    positions[i] = new Vector3Int(x + Mathf.FloorToInt(i/13),y + i % 13, 0);
                }

                for(int i = 0; i<16*22; i++)
                {
                    positions2[i] = new Vector3Int(x + Mathf.FloorToInt(i/22),y + i % 16, 0);
                }

                
                if(map[x/13,y/13] == 1)
                {
                    bpTileMap.SetTiles(positions,room1);
                }
                else if(map[x/13,y/13] == 2)
                {
                    bpTileMap.SetTiles(positions,room2);
                }
                else if(map[x/13,y/13] == 3)
                {
                    bpTileMap.SetTiles(positions,room3);
                }
                else if(map[x/13,y/13] == 4)
                {
                    bpTileMap.SetTiles(positions,room4);
                }
                

            }
        } 

        for (int i = 0; i < 13*13*20; i++)
        {
            int x = i/(13*20);
            int y = i%13*20;

            if (x == 0 || y == 0 || x == 13*20-1 || y == 13*20-1) 
            {
                TileBase tile1 = bpTileMap.GetTile(new Vector3Int(0,0,0));
                tilemap.SetTile(new Vector3Int(x,y,0),tile1);
            } 
            
        }
    }
}