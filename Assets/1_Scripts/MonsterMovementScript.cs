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
    [Range(8f,24f)]
    [SerializeField] float DoorSightDivide = 16f; 
    
    [Header("MaskSetting")]
    [SerializeField] private LayerMask TargetMask;  
    [SerializeField] private LayerMask obstacleMask;        // 장애물 대상

    [Header("Draw Line")]
    [Range(0.01f, 1f)][SerializeField]
    private float angle; //선 1개 1개의 각도.
    [Range(0.01f, 1f)][SerializeField]
    private float refreshRate; //선 1개 1개의 각도.
    [Header("Head Setting")][SerializeField]
    private Transform Head; //머리 각도 세팅

    float lastseenplayer;
    IEnumerator co;
    void Awake()
    {
        Anim = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        WayPoints = new Transform[WayPointGroup.childCount];
        for(int i = 1; i <= WayPointGroup.childCount; i++){
            WayPoints[i-1] = WayPointGroup.GetChild(i-1);
        }
        co = MoneterAI();
        StartCoroutine(co);
        lastseenplayer = Time.time - 5f;
        IEnumerator co2 = GetSight();
        StartCoroutine(co2);
    }
    void SetRandomPoint(){
        target = WayPoints[UnityEngine.Random.Range(0,WayPoints.Length)];
        Debug.Log($"Waypoint Setted : {target}");
    }
    IEnumerator GetSight(){
        float FAA = SightAngle/2f;//final add angle
        while(true){
            float NSA2 = -SightAngle/2f;//now sight angle2\
            while(NSA2 < FAA){
                NSA2+=angle;
                float NSAMAX = SightAngle/(2f);
                float NSA = -SightAngle/(2f);//now sight angle\
                Debug.Log($"{NSA2},{NSAMAX}");
                while(NSA < NSAMAX){// 추가각도가 < ((-최대추가각도/2)~(최대 추가각도/2))
                    NSA+=angle;  //전체 90도일때 -45 ~ 45 추가
                    RaycastHit Hitinfo;
                    Vector3 ShotPos = transform.position + new Vector3(0,0.5f,0);//raycast 시작지점
                    Vector3 ShotRad = Quaternion.Euler(NSA2,NSA+Head.eulerAngles.y,0) * Vector3.forward;//raycast 각도
                    Physics.Raycast(ShotPos,ShotRad,out Hitinfo,SightRange,obstacleMask);
                    if(Hitinfo.collider != null){
                        if ( (-SightAngle/DoorSightDivide<NSA)&&(SightAngle/DoorSightDivide>NSA)&&(Hitinfo.collider.CompareTag("Door")||Hitinfo.collider.CompareTag("LockedDoor")) && Hitinfo.distance < 2f){
                            Debug.Log($"I NEED TO DESTORY {Hitinfo.collider.name}");
                            Debug.DrawRay(ShotPos,ShotRad * Hitinfo.distance,Color.cyan,refreshRate);
                            StopCoroutine(co);
                            Agent.velocity = new Vector3(0,0,0);
                            Agent.speed = 0f;
                            Anim.SetBool("run",false);
                            Anim.SetTrigger("Attack");
                            yield return new WaitForSeconds(0.7f);
                            Agent.speed = 0;
                            Hitinfo.collider.tag = "Untagged";
                            if(Hitinfo.collider.GetComponent<Animator>())
                                Hitinfo.collider.GetComponent<Animator>().enabled = false;
                            if(Hitinfo.collider.GetComponent<Rigidbody>()==null){
                                Hitinfo.collider.AddComponent<Rigidbody>();
                            }
                            Hitinfo.collider.GetComponent<Rigidbody>().isKinematic = false;
                            Hitinfo.collider.GetComponent<Rigidbody>().AddForce(Hitinfo.collider.transform.position - transform.position * 5f);
                            Destroy(Hitinfo.collider.gameObject,3.5f);
                            yield return new WaitForSeconds(3.5f);
                            StartCoroutine(co);
                            
                        }
                        else if (Hitinfo.collider.CompareTag("Player") && !target.CompareTag("Player")){
                            Debug.DrawRay(ShotPos,ShotRad * Hitinfo.distance,Color.red,refreshRate);
                            lastseenplayer = Time.time;
                            target = Hitinfo.collider.transform;
                            Debug.Log("I SAW PLAYER!!");
                            StopCoroutine(co);
                            StartCoroutine(co);
                        }
                        else{
                            if(((-SightAngle/DoorSightDivide)<NSA)&&((SightAngle/DoorSightDivide)>NSA))
                                Debug.DrawRay(ShotPos,ShotRad * Hitinfo.distance,Color.orange,refreshRate);
                            else
                                Debug.DrawRay(ShotPos,ShotRad * Hitinfo.distance,Color.yellow,refreshRate);
                        }
                    }
                    else{
                        if(((-SightAngle/DoorSightDivide)<NSA)&&((SightAngle/DoorSightDivide)>NSA))
                            Debug.DrawRay(ShotPos,ShotRad * SightRange,Color.darkGreen,refreshRate);
                        else
                            Debug.DrawRay(ShotPos,ShotRad * SightRange,Color.green,refreshRate);
                    }  
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
        
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
                    SetRandomPoint();
                    Anim.SetBool("run",false);
                    yield return new WaitForSeconds(3.5f);
                    Anim.SetBool("run",true);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target.CompareTag("Player") && Time.time - lastseenplayer > 7f){
            Debug.Log("I nEed tO FInD PLAYER!!");
            SetRandomPoint();
        }
    }
}
