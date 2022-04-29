using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sort : MonoBehaviour
{
    //元素个数
    public int _len;

    public float speed = 0.5f;

    //预制体
    [SerializeField]
    private GameObject sortcubePrefabs;
    [SerializeField]
    private GameObject valuePoint;

    public Text NumText;

    //是否正在排序
    bool isSorting = false;

    private int[] arr;
    [SerializeField]
    private List<GameObject> cubeList = new List<GameObject>();

    [SerializeField]
    private Material[] cubeColors;
    private GameObject[] cube;

    //枚举排序类型
    public enum SortName { a, b, c}

    //用来存放所有cube
    public Transform cubes;

    //初始化
    public void Initialized(int n)
    {
        arr = new int[n];
        System.Random rd = new System.Random();
        for (int i = 0; i < n; i++)
        {
            GameObject obj = Instantiate(sortcubePrefabs);
            obj.name = "cube" + i;
            int v = rd.Next(1, 30);
            obj.transform.localScale = new Vector3(1f, v, 1f);
            obj.transform.SetParent(cubes);
            obj.transform.position = new Vector3(0, (v / 2f), -(n / 2) + i * 1.1f);
            obj.GetComponent<MeshRenderer>().material.color = Color.white;

            GameObject vp = Instantiate(valuePoint);
            vp.transform.SetParent(obj.transform);
            vp.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y + (v + 1) * 0.5f, obj.transform.position.z);
            Text text = vp.GetComponentInChildren<Text>();
            text.text = v.ToString();
            cubeList.Add(obj);
            arr[i] = v;
        }
        _len = n;
        NumText.text = "当前排序数字个数：" +_len.ToString();
    }

    //选择排序
    IEnumerator SelectSort()
    {
        int minIndex;
        for(int i = 0; i < _len-1 ; i++)
        {
            ChangeColor(i, Color.blue);
            yield return new WaitForSeconds(speed);

            minIndex = i;
            for(int j = i+1; j < _len ; j++)
            {
                ChangeColor(j, Color.cyan);
                yield return new WaitForSeconds(0.2f * speed);

                if (arr[j] < arr[minIndex])
                {
                    if (minIndex != i)
                        ChangeColor(minIndex, Color.white);

                    minIndex = j;
                    ChangeColor(j, Color.red);
                    continue;
                }
                ChangeColor(j, Color.white);
            }

            if(minIndex != i)
            {
                //交换坐标
                int t = arr[i];
                arr[i] = arr[minIndex];
                arr[minIndex] = t;

                ChangeListAndPosition(i, minIndex);

            }
            ChangeColor(i, Color.green);
            yield return new WaitForSeconds(speed);
        }
    }

    //冒泡排序
    IEnumerator BubbleSort()
    {
        for(int i = 0; i < _len-1; i++)
        {

            yield return new WaitForSeconds(speed);

            for(int j = 0; j < _len - i - 1; j++)
            {
                bool last = j == _len - i - 2;

                ChangeColor(j, Color.blue);
                ChangeColor(j + 1, Color.blue);
                yield return new WaitForSeconds(speed);

                if(arr[j] > arr[j + 1])
                {
                    //交换坐标
                    int t = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = t;

                    //交换位置
                    ChangeListAndPosition(j, j + 1);
                    
                    if (last)
                    {
                        ChangeColor(j + 1, Color.green);
                    }
                    else
                    {
                        ChangeColor(j + 1, Color.white);
                    }
                    ChangeColor(j, Color.white);
                    yield return new WaitForSeconds(speed);
                    continue;
                }
                ChangeColor(j, Color.white);
            
                if(last && i == _len - 2)
                {
                    ChangeColor(j, Color.green);
                    ChangeColor(j + 1, Color.green);
                }
                else if (last)
                {
                    ChangeColor(j + 1, Color.green);
                }
                else
                {
                    ChangeColor(j + 1, Color.white);
                }
            }
        }
    }

    //插入排序
    IEnumerator InsertSort()
    {
        for(int i = 0; i < _len; i++)
        {
            yield return new WaitForSeconds(speed);
            ChangeColor(i, Color.blue);
            for(int j = i; j > 0; j--)
            {
                ChangeColor(j, Color.blue);
                ChangeColor(j - 1, Color.blue);

                if(arr[j] > arr[j - 1])
                {
                    break;
                }

                ChangeColor(j, Color.red);
                ChangeColor(j - 1, Color.green);

                //交换下标
                int t = arr[j];
                arr[j] = arr[j - 1];
                arr[j - 1] = t;

                ChangeListAndPosition(j, j - 1);

                yield return new WaitForSeconds(speed);

                ChangeColor(j, Color.blue);
                ChangeColor(j-1, Color.blue);
            }
        }
        for(int i = 0; i < _len; i++)
        {
            ChangeColor(i, Color.green);
        }
    }

    //交换List和位置
    void ChangeListAndPosition(int i, int j)
    {
        GameObject obj = cubeList[i];
        cubeList[i] = cubeList[j];
        cubeList[j] = obj;
        //cubeList[i].GetComponent<MeshRenderer>().material.color = Color.white;
        ChangeColor(i, Color.white);
        //cubeList[minIndex].GetComponent<MeshRenderer>().material.color = Color.white;
        ChangeColor(j, Color.white);

        //交换位置
        Vector3 ti = new Vector3(cubeList[i].transform.position.x, cubeList[j].transform.position.y, cubeList[i].transform.position.z);
        Vector3 tj = new Vector3(cubeList[j].transform.position.x, cubeList[i].transform.position.y, cubeList[j].transform.position.z);
        StartCoroutine(SwapPosition(i, tj));
        StartCoroutine(SwapPosition(j, ti));
    }

    //移动位置
    IEnumerator SwapPosition(int i, Vector3 j)
    {
        StopCoroutine(SelectSort());
        //交换位置
        while (cubeList[i].transform.localPosition != j)
        {
            cubeList[i].transform.localPosition = Vector3.MoveTowards(cubeList[i].transform.localPosition, j, 30 * Time.deltaTime);
            yield return 0;
        }
    }

    //修改颜色
    void ChangeColor(int i, Color c)
    {
        cubeList[i].GetComponent<MeshRenderer>().material.color = c;
    }

    //清除
    void clear()
    {
        foreach(var t in cubeList)
        {
            Destroy(t);
        }
        cubeList.Clear();
        StopAllCoroutines();
    }
    
    //打印数组
    void print()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach(var t in arr)
        {
            sb.Append(" " + t.ToString());
        }
    }

    //重置
    public void restart()
    {
        clear();
        isSorting = false;
        Initialized(_len);
    }

    public void Speed_Changed(float value)
    {
        Time.timeScale = value;
    }

    /// <summary>
    /// 数字个数滑动条发生变化
    /// </summary>
    public void Num_Changed(int value)
    {
        clear();
        isSorting = false;
        _len = value;
        Initialized(_len);
    }

    //开始排序
    public void StartSorting(string sortName)
    {
        if (!isSorting)
        {
            isSorting = true;
            StartCoroutine(sortName);
        }
    }
}
