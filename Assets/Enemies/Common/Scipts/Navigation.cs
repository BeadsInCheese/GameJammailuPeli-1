using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Navigation : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 Target;
    public Tilemap map;
    public bool moving = false;
    int width;
    int height=0;
    TileBase[] tiles;
    NavNode[,] allNodes;
    public class NavNode 
    {
       public  List<NavNode> children = new List<NavNode>();
        public int x;
        public int y;
       public  bool traversed = false;
        public NavNode(int x, int y) 
        {
            this.x = x;
            this.y = y;
             traversed=false;
        }
    }
    public class BFDNavNode
    {
        public BFDNavNode parent;
        public NavNode current;
        public BFDNavNode(NavNode c) {
            this.current = c;
        }
        public List<NavNode> GetPath(List<NavNode> path) 
        {
            path.Add(this.current);
            if (parent != null)
            {
                return parent.GetPath(path);
            }
            else {
                return path;
            }
        }
    }
    public void ConstructTree(TileBase[] tiles)
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
        foreach (var i in allNodes) 
        {
            if (i != null)
            {
                ConstructTree(i, tiles);
            }
        }
    }

    public void ConstructTree(NavNode root, TileBase[] tiles)
    {

        if (root.x + root.y * width < tiles.Length-1)
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
    public void GetNavArea(BoundsInt bounds) 
    {
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

    public List<NavNode> DFS(NavNode root,NavNode target,List<NavNode>path) 
    {
        if (root==null||root.traversed) 
        {
            return null;
        }
            root.traversed = true;
        
        List<NavNode> localPath = new List<NavNode>(path);
        localPath.Add(root);
        if (root == target) 
        {
            return localPath;
        }
        foreach (var child in root.children) 
        {
            if (child == target) {
                return localPath;
            }
        }
        foreach (var child in root.children) 
        {
            var result=DFS(child,target,localPath);
            if (result !=null) {
                
                return result;
            }
        }

        return null;
        
    
    }
    public Vector2 MapToWorldPos(Vector2 pos) 
    {
        return map.CellToWorld(new Vector3Int((int)pos.x, (int)pos.y, 0))+map.origin;

    }
    public Vector2 WorldToMapPos(Vector2 pos)
    {
        var temp = map.WorldToCell(new Vector3(pos.x, pos.y,0)-map.origin);
        return new Vector2(temp.x,temp.y);

    }
    public List<NavNode> BFS(NavNode root, NavNode target, List<NavNode> path)
    {
        List<NavNode> localPath = new List<NavNode>(path);
        Queue<BFDNavNode> searcharea = new Queue<BFDNavNode>();
        if (root == null) 
        {
            return null;
        }
        BFDNavNode r= new BFDNavNode(root);
        searcharea.Enqueue(r);
        r.current.traversed = true;
        while (searcharea.Count > 0) 
        {
            var v = searcharea.Dequeue();
            if (v.current == target) 
            {

                return v.GetPath(new List<NavNode>());
            }
            foreach (NavNode e in v.current.children)
            {
                if (e != null&&!e.traversed)
                {
                    e.traversed = true;
                    BFDNavNode bfdEdge = new BFDNavNode(e);
                    bfdEdge.parent = v;
                    searcharea.Enqueue(bfdEdge);
                }
            }
        }
        return null;
    
    }
    List<NavNode> p;
    public void Start()
    {
        Retarget(Target);

        Debug.Log("Path");
        foreach (var i in p) {
            Debug.Log(i.x+"   "+i.y);
           
        }
        for (int i = 0; i< p.Count-1; i++)
        {
            Debug.DrawLine(MapToWorldPos(new Vector2(p[i].x, p[i].y)), MapToWorldPos(new Vector2(p[i+1].x, p[i+1].y)));

        }
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
   public Vector2 moveTarget=new Vector2(0,0);
    Rigidbody2D rb;
    public float speed;
    public void Retarget(Vector2 tar) 
    {
        GetNavArea(map.cellBounds);
        ConstructTree(tiles);
        var temp = WorldToMapPos(tar);
        var playerGridPos = WorldToMapPos(transform.position);
        p = BFS(allNodes[(int)Mathf.Round(playerGridPos.x), (int)Mathf.Round(playerGridPos.y)], allNodes[(int)temp.x, (int)temp.y], new List<NavNode>());
    }
    void Update()
    {
        if (moving)
        {
            Retarget(Target);



            if (p != null)
            {
                for (int i = 0; i < p.Count - 1; i++)
                {
                    //Debug.DrawLine(MapToWorldPos(new Vector2(0,0)), MapToWorldPos(new Vector2(30,30)));
                    Debug.DrawLine(MapToWorldPos(new Vector2(p[i].x, p[i].y)) + (Vector2)map.cellSize / 2, MapToWorldPos(new Vector2(p[i + 1].x, p[i + 1].y)) + (Vector2)map.cellSize / 2);


                }
                Vector2 tWorldpos = new Vector2(0, 0);
                if (p.Count > 1)
                {
                    tWorldpos = MapToWorldPos(new Vector2(p[p.Count - 2].x, p[p.Count - 2].y)) + (Vector2)map.cellSize / 2;
                }

                moveTarget = (tWorldpos - (Vector2)transform.position);
                Debug.DrawRay(transform.position, moveTarget.normalized);


            }
            rb.velocity = moveTarget.normalized * speed;
        }
    }
}
