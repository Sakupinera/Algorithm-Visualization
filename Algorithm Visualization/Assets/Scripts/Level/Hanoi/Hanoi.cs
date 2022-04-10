using System.Collections;
using System;
using System.Timers;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
public class Hanoi : MonoBehaviour
{
    public GameObject model;//用于组成汉诺塔的基本方块
    public int n=5;//汉诺塔的原始方块数量
    private Vector3 pos_a;//a柱子的位置
    private Vector3 pos_b;//b柱子的位置
    private Vector3 pos_c;//c柱子的位置
    [SerializeField]
    private Transform pos_A,pos_B,pos_C;
    public bool is_auto_run = false;//是否自动执行代码
    public float time=1;//移动物体耗费的时间
    public float space=1;//上下间隔的距离
    public Text show_num;
    public Text show_speed;
    Stack st_a = new Stack();//利用栈储存物体对象，便于进行移动操作
    Stack st_b = new Stack();
    Stack st_c = new Stack();
    double size;
    float timer;
    GameObject[] md = new GameObject[100];//最多100个，再多也跑不完了
    GameObject tmp;
    string[] order_of_hnt = new String[2];//保存执行顺序的数组，先计算完结果再按顺序移动方块
    int current_index=0;

    void Awake(){
        pos_a = pos_A.transform.position;
        pos_b = pos_B.transform.position;
        pos_c = pos_C.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (n > 100)
        {
            n = 100;
        }
        size = n / (3.5 * 4);
        Vector3 old_size = model.transform.localScale;
        model.active = false;
        tmp= new GameObject();
        tmp.name = "TMP";
        //number_change((float)(n));//初始化显示文本
        for (int i = 0; i < n; i++)
        {
            md[i] = GameObject.Instantiate(model);
            
            md[i].active = true;
            md[i].name= i.ToString();
            md[i].transform.parent = tmp.transform;
            md[i].transform.localScale = new Vector3(old_size.x-(old_size.x/(n+1))*(i+1), old_size.y, old_size.z);
            md[i].transform.position = new Vector3(pos_a.x, pos_a.y + (float)(space * (i + 1)), pos_a.z);
            st_a.Push(i);//都先塞进A
        }
        HNT(n, 'A', 'B', 'C');



    }

