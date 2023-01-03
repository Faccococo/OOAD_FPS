using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [Header("������������")]
    [SerializeField] [Tooltip("��������")] private string name="enemy";     //��������
    [SerializeField] [Tooltip("��������")] private float energy=100f;    //�Լ���������������
    [SerializeField] [Tooltip("����׷��ʱ��")] private float hostility = 5f; 
    [SerializeField] [Tooltip("�����ָ��ٶ�")] private float energy_increase = 35f;
    [Header("������������")]
    [SerializeField] [Tooltip("���ó�湥��")] private bool ishit;

    [SerializeField] [Tooltip("��湥������")] private float hitdistance;
    [SerializeField] [Tooltip("���ó�̹�������")] private int hitrate;
    [SerializeField] [Tooltip("���ʱ��")] private float hittime = 10f;
    [SerializeField] [Tooltip("��湥���ٶ�")] private float hitspeed = 80f;
    [SerializeField] [Tooltip("��������")] private float hitcost = 80f;

    [SerializeField] [Tooltip("���ñ��ܹ���")] private bool isrun; //���ñ���
    [SerializeField] [Tooltip("���ñ��ܹ�������")] private int runrate;
    [SerializeField] [Tooltip("����ʱ��")] private float runtime=3f;
    [SerializeField] [Tooltip("�����ٶ�")] private float runspeed = 6f;
    [SerializeField] [Tooltip("��������")] private float runcost = 20;

    [Header("Ѱ·��������")]
    [SerializeField] [Tooltip("�������")] private float beginhost_distance = 25f;
    [SerializeField] [Tooltip("׷������")] private float keephost_distance = 35f;
    [SerializeField] [Tooltip("�λ�����")] private float attract_distance = 10f;
    [Header("��������״̬��ʾ�������޸ģ�")]
    [SerializeField] [Tooltip("�Ƿ񼤻�")] private bool ishost;
    [SerializeField] [Tooltip("�Ƿ�׷��")] private bool iskeephost;
    [SerializeField] [Tooltip("�Ƿ��λ�")] private bool isattract;
    [SerializeField] [Tooltip("��Ҿ���")] private float distance;
    [SerializeField] [Tooltip("�Ƿ���")] private bool isrunning;
    [SerializeField] [Tooltip("�Ƿ���")] private bool ishitting;
    [SerializeField] [Tooltip("��������״̬")] private float thisenergy;      //���˵�ǰ��������
    [SerializeField] [Tooltip("׷������״̬")] private float thishostility;    //���˵�ǰ׷������
    private float thisrunningtime;
    private float thishittingtime;

    private Transform target;   //����׷��Ŀ���λ��
    private Transform myself;   //�Լ�����
    private Vector3 position;  //�Լ�����ĳ�ʼλ��
        //�Լ�������
    
    public float MoveSpeed = 2.5f; //�����ƶ��ٶ�
    private NavMeshAgent navMeshAgent;  //����Ѱ·���
   
   


    void Start()
    {
        myself = gameObject.transform;
        position = myself.transform.position;
        target = GameObject.FindWithTag("Player").transform;  //��ȡ��Ϸ�����ǵ�λ�ã����ҵĹ����������ǵı�ǩ��Player
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = MoveSpeed;  //����Ѱ·���������ٶ�
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
        //Debug.Log(position);
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
            navMeshAgent.SetDestination(position); //����Ѱ·Ŀ��
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
            }
        }
        if (thisenergy < energy) {
            thisenergy += energy_increase * Time.deltaTime;
        }

    }
}
