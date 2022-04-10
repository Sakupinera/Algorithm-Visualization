using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPoint
{
    int row;
    int col;

    public int Row
    {
        get { return row; }
        set { row = value; }
    }

    public int Col
    {
        get { return col; }
        set { col = value; }
    }

    public MyPoint() { }

    public MyPoint(int row,int col)
    {
        this.row = row;
        this.col = col;
    }
}
