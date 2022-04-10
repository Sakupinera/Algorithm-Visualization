using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public enum Status { Free, Busy };

public class AStarTilemap : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;       //  瓦片地图
    [SerializeField] Text promptText;         //  提示文字
    [SerializeField] PromptPanel promptPanel;

    public PromptPanel PromptPanel
    {
        get { return promptPanel; }
        set { promptPanel = value; }
    }
    public Tile baseTile;         //  使用的最基本的Tile，这里是白色块，然后根据数据设置不同颜色生成不同Tile
    [SerializeField] List<Color> colors;    //  所使用到的全体颜色的集合

    Tile[] arrTiles;                        //  生成的Tile数组

    Status currentStatus;                   //  用于保存当前状态
    float printInterval;                       //  用于存储演示时的打印速度

    int[,] map;                             //  记载0，1地图信息，0表示空地，1表示墙

    A_Star a_Star;

    MyPoint begPos;                         //  起点坐标
    MyPoint endPos;                         //  终点坐标

    List<MyPoint> path;                     //  用于存储算法的路径
    bool isFinded = false;                  //  用于表示是否找到路径
    bool isPrinted = false;                 //  用于表示该路径是否被打印

    List<List<MyPoint>> walkedPath;         //  用于存储走过的路

    bool inited = false;

    // Start is called before the first frame update
    public void Init()
    {
        tilemap = GetComponent<Tilemap>();
        InitArrTiles();

        StartCoroutine(InitTiles());
        printInterval = 0.1f;

        promptText = PromptPanel.GetPromptText();
        Debug.Log(promptText);
        promptPanel.HideMe();

        currentStatus = Status.Free;
        a_Star = new A_Star();

        inited = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && inited == true)
        {
            SelectTile();
        }
    }

    //  改变演示速度
    public void ChangePrintSpeed(float value)
    {
        printInterval = 1 / value;
    }

    /// <summary>
    /// 用于可视化演示时设置显示瓦片
    /// </summary>
    private void SelectTile()
    {
        if (currentStatus == Status.Busy)
            return;

        Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int x = vector.x >= 0 ? (int)vector.x : (int)vector.x - 1;
        int y = vector.y >= 0 ? (int)vector.y : (int)vector.y - 1;
        Vector3Int vector3Int = new Vector3Int(x, y, 0);
        if (tilemap.HasTile(vector3Int))
        {
            print($"({vector3Int.x}, {vector3Int.y}, {vector3Int.z})");
            if (map[vector3Int.y + 5, vector3Int.x + 7] == 0)
            {
                tilemap.SetTile(vector3Int, arrTiles[2]);
                SetPoint(new MyPoint(vector3Int.y + 5, vector3Int.x + 7));
            }
            else
            {
                StartCoroutine(HintTextPrint("不能将墙壁设置为寻路点！"));
            }
        }
    }

    /// <summary>
    /// 用于设置起点和终点
    /// </summary>
    /// <param name="point"></param>
    void SetPoint(MyPoint point)
    {
        if (begPos == null)
        {
            begPos = point;
        }
        else
        {
            if (endPos == null)
            {
                endPos = point;
                currentStatus = Status.Busy;
            }
        }
    }

    /// <summary>
    /// A*算法可视化
    /// </summary>
    public void A_Star_Visualization()
    {
        if (currentStatus == Status.Busy)
        {
            if (isPrinted == false)
            {
                isPrinted = true;
                walkedPath = a_Star.GetWalkedPath(map, new AStarPoint(begPos.Row, begPos.Col), new AStarPoint(endPos.Row, endPos.Col));
                path = a_Star.GetA_StarPath(map, new AStarPoint(begPos.Row, begPos.Col), new AStarPoint(endPos.Row, endPos.Col), out isFinded);
                StartCoroutine(PrintPath(printInterval));
            }
        }
        else
        {
            StartCoroutine(HintTextPrint("请先选择两个寻路点！"));
        }
    }

    /// <summary>
    /// 该函数用于显示提示文字
    /// </summary>
    /// <param name="hintTextStr"></param>
    /// <returns></returns>
    IEnumerator HintTextPrint(string hintTextStr)
    {
        promptPanel.ShowMe();
        promptText.text = hintTextStr;
        yield return new WaitForSeconds(1f);
        promptPanel.HideMe();
        promptText.text = "";
    }

    /// <summary>
    /// 用于打印可视化线路图
    /// </summary>
    /// <param name="printInterval"></param>
    /// <returns></returns>
    IEnumerator PrintPath(float printInterval)
    {
        bool interchange = true;
        foreach (var list in walkedPath)
        {
            if (interchange == true)
            {
                foreach (var point in list)
                {
                    Vector3Int vector3Int = new Vector3Int(point.Col - 7, point.Row - 5, 0);
                    tilemap.SetTile(vector3Int, arrTiles[4]);
                    yield return new WaitForSeconds(printInterval);
                }
                interchange = false;
            }
            else
            {
                foreach (var point in list)
                {
                    Vector3Int vector3Int = new Vector3Int(point.Col - 7, point.Row - 5, 0);
                    tilemap.SetTile(vector3Int, arrTiles[6]);
                    yield return new WaitForSeconds(printInterval);
                }

                foreach (var point in list)
                {
                    Vector3Int vector3Int = new Vector3Int(point.Col - 7, point.Row - 5, 0);

                    tilemap.SetTile(vector3Int, arrTiles[5]);
                    yield return new WaitForSeconds(printInterval);
                }
                interchange = true;
            }
        }

        if (isFinded == false)
        {
            StartCoroutine(HintTextPrint("未找到通路！"));
        }

        foreach (var point in path)
        {
            Vector3Int vector3Int = new Vector3Int(point.Col - 7, point.Row - 5, 0);
            tilemap.SetTile(vector3Int, arrTiles[3]);
            yield return new WaitForSeconds(printInterval);
        }
    }

    /// <summary>
    /// 初始化瓦块数组，数组中存储用于显示的瓦片的信息
    /// </summary>
    void InitArrTiles()
    {
        arrTiles = new Tile[colors.Count];

        for (int i = 0; i < colors.Count; i++)
        {
            arrTiles[i] = ScriptableObject.CreateInstance<Tile>();  //创建Tile，注意，要使用这种方式
            arrTiles[i].sprite = baseTile.sprite;
            arrTiles[i].color = colors[i];
        }
    }

    //  初始化瓦块地图
    IEnumerator InitTiles()
    {
        int levelW = 12;
        int levelH = 10;

        map = new int[10, 12];

        for (int i = -5; i < levelH - 5; i++)
        {   //这里就是设置每个Tile的信息了
            for (int j = -7; j < levelW - 7; j++)
            {
                tilemap.SetTile(new Vector3Int(j, i, 0), arrTiles[UnityEngine.Random.Range(0f, 1f) / 0.7 > 1 ? 0 : 1]);
                Color currentColor = tilemap.GetColor(new Vector3Int(j, i, 0));
                if (currentColor == Color.white)
                {
                    map[i + 5, j + 7] = 0;
                }
                else
                {
                    map[i + 5, j + 7] = 1;
                }
            }
            yield return null;
        }
    }

    //  刷新地图
    public void RefreshMap()
    {
        StopAllCoroutines();

        int levelW = 12;
        int levelH = 10;

        for (int i = -5; i < levelH - 5; i++)
        {   //  重新设置每个Tile的信息
            for (int j = -7; j < levelW - 7; j++)
            {
                tilemap.SetTile(new Vector3Int(j, i, 0), arrTiles[1]);
            }
        }

        promptPanel.HideMe();
        promptText.text = "";
        StartCoroutine(InitTiles());
        walkedPath = null;
        begPos = null;
        endPos = null;
        isPrinted = false;
        currentStatus = Status.Free;
    }
}