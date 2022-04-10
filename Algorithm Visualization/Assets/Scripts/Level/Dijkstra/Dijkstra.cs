using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DijkstraPoint : MyPoint
{

    public DijkstraPoint(int row, int col)
    {
        this.Row = row;
        this.Col = col;
    }

    public DijkstraPoint(DijkstraPoint dijkstraPoint)
    {
        this.Row = dijkstraPoint.Row;
        this.Col = dijkstraPoint.Col;
    }

}

class Dijkstra
{
    const int MaxValue = 1000000000;
    public enum Direction
    {
        p_up,
        p_down,
        p_left,
        p_right,
    };

    public int PointToIndex(DijkstraPoint dijkstraPoint, int[,] map)
    {
        int index = dijkstraPoint.Row * map.GetLength(1) + dijkstraPoint.Col;
        return index;
    }

    public DijkstraPoint IndexToPoint(int index, int[,] map)
    {
        int row = index / map.GetLength(1);
        int col = index % map.GetLength(1);
        return new DijkstraPoint(row, col);
    }

    public bool DetectOutOfRange(int index, int[,] map)
    {
        if (index < 0 || index >= map.Length)
            return true;
        else
            return false;
    }

    //  初始化邻接矩阵
    public void InitWeightsArray(int[,] weights, int[,] map)
    {
        int mapCol = map.GetLength(1);

        for (int i = 0; i < weights.GetLength(0); i++)
        {
            for (int j = 0; j < weights.GetLength(1); j++)
            {
                if (i == j)
                    weights[i, j] = 0;
                else
                    weights[i, j] = MaxValue;
            }
        }

        for (int i = 0; i < weights.GetLength(0); i++)
        {
            for (int x = 0; x < 4; x++)
            {
                int aroundIndex = i;
                Direction dir = (Direction)x;
                switch (dir)
                {
                    case Direction.p_up:
                        aroundIndex -= mapCol;
                        if (!DetectOutOfRange(aroundIndex, map))
                        {
                            DijkstraPoint point = new DijkstraPoint(IndexToPoint(aroundIndex, map));
                            if (map[point.Row, point.Col] != 1)
                            {
                                weights[i, aroundIndex] = 1;
                            }
                        }
                        break;
                    case Direction.p_down:
                        aroundIndex += mapCol;
                        if (!DetectOutOfRange(aroundIndex, map))
                        {
                            DijkstraPoint point = new DijkstraPoint(IndexToPoint(aroundIndex, map));
                            if (map[point.Row, point.Col] != 1)
                            {
                                weights[i, aroundIndex] = 1;
                            }
                        }
                        break;
                    case Direction.p_left:
                        if (!(i % mapCol == 0))
                        {
                            aroundIndex -= 1;
                            if (!DetectOutOfRange(aroundIndex, map))
                            {
                                DijkstraPoint point = new DijkstraPoint(IndexToPoint(aroundIndex, map));
                                if (map[point.Row, point.Col] != 1)
                                {
                                    weights[i, aroundIndex] = 1;
                                }
                            }
                        }
                        break;
                    case Direction.p_right:
                        if (!(i % mapCol == mapCol - 1))
                        {
                            aroundIndex += 1;
                            if (!DetectOutOfRange(aroundIndex, map))
                            {
                                DijkstraPoint point = new DijkstraPoint(IndexToPoint(aroundIndex, map));
                                if (map[point.Row, point.Col] != 1)
                                {
                                    weights[i, aroundIndex] = 1;
                                }
                            }
                        }
                        break;
                }
            }
        }
    }

    //  查找可以加入到最优路径中的顶点
    public int FindMinV(int[] book, int[] dist)
    {
        int minV = -1;
        int minDist = int.MaxValue;
        for (int i = 0; i < book.Length; i++)
        {
            if (book[i] == 0 && dist[i] < minDist)
            {
                minV = i;
                minDist = dist[i];
            }
        }
        return minV;
    }

