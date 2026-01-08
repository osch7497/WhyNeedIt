using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovementScript : MonoBehaviour
{
    NavMeshAgent Agent;
    Animator Anim;
    [Header ("Now Target(monster tracing)")]
    [SerializeField]private Transform target;
    private Transform[] WayPoints;
    [Header("TargetGroup")]
    [SerializeField] private Transform WayPointGroup;
    [Header("RaySetting")]
    [SerializeField] float SightAngle; 
    [SerializeField] float SightRange; 
    [Header("TargetSetting (if it is null, it setted automatic)")]
    [Header("타겟 설정(비어있으면 자동으로 사람으로 설정됨)")]
    [SerializeField] Transform PlayerTarget;
    
    [Header("MaskSetting")]
    [SerializeField] private LayerMask TargetMask;  
    [SerializeField] private LayerMask obstacleMask;        // 장애물 대상

    [Header("Draw Line")]
    [Range(0.01f, 1f)]
    [SerializeField] private float angle; //선 1개 1개의 각도.
    [Header("Head and Neck Setting")]
    [SerializeField] private Transform Head; //머리 각도 세팅
    [SerializeField] private Transform Neck; //머리 각도 세팅

    float lastseenplayer;
    void Awake()
    {
        Anim = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        WayPoints = new Transform[WayPointGroup.childCount];
        for(int i = 1; i <= WayPointGroup.childCount; i++){
            WayPoints[i-1] = WayPointGroup.GetChild(i-1);
        }
        if(PlayerTarget == null)
            PlayerTarget = GameObject.FindWithTag("Player").transform;
        IEnumerator co = MoneterAI();
        StartCoroutine(co);
        lastseenplayer = Time.time - 5f;
    }
    void SetRandomPoint(){
        target = WayPoints[UnityEngine.Random.Range(0,WayPoints.Length)];
        Debug.Log($"Waypoint Setted : {target}");
    }
    IEnumerator GetSight(){
        
        float FAA = SightAngle/2f;//final add angle
        float NSA = -SightAngle/2f;//now sight angle\
        while(NSA < FAA){// 추가각도가 < ((-최대추가각도/2)~(최대 추가각도/2))
            NSA+=angle;  //전체 90도일때 -45 ~ 45 추가
            RaycastHit Hitinfo;
            Vector3 ShotPos = transform.position + new Vector3(0,0.5f,0);//raycast 시작지점
            Vector3 ShotRad = Quaternion.Euler(0,NSA+Head.eulerAngles.y,0) * Vector3.forward;//raycast 각도
            Physics.Raycast(ShotPos,Quaternion.Euler(0,NSA+Head.eulerAngles.y,0) * Vector3.forward,out Hitinfo,SightRange,obstacleMask);
            if(Hitinfo.collider != null){
                float Distance = Vector3.Distance(transform.position,Hitinfo.collider.transform.position);
                if (Hitinfo.collider.CompareTag("Player")){
                    Debug.DrawRay(ShotPos,ShotRad * Distance,Color.red);
                    lastseenplayer = Time.time;
                    target = Hitinfo.collider.transform;
                }
                else{
                    Debug.DrawRay(ShotPos,ShotRad * Distance,Color.yellow);
                }
            }
            else{
               Debug.DrawRay(ShotPos,ShotRad * SightRange,Color.green);
            }
            
        }
        yield return null;
    }
    IEnumerator MoneterAI(){
        if(target == null)
            SetRandomPoint();
        while(true){
            Agent.SetDestination(target.position);
            if(Vector3.Distance(transform.position,target.position) > 1.5f){
                    Agent.speed = 5;
                Anim.SetBool("run",true);
            }
            else{
                Anim.SetBool("run",false);
                if(target.CompareTag("Player")){
                    Agent.speed = 0;
                    Anim.SetTrigger("Attack");
                    yield return new WaitForSeconds(2f);
                }else{
                    Anim.SetBool("run",false);
                    yield return new WaitForSeconds(3.5f);
                    Anim.SetBool("run",true);
                    SetRandomPoint();
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        IEnumerator co = GetSight();
        StartCoroutine(co);
        StartCoroutine(GetSight());
        Debug.Log($"Agent enabled:{Agent.enabled}");
        if(target.CompareTag("Player") && Time.time - lastseenplayer > 5f){
            SetRandomPoint();
        }
    }
}