    // Update is called once per frame
    void Update()
    {
        if (is_auto_run == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

            }
        }
        else
        {
            timer += Time.deltaTime;//开始计时
            if (timer > time)//延迟2S执行
            {
                autorun();
                timer = 0;
            }
        }
    }
    public void autorun()
    {
        if (order_of_hnt[0].Length > current_index)
        {
            char begin = order_of_hnt[0][current_index];
            char end = order_of_hnt[1][current_index];
            move(begin, end);
            current_index += 1;
        }
       
    }
    void HNT(int n, char a, char b, char c)//汉诺塔主体函数
    {
        if (n == 1)
        {
            Console.WriteLine(a + "-->" + c);
            //Debug.Log(a + "-->" + c);
            //move(a, c);
            order_of_hnt[0] += a;
            order_of_hnt[1] += c;
        }
        else
        {
            HNT(n - 1, a, c, b);
            //Console.WriteLine(a + "-->" + c);
            //Debug.Log(a + "-->" + c);
            //move(a, c);
            order_of_hnt[0] += a;
            order_of_hnt[1] += c;
            HNT(n - 1, b, a, c);
        }
    }
    void move(char start, char end)//物体现在的位置   物体将要移动到的位置
    {
        int index_begin = 0;//打算移动的方块
        Vector3 pos_end = new Vector3();
        //GameObject target = new GameObject();
        //六种情况
        //检测输入
        if (start == 'A')
        {
            index_begin = Convert.ToInt32(st_a.Pop());
        }

        if (start == 'B')
        {
            index_begin = Convert.ToInt32(st_b.Pop());
        }

        if (start == 'C')
        {
            index_begin = Convert.ToInt32(st_c.Pop());
        }
        if (end == 'A')
        {
            st_a.Push(index_begin);
            int num = st_a.Count;
            pos_end = new Vector3(pos_a.x, pos_a.y + num * space, pos_a.z);
            StartCoroutine(mobe_obj(md[index_begin], pos_end, time));
            //md[index_begin].transform.position = pos_end;//赋值
        }
        if (end == 'B')
        {
            st_b.Push(index_begin);
            int num = st_b.Count;
            pos_end = new Vector3(pos_b.x, pos_b.y + num * space, pos_b.z);
            //while (md[index_begin].transform.position != pos_end)
            //{
            //    md[index_begin].transform.position = Vector3.MoveTowards(md[index_begin].transform.position, pos_end,0.001f);
            // }
            StartCoroutine(mobe_obj(md[index_begin], pos_end, time));
            //md[index_begin].transform.position = pos_end;


        }
        if (end == 'C')
        {
            st_c.Push(index_begin);
            int num = st_c.Count;
            pos_end = new Vector3(pos_c.x, pos_c.y + num * space, pos_c.z);
            StartCoroutine(mobe_obj(md[index_begin], pos_end, time));
            //md[index_begin].transform.position = pos_end;//赋值
        }
    }
    IEnumerator mobe_obj(GameObject obj, Vector3 end,float time)
    {
        Vector3 start = obj.transform.position;
        //float a = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000);
        Vector3 delta=new Vector3(end.x - start.x, end.y - start.y, end.z - start.z);
        delta /= time;
        for (float timer =0; timer <= time; timer += Time.deltaTime)
        {
            obj.transform.position = start + delta * timer;
            yield return 0;
        }
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void resets()
    {
        is_auto_run = false;
        current_index = 0;
        st_a = new Stack();//利用栈储存物体对象，便于进行移动操作
        st_b = new Stack();
        st_c = new Stack();
        order_of_hnt = new String[2];
        Vector3 old_size = model.transform.localScale;

        for (int i = 0; i < n; i++)
        {
            md[i].active = true;
            md[i].name = i.ToString();
            md[i].transform.parent = tmp.transform;
            md[i].transform.localScale = new Vector3(old_size.x - (old_size.x / (n + 1)) * (i + 1), old_size.y, old_size.z);
            md[i].transform.position = new Vector3(pos_a.x, pos_a.y + (float)(space * (i + 1)), pos_a.z);
            st_a.Push(i);//都先塞进A
        }

        HNT(n, 'A', 'B', 'C');
    }

    /// <summary>
    /// 下一步
    /// </summary>
    public void next_step()
    {
        if (order_of_hnt[0].Length > current_index && is_auto_run==false)
        {
            char begin = order_of_hnt[0][current_index];
            char end = order_of_hnt[1][current_index];
            move(begin, end);
            current_index += 1;
        }
    }

    /// <summary>
    /// 自动
    /// </summary>
    public void auto_run_switch()
    {
        is_auto_run = !is_auto_run;
    }

    /// <summary>
    /// 数目
    /// </summary>
    /// <param name="new_num"></param>
    public void number_change(float new_num)
    {
        for (int i = 0; i < n; i++)
        {
            Destroy(md[i]);
        }
        new_num *= 10f;
        new_num += 3;
        n = (int)(new_num);

        if(n>8)
            n=8;
        // Mathf.Clamp(n,3,8);

        //Debug.Log(new_num.ToString());
        if (n > 100)
        {
            n = 100;
        }
        size = n / (3.5 * 4);
        for (int i = 0; i < n; i++)
        {
            md[i] = GameObject.Instantiate(model);
            md[i].active = true;
        }
        show_num.text= "汉诺塔层数："+n.ToString();
        resets();
    }

    /// <summary>
    /// 速度
    /// </summary>
    /// <param name="new_speed"></param>
    public void speed_change(float new_speed)
    {
        float a = -2.8f;
        float b = 3f;
        time = a * (new_speed) + b;

        show_speed.text="当前速度为："+((-a)*new_speed+(b+a));

    }
}
