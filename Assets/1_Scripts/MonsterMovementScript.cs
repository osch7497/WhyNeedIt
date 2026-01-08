using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovementScript : MonoBehaviour
{
    NavMeshAgent Agent;
    Animator Anim;
    private Transform target;
    private Transform[] WayPoints;
    [Header("TargetGroup")]
    [SerializeField] private Transform WayPointGroup;
    [Header("RaySetting")]
    [SerializeField] float SightAngle; 
    [SerializeField] float SightRange; 
    
    
    [Header("MaskSetting")]
    [SerializeField] private LayerMask targetMask;          // 탐색 대상
    [SerializeField] private LayerMask obstacleMask;        // 장애물 대상

    [Header("Draw Line")]
    [Range(0.01f, 1f)]
    [SerializeField] private float angle; //선 1개 1개의 각도.
    void Awake()
    {
        Anim = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        WayPoints = new Transform[WayPointGroup.childCount];
        for(int i = 1; i <= WayPointGroup.childCount; i++){
            WayPoints[i-1] = WayPointGroup.GetChild(i-1);
        }
        IEnumerator co = MoneterAI();
        StartCoroutine(co);
    }
    void SetRandomPoint(){
        target = WayPoints[UnityEngine.Random.Range(0,WayPoints.Length)];
        Debug.Log($"Waypoint Setted : {target}");
    }
    IEnumerator GetSight(){
        float FAA = SightAngle/2f;//final add angle
        float NSA = -SightAngle/2f;//now sight angle
        while(NSA < FAA){
            NSA+=angle;
            RaycastHit Hitinfo;
            Physics.Raycast(transform.position + new Vector3(0,1f,0),transform.rotation * Quaternion.Euler(0,NSA,0) * Vector3.forward,out Hitinfo,15f,obstacleMask);
            float Distance = (Hitinfo.collider != null) ? Vector3.Distance(transform.position,Hitinfo.collider.transform.position) : 15f;
            Debug.DrawRay(transform.position + new Vector3(0,1f,0),transform.rotation * Quaternion.Euler(0,NSA,0) * Vector3.forward * Distance,Color.green);
        }
        yield return null;
    }
    IEnumerator MoneterAI(){
        if(target == null)
            SetRandomPoint();
        while(true){
            Agent.SetDestination(target.position);
            if(Vector3.Distance(transform.position,target.position) > 1.5f){
                Agent.enabled = true;
                Anim.SetBool("run",true);
            }
            else{
                Anim.SetBool("run",false);
                if(target.CompareTag("Player")){
                    Agent.enabled = false;
                    Anim.SetTrigger("Attack");
                    yield return new WaitForSeconds(3.5f);
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
    }
}
