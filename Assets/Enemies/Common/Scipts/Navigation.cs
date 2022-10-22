using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Navigation : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 Target;
    public Tilemap map;
    int vision = 10;
    int width;
    int height=0;
    TileBase[] tiles;
    NavNode[,] allNodes;
    public class NavNode {
       public  List<NavNode> children = new List<NavNode>();
        public int x;
        public int y;
       public  bool traversed = false;
        public NavNode(int x, int y) {
            this.x = x;
            this.y = y;
             traversed=false;
        }
    }
    public void constructTree(TileBase[] tiles)
    {
        for (int x = 0; x < width-1; x++)
        {
            for (int y = 0; y < height-1; y++)
            {
                TileBase tile = tiles[x + y * width];
                if (tile == null)
                {
                    allNodes[x,y]=new NavNode(x,y);
                }
                else
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }
        foreach (var i in allNodes) {
            if (i != null)
            {
                constructTree(i, tiles);
            }
        }
    }

    public void constructTree(NavNode root, TileBase[] tiles)
    {

        if (root.x + root.y * width < tiles.Length-10)
        {

            if (tiles[root.x + root.y * width] == null && root.children.Count == 0&&root!=null)
            {
                if (root.x < width)
                {

                    root.children.Add(allNodes[root.x + 1, root.y]);
                }
                if (root.x < width && root.y + 1 < tiles.Length / width)
                {
                    root.children.Add(allNodes[root.x + 1, root.y + 1]);
                }
                if (root.y + 1 < tiles.Length)
                {
                    root.children.Add(allNodes[root.x, root.y + 1]);
                }
                if (root.x - 1 > 0)
                {
                    root.children.Add(allNodes[root.x - 1, root.y]);
                }
                if (root.x - 1 > 0 && root.y - 1 > 0)
                {
                    root.children.Add(allNodes[root.x - 1, root.y - 1]);
                }
                if (root.y - 1 > 0)
                {
                    root.children.Add(allNodes[root.x, root.y - 1]);
                }
            }
        }
    }
    public void getNavArea(BoundsInt bounds) {
        tiles=map.GetTilesBlock(bounds);
        width = bounds.size.x;
        height = bounds.size.y;
        allNodes = new NavNode[width,height];
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = tiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    //Debug.Log(tiles.Length);
                }
                else
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }

    }

    public List<NavNode> BFS(NavNode root,NavNode target,List<NavNode>path) {
        if (root==null||root.traversed) {
            return null;
        }
            root.traversed = true;
        
        List<NavNode> localPath = new List<NavNode>(path);
        localPath.Add(root);
        if (root == target) {
            return localPath;
        }
        foreach (var child in root.children) {
            if (child == target) {
                return localPath;
            }
        }
        foreach (var child in root.children) {
            var result=BFS(child,target,localPath);
            if (result !=null) {
                
                return result;
            }
        }

        return null;
        
    
    }
    void Start()
    {
        getNavArea(map.cellBounds);
        constructTree(tiles);
        var p=BFS(allNodes[1,1], allNodes[1, 10],new List<NavNode>());
        Debug.Log("Path");
        foreach (var i in p) {
            Debug.Log(i.x+"   "+i.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
