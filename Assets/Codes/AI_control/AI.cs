using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [Header("本体属性设置")]
    [SerializeField] [Tooltip("自身能量")] private float energy = 100f;    //自己的体力属性上限
    [SerializeField] [Tooltip("自身追击时间")] private float hostility = 5f;
    [SerializeField] [Tooltip("能量恢复速度")] private float energy_increase = 35f;
    [Header("技能属性设置")]
    [SerializeField] [Tooltip("启用冲锋攻击")] private bool ishit=true;

    [SerializeField] [Tooltip("冲锋攻击距离")] private float hitdistance;
    [SerializeField] [Tooltip("启用冲刺攻击概率")] private int hitrate=20;
    [SerializeField] [Tooltip("冲锋时间")] private float hittime = 10f;
    [SerializeField] [Tooltip("冲锋攻击速度")] private float hitspeed = 300f;
    [SerializeField] [Tooltip("消耗能量")] private float hitcost = 80f;

    [SerializeField] [Tooltip("启用奔跑攻击")] private bool isrun; //启用奔跑
    [SerializeField] [Tooltip("启用奔跑攻击概率")] private int runrate;
    [SerializeField] [Tooltip("奔跑时间")] private float runtime = 3f;
    [SerializeField] [Tooltip("奔跑速度")] private float runspeed = 6f;
    [SerializeField] [Tooltip("消耗能量")] private float runcost = 20;

    [Header("寻路设置属性")]
    [SerializeField] [Tooltip("激活距离")] private float beginhost_distance = 15f;
    [SerializeField] [Tooltip("追击距离")] private float keephost_distance = 20f;
    [SerializeField] [Tooltip("游击距离")] private float attract_distance = 10f;
    [Header("自身属性状态显示（不可修改）")]
    [SerializeField] [Tooltip("是否激活")] private bool ishost;
    [SerializeField] [Tooltip("是否追击")] private bool iskeephost;
    [SerializeField] [Tooltip("是否游击")] private bool isattract=true;
    [SerializeField] [Tooltip("玩家距离")] private float distance;
    [SerializeField] [Tooltip("是否奔跑")] private bool isrunning;
    [SerializeField] [Tooltip("是否冲锋")] private bool ishitting;
    [SerializeField] [Tooltip("体力能量状态")] private float thisenergy;      //敌人当前体力属性
    [SerializeField] [Tooltip("追击能量状态")] private float thishostility;    //敌人当前追击属性
    [Header("攻击属性")]
    [SerializeField] [Tooltip("击飞速度")] private float hittedtime = 2f;
    [SerializeField] [Tooltip("击飞速度")] private float hittedspend = 30f;
    [SerializeField] [Tooltip("击飞高度")] private float hittedhigh = 1f;
    private float thisrunningtime;
    private float thishittingtime;

    private Vector3 hitposition; //设置击飞方向
    public float thishittedtime;
    private Transform target;   //设置追踪目标的位置
    private Transform myself;   //自己本体
    private Vector3 position;  //自己本体的初始位置
        //自己的名称
    
    public float MoveSpeed = 2.5f; //敌人移动速度
    private NavMeshAgent navMeshAgent;  //设置寻路组件
    public CharacterController characterController;


    void Start()
    {
        myself = gameObject.transform;
        position = myself.transform.position;
        target = GameObject.Find("Player").transform;  //获取游戏中主角的位置，在我的工程里面主角的标签是Player
        characterController = GameObject.Find("Player").GetComponent<CharacterController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = MoveSpeed;  //设置寻路器的行走速度
        if (navMeshAgent == null)
        {
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        }
        thisenergy = energy;
        thishostility = hostility;
        isrunning = false;
        ishitting = false;

    }


    void Update()
    {
        distance = (target.transform.position - myself.transform.position).magnitude;
        if (distance < beginhost_distance && thishostility>=hostility) {
            ishost = true;
        }//
        if (distance > keephost_distance && ishost) {
            iskeephost = true;
            thishostility -= 1f*Time.deltaTime;
            if (thishostility < 0) {
                iskeephost = false;
                ishost = false;
            }
        }
        else { iskeephost = false; }
        if (!ishost&&thishostility<hostility) {
            thishostility += 1f*Time.deltaTime;
        }
        if (distance < attract_distance)
        {
            isattract = true;
        }
        else { 
            isattract = false; 
        }
        if (ishost)
        {

            if (!isattract)
            {
                if (isrun == true && thisenergy >= runcost) {
                    if (!isrunning && !ishitting) {
                        if (Random.Range(1,6000) < runrate) {
                            isrunning = true;
                            thisenergy -= runcost;
                            thisrunningtime = runtime;
                            navMeshAgent.speed = runspeed;
                        }
                    }
                    
                }
                navMeshAgent.SetDestination(target.transform.position);
            }
            else {
                if (ishit == true && thisenergy >= hitcost)
                {
                    if (!isrunning && !ishitting)
                    {
                        if (Random.Range(1, 6000) < hitrate)
                        {
                            ishitting = true;
                            thisenergy -= hitcost;
                            thishittingtime = hittime;
                            navMeshAgent.speed = hitspeed;
                        }
                    }
                }
                if (ishitting) {
                    navMeshAgent.SetDestination(target.transform.position);
                } else {
                    navMeshAgent.SetDestination(target.transform.position + 2 * (myself.transform.position - target.transform.position));
                }
                
            }
        }
        else { 
            navMeshAgent.SetDestination(position); //设置寻路目标
        }
        if (isrunning) {
            thisrunningtime -= Time.deltaTime;
            if (thisrunningtime <= 0 || isattract) {
                isrunning = false;
                navMeshAgent.speed = MoveSpeed;
            }
        }
        if (ishitting)
        {
            thishittingtime -= Time.deltaTime;
            if (thishittingtime <= 0|| distance<3)
            {
                ishitting = false;
                navMeshAgent.speed = MoveSpeed;
                if (distance < 5) {
                    hitposition = target.transform.position - myself.transform.position;
                    thishittedtime = hittedtime;
                    hitposition = hitposition / distance;
                    hitposition.y += hittedhigh;
                }
            }
        }
        if (thisenergy < energy) {
            thisenergy += energy_increase * Time.deltaTime;
        }
        if (thishittedtime > 0) {
            thishittedtime -= Time.deltaTime;

            characterController.Move(hitposition * hittedspend * Time.deltaTime*thishittedtime*thishittedtime/hittedtime/hittedtime);
        }
    }
}