    public List<List<MyPoint>> GetWalkedPath(int[,] map, DijkstraPoint begPos, DijkstraPoint endPos)
    {
        //  用于记载走过的路
        List<List<MyPoint>> walkedPath = new List<List<MyPoint>>();

        int vertexNum = map.Length;

        //  采用邻接矩阵的形式存储
        int[,] weights = new int[vertexNum, vertexNum];
        InitWeightsArray(weights, map);

        //  记录起点到终点的最短路径长度
        int[] dist = new int[vertexNum];
        for (int i = 0; i < dist.Length; i++)
            dist[i] = -1;

        //  标记某点是否在最优路径集合中
        int[] book = new int[vertexNum];

        //  用来记载前一个节点的坐标
        DijkstraPoint[] pointPath = new DijkstraPoint[vertexNum];

        int begIndex = PointToIndex(begPos, map);

        //  初始化地点到终点的最短路径长度
        for (int i = 0; i < dist.Length; i++)
            dist[i] = MaxValue;

        dist[begIndex] = 0;

        while (true)
        {
            int v = FindMinV(book, dist);

            if (v == -1)
                break;
            //  将该节点加入最优路径集合
            book[v] = 1;

            //  * 添加走过的点
            walkedPath.Add(new List<MyPoint>());
            walkedPath[walkedPath.Count - 1].Add(IndexToPoint(v, map));

            DijkstraPoint point = new DijkstraPoint(IndexToPoint(v, map));

            if (point.Row == endPos.Row && point.Col == endPos.Col)
                break;

            //  * 新添加的列表用于显示被更新标记的节点
            walkedPath.Add(new List<MyPoint>());

            //  新结点加入集合会影响其它不在集合里dist的最小值
            for (int i = 0; i < book.Length; i++)
            {
                if (book[i] == 0)
                {
                    if (dist[v] + weights[v, i] < dist[i])
                    {
                        dist[i] = dist[v] + weights[v, i];

                        //  * 添加被更新标记的节点到列表中
                        walkedPath[walkedPath.Count - 1].Add(IndexToPoint(i, map));
                        pointPath[i] = new DijkstraPoint(IndexToPoint(v, map));
                    }
                }
            }
        }

        return walkedPath;
    }

    public List<MyPoint> GetDijkstraPath(int[,] map, DijkstraPoint begPos, DijkstraPoint endPos, out bool isFinded)
    {
        List<MyPoint> dijkstraPath = new List<MyPoint>();
        isFinded = false;

        int vertexNum = map.Length;

        //  采用邻接矩阵的形式存储
        int[,] weights = new int[vertexNum, vertexNum];
        InitWeightsArray(weights, map);

        //  记录起点到终点的最短路径长度
        int[] dist = new int[vertexNum];
        for (int i = 0; i < dist.Length; i++)
            dist[i] = -1;

        //  标记某点是否在最优路径集合中
        int[] book = new int[vertexNum];

        //  用来记载前一个节点的坐标
        DijkstraPoint[] pointPath = new DijkstraPoint[vertexNum];

        int begIndex = PointToIndex(begPos, map);

        //  初始化地点到终点的最短路径长度
        for (int i = 0; i < dist.Length; i++)
            dist[i] = MaxValue;

        dist[begIndex] = 0;

        while (true)
        {
            int v = FindMinV(book, dist);

            if (v == -1)
                break;
            //  将该节点加入最优路径集合
            book[v] = 1;

            DijkstraPoint point = new DijkstraPoint(IndexToPoint(v, map));

            if (point.Row == endPos.Row && point.Col == endPos.Col)
                break;

            //  新结点加入集合会影响其它不在集合里dist的最小值
            for (int i = 0; i < book.Length; i++)
            {
                if (book[i] == 0)
                {
                    if (dist[v] + weights[v, i] < dist[i])
                    {
                        dist[i] = dist[v] + weights[v, i];
                        pointPath[i] = new DijkstraPoint(IndexToPoint(v, map));
                    }
                }
            }
        }

        DijkstraPoint currentPos = endPos;

        while (currentPos != null)
        {
            dijkstraPath.Add(currentPos);

            if (currentPos.Row == begPos.Row && currentPos.Col == begPos.Col)
            {
                isFinded = true;
                break;
            }

            currentPos = pointPath[PointToIndex(currentPos, map)];
        }

        return dijkstraPath;

    }
}

