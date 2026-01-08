using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovementScript : MonoBehaviour
{
    NavMeshAgent Agent;
    Animator Anim;
    public Transform target;
    public Transform WayPointGroup;
    private Transform[] WayPoints;
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
        target = WayPoints[Random.Range(0,WayPoints.Length)];
        Debug.Log($"Waypoint Setted : {target}");
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
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Agent enabled:{Agent.enabled}");
    }
}
