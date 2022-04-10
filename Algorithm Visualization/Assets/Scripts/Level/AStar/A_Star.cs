using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPoint : MyPoint
{
    private int totalCost;
    private int currentCost;
    private int estimatedCost;

    public static int LCost = 10;
    public static int BCost = 14;

    public int TotalCost
    {
        get { return totalCost; }
        set { totalCost = value; }
    }

    public int CurrentCost
    {
        get { return currentCost; }
        set { currentCost = value; }
    }

    public int EstimatedCost
    {
        get { return estimatedCost; }
        set { estimatedCost = value; }
    }

    public AStarPoint(int row, int col)
    {
        this.Row = row;
        this.Col = col;
        this.totalCost = 0;
        this.currentCost = 0;
        this.estimatedCost = 0;
    }

    public void SetTotalCost()
    {
        totalCost = currentCost + estimatedCost;
    }

    public static int GetEstimatedCost(AStarPoint endPos, AStarPoint pos)
    {
        int x = ((endPos.Col > pos.Col) ? (endPos.Col - pos.Col) : (pos.Col - endPos.Col));
        int y = ((endPos.Row > pos.Row) ? (endPos.Row - pos.Row) : (pos.Row - endPos.Row));

        return (x + y) * LCost;
    }
};

public class A_Star
{
    public class TreeNode
    {
        private AStarPoint pos;
        private List<TreeNode> childs;
        private TreeNode parentNode;

        public AStarPoint Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public List<TreeNode> Childs
        {
            get { return childs; }
            set { childs = value; }
        }

        public TreeNode ParentNode
        {
            get { return parentNode; }
            set { parentNode = value; }
        }

        public TreeNode(AStarPoint point)
        {
            pos = new AStarPoint(point.Row, point.Col);
            childs = new List<TreeNode>();
            parentNode = null;
        }
    }

    public enum Direction
    {
        p_up,
        p_down,
        p_left,
        p_right,
    };

    //  判断是否需要统计
    private bool NeedAdd(AStarPoint pos, int[,] map, bool[,] pathMap)
    {
        if (pos.Row >= map.GetLength(0) || pos.Row < 0 || pos.Col >= map.GetLength(1) || pos.Col < 0)
            return false;
        if (1 == map[pos.Row, pos.Col])
            return false;
        if (true == pathMap[pos.Row, pos.Col])
            return false;
        return true;
    }

    public List<List<MyPoint>> GetWalkedPath(int[,] map, AStarPoint begPos, AStarPoint endPos)
    {
        List<List<MyPoint>> walkedPath = new List<List<MyPoint>>();

        //	2.辅助地图	false 0:没有走过		true 1:走过
        bool[,] pathMap = new bool[map.GetLength(0), map.GetLength(1)];

        //	4.标记起点走过
        pathMap[begPos.Row, begPos.Col] = true;
        //  存储走过的点
        walkedPath.Add(new List<MyPoint>());
        walkedPath[0].Add(begPos);

        //	5.创建一棵树，起点是树的根
        TreeNode pNew = new TreeNode(begPos);

        //	新结点成为树根
        //	pRoot一直指向树的根节点
        TreeNode pRoot = pNew;

        //	6.需要一个动态数组，存储用来比较的节点
        List<TreeNode> buff = new List<TreeNode>();

        //	7.寻路
        TreeNode pCurrent = pRoot;
        TreeNode pChild = null;

        TreeNode it;
        TreeNode itMin;

        while (true)
        {
            walkedPath.Add(new List<MyPoint>());
            //	7.1	找到当前点周围能走的点
            for (int i = 0; i < 4; i++)
            {
                pChild = new TreeNode(pCurrent.Pos);
                Direction dir = (Direction)i;
                switch (dir)
                {
                    case Direction.p_up:
                        pChild.Pos.Row--;
                        pChild.Pos.CurrentCost += AStarPoint.LCost;
                        break;
                    case Direction.p_down:
                        pChild.Pos.Row++;
                        pChild.Pos.CurrentCost += AStarPoint.LCost;
                        break;
                    case Direction.p_left:
                        pChild.Pos.Col--;
                        pChild.Pos.CurrentCost += AStarPoint.LCost;
                        break;
                    case Direction.p_right:
                        pChild.Pos.Col++;
                        pChild.Pos.CurrentCost += AStarPoint.LCost;
                        break;
                    #region
                    //case Direction.p_lup:
                    //    pChild.Pos.Row--;
                    //    pChild.Pos.Col--;
                    //    pChild.Pos.CurrentCost += MyPoint.BCost;
                    //    break;
                    //case Direction.p_ldown:
                    //    pChild.Pos.Row++;
                    //    pChild.Pos.Col--;
                    //    pChild.Pos.CurrentCost += MyPoint.BCost;
                    //    break;
                    //case Direction.p_rup:
                    //    pChild.Pos.Row--;
                    //    pChild.Pos.Col++;
                    //    pChild.Pos.CurrentCost += MyPoint.BCost;
                    //    break;
                    //case Direction.p_rdown:
                    //    pChild.Pos.Row++;
                    //    pChild.Pos.Col++;
                    //    pChild.Pos.CurrentCost += MyPoint.BCost;
                    //    break;
                    #endregion
                    default:
                        break;
                }

                //	7.2	计算代价值
                pChild.Pos.EstimatedCost = AStarPoint.GetEstimatedCost(endPos, pChild.Pos);
                pChild.Pos.SetTotalCost();

                //	7.3	入树，入buff数组，标记走过
                if (NeedAdd(pChild.Pos, map, pathMap))
                {
                    //	入树
                    pCurrent.Childs.Add(pChild);
                    pChild.ParentNode = pCurrent;

                    //	入buff数组
                    buff.Add(pChild);
                    //	标记走过
                    pathMap[pChild.Pos.Row, pChild.Pos.Col] = true;

                    //  存入可选择的点
                    walkedPath[walkedPath.Count - 1].Add(pChild.Pos);
                }
            }

            if (buff.Count == 0)
                break;

            //	7.4	从buff数组中找出totalCost值最小的那个
            itMin = buff[0];
            for (int i = 0; i < buff.Count; i++)
            {
                it = buff[i];
                itMin = (itMin.Pos.TotalCost > it.Pos.TotalCost) ? it : itMin;
            }

            //	7.5	删掉，变化成当前的点
            walkedPath.Add(new List<MyPoint>());
            pCurrent = itMin;
            walkedPath[walkedPath.Count - 1].Add(pCurrent.Pos);
            buff.Remove(itMin);

            //	7.6	判断是否寻路结束
            if (pCurrent.Pos.Row == endPos.Row && pCurrent.Pos.Col == endPos.Col)
            {
                break;
            }
        }

        return walkedPath;
    }

