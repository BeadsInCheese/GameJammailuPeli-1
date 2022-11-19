using System;
using UnityEngine;

public class Generator : MonoBehaviour
{
    const int SINE_PARAMETER = 345941;
    static int seed = 89348;

    static int areaBound = 20;
    static int squareSize = 13;
    static int[,] map = new int[areaBound,areaBound];

    static int maxTotalSq = 330;
    static int maxExplorableSq = 250;
    static int minPaths = 3;
    static int maxPaths = 8;
    static int minFixedSq = 5;
    static int maxFixedSq = 10;

    static int startEdgeOffset = 2;
    static int minManhattanDistanceToExit = 700;

    static int[,] fixedPoints;
    static int noFixedPoints;
    static int noPaths;

    static float noiseParameter = 0.2f;


    public static int[,] generate(int s)
    {
        seed = s;

        int startX = g(seed,areaBound,startEdgeOffset,startEdgeOffset);
        int startY = g(seed,areaBound,startEdgeOffset,startEdgeOffset);
        int endX = g(seed,areaBound,startEdgeOffset,startEdgeOffset);
        int endY = g(seed,areaBound,startEdgeOffset,startEdgeOffset);

        Debug.Log("start: "+startX+", "+startY+" end: "+endX+", "+endY);



        noFixedPoints = 5;//g(seed,5,0,minFixedSq);
        fixedPoints = new int[noFixedPoints,2];

        fixedPoints[0,0] = g(seed,10,0,2);
        fixedPoints[0,1] = g(seed,10,0,2);

        for (int i = 1; i < noFixedPoints; i++)
        {
            fixedPoints[i,0] = fixedPoints[0,0] + g(seed,5,0,2);
            fixedPoints[i,1] = fixedPoints[0,1] + g(seed,5,0,2);
            Debug.Log("fixed point: "+fixedPoints[i,0]+" "+fixedPoints[i,1]);
        }

        noPaths = 1;//g(seed,5,0,10);

        Debug.Log("rolled "+noFixedPoints+" fixed points, and "+noPaths+" paths");

        

        for (int i = 0; i < noPaths; i++)
        {
            //ITERATION 1
            int r1 = g(seed,areaBound,startEdgeOffset,startEdgeOffset);
            int r2 = g(seed,areaBound,startEdgeOffset,startEdgeOffset);
            int r3 = g(seed,areaBound,startEdgeOffset,startEdgeOffset);
            int r4 = g(seed,areaBound,startEdgeOffset,startEdgeOffset);


            int fixpoint1 = 0;//g(seed,noFixedPoints,0,0);
            int fixpoint2 = 1;//g(seed,noFixedPoints,0,0);

            //connect (r1,r2) to (r3,r4) through some fixed points
            connect(r1,r2,r3,r4,fixpoint1,fixpoint2);

            //ITERATION 2
            connect(r1,r2,fixedPoints[0,0],fixedPoints[0,1],2,3);
            connect(fixedPoints[0,0],fixedPoints[0,1],fixedPoints[1,0],fixedPoints[1,1],1,2);
            connect(fixedPoints[1,0],fixedPoints[1,1],r3,r4,0,4);

            //ITERATION 3...



            if(i == noPaths - 1)
            {
                int randomFixedPointX = g(seed,noFixedPoints,0,0);
                int randomFixedPointY = g(seed,noFixedPoints,0,0);
                //connect start and end
                connect(startX,startY,endX,endY,randomFixedPointX,randomFixedPointY);
            }
        }

        
        map[startX,startY] = 2;
        map[endX,endY] = 3;

        return map;
    }

    static void connect(int sX, int sY, int tX, int tY, int fixpoint1, int fixpoint2)
    {

        //Debug.Log("start of path: "+sX+","+sY+", end of path: "+tX+","+tY);

        //from (sX,sY) to (fixedPoints[0,0],sY)    
        for (int j = sX; j != fixedPoints[fixpoint1,0]; j += (int)Mathf.Sign(fixedPoints[fixpoint1,0] - sX))
        {
            map[j,sY] = 1;
        }
        //from (fixedPoints[0,0],sY) to (fixedPoints[0,0],fixedPoint[0,1])
        for (int j = sY; j != fixedPoints[fixpoint1,1]; j += (int)Mathf.Sign(fixedPoints[fixpoint1,1] - sY))
        {

            map[fixedPoints[fixpoint1,0],j] = 1;    
            
            
        }
        //from (fp[0,0],fp[0,1]) to (fp[1,0],fp[0,1])
        for (int j = fixedPoints[fixpoint1,0]; j != fixedPoints[fixpoint2,0]; j += (int)Mathf.Sign(fixedPoints[fixpoint2,0] - fixedPoints[fixpoint1,0]))
        {
            map[j,fixedPoints[fixpoint1,1]] = 1;
        }
        //from (fp[1,0],fp[0,1]) to (fp[1,0],fp[1,1])
        for (int j = fixedPoints[fixpoint1,1]; j != fixedPoints[fixpoint2,1]; j += (int)Mathf.Sign(fixedPoints[fixpoint2,1] - fixedPoints[fixpoint1,1]))
        {
            map[fixedPoints[fixpoint2,0],j] = 1;
        }
        //from (fp[1,0],fp[1,1]) to (tX,fp[1,1])
        for (int j = fixedPoints[fixpoint2,0]; j != tX; j += (int)Mathf.Sign(tX - fixedPoints[fixpoint2,0]))
        {
            map[j,fixedPoints[fixpoint2,1]] = 1;
        }
        //from (tX,fp[1,1]) to (tX,tY)
        for (int j = fixedPoints[fixpoint2,1]; j != tY; j += (int)Mathf.Sign(tY - fixedPoints[fixpoint2,1]))
        {
            map[tX,j] = 1;
        }


        
    }

    static void InterpolatedConnect()
    {

    }

    static int g(int x, int t, int a, int b)
    {
        float temp = (float)(100000*Math.Sin(x) % 1);
        //float temp = 100000*Mathf.Sin(x) % 1;
        //Debug.Log(temp);
        int f = Mathf.FloorToInt(10000*temp);
        //Debug.Log(f);
        int value = (SINE_PARAMETER - f) % (t - 2*a) + b;
        seed++;
        return value;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