    public List<MyPoint> GetA_StarPath(int[,] map, AStarPoint begPos, AStarPoint endPos, out bool isFinded)
    {
        List<MyPoint> a_StarPath = new List<MyPoint>();

        //	2.辅助地图	false 0:没有走过		true 1:走过
        bool[,] pathMap = new bool[map.GetLength(0), map.GetLength(1)];

        //	4.标记起点走过
        pathMap[begPos.Row, begPos.Col] = true;

        //	5.创建一棵树，起点是树的根
        TreeNode pNew = new TreeNode(begPos);

        //	新结点成为树根
        //	pRoot一直指向树的根节点
        TreeNode pRoot = pNew;

        //	6.需要一个动态数组，存储用来比较的节点
        List<TreeNode> buff = new List<TreeNode>();

        //	7.寻路
        TreeNode pCurrent = pRoot;
        TreeNode pChild = null;

        TreeNode it;
        TreeNode itMin;

        isFinded = false;

        while (true)
        {
            //	7.1	找到当前点周围能走的点
            for (int i = 0; i < 4; i++)
            {
                pChild = new TreeNode(pCurrent.Pos);
                Direction dir = (Direction)i;
                switch (dir)
                {
                    case Direction.p_up:
                        pChild.Pos.Row--;
                        pChild.Pos.CurrentCost += AStarPoint.LCost;
                        break;
                    case Direction.p_down:
                        pChild.Pos.Row++;
                        pChild.Pos.CurrentCost += AStarPoint.LCost;
                        break;
                    case Direction.p_left:
                        pChild.Pos.Col--;
                        pChild.Pos.CurrentCost += AStarPoint.LCost;
                        break;
                    case Direction.p_right:
                        pChild.Pos.Col++;
                        pChild.Pos.CurrentCost += AStarPoint.LCost;
                        break;
                    #region
                    //case Direction.p_lup:
                    //    pChild.Pos.Row--;
                    //    pChild.Pos.Col--;
                    //    pChild.Pos.CurrentCost += MyPoint.BCost;
                    //    break;
                    //case Direction.p_ldown:
                    //    pChild.Pos.Row++;
                    //    pChild.Pos.Col--;
                    //    pChild.Pos.CurrentCost += MyPoint.BCost;
                    //    break;
                    //case Direction.p_rup:
                    //    pChild.Pos.Row--;
                    //    pChild.Pos.Col++;
                    //    pChild.Pos.CurrentCost += MyPoint.BCost;
                    //    break;
                    //case Direction.p_rdown:
                    //    pChild.Pos.Row++;
                    //    pChild.Pos.Col++;
                    //    pChild.Pos.CurrentCost += MyPoint.BCost;
                    //    break;
                    #endregion
                    default:
                        break;
                }

                //	7.2	计算代价值
                pChild.Pos.EstimatedCost = AStarPoint.GetEstimatedCost(endPos, pChild.Pos);
                pChild.Pos.SetTotalCost();

                //	7.3	入树，入buff数组，标记走过
                if (NeedAdd(pChild.Pos, map, pathMap))
                {
                    //	入树
                    pCurrent.Childs.Add(pChild);
                    pChild.ParentNode = pCurrent;

                    //	入buff数组
                    buff.Add(pChild);
                    //	标记走过
                    pathMap[pChild.Pos.Row, pChild.Pos.Col] = true;
                }
            }

            if (buff.Count == 0)
                break;

            //	7.4	从buff数组中找出totalCost值最小的那个
            itMin = buff[0];
            for (int i = 0; i < buff.Count; i++)
            {
                it = buff[i];
                itMin = (itMin.Pos.TotalCost > it.Pos.TotalCost) ? it : itMin;
            }

            //	7.5	删掉，变化成当前的点
            pCurrent = itMin;
            buff.Remove(itMin);

            //	7.6	判断是否寻路结束
            if (pCurrent.Pos.Row == endPos.Row && pCurrent.Pos.Col == endPos.Col)
            {
                isFinded = true;
                break;
            }
        }

        a_StarPath.Clear();

        if (isFinded)
        {
            while (pCurrent != null)
            {
                a_StarPath.Add(pCurrent.Pos);
                pCurrent = pCurrent.ParentNode;
            }
        }

        return a_StarPath;
    }
}
