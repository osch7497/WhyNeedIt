using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MonsterMovementScript : MonoBehaviour
{
    NavMeshAgent Agent;
    Animator Anim;

    private bool isRunning;
    private float timer;

    [Header("Now Target(monster tracing)")]
    [SerializeField] private Transform target;

    private Transform[] WayPoints;

    [Header("TargetGroup")]
    [SerializeField] private Transform WayPointGroup;

    [Header("RaySetting")]
    [SerializeField] float SightAngle;
    [SerializeField] float SightRange;
    [Range(8f, 45f)]
    [SerializeField] float DoorSightDivide = 45f;

    [Header("MaskSetting")]
    [SerializeField] private LayerMask TargetMask;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Draw Line")]
    [Range(0.01f, 1f)][SerializeField] private float angle;
    [Range(0.01f, 1f)][SerializeField] private float refreshRate;

    [Header("Head Setting")]
    [SerializeField] private Transform Head;

    [Header("Attack Setting")]
    [SerializeField][Range(0.1f, 10f)]
    private float PlayerAttackRange = 1.5f;

    float lastseenplayer;
    IEnumerator co;

    void Awake()
    {
        Anim = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();

        WayPoints = new Transform[WayPointGroup.childCount];
        for (int i = 0; i < WayPointGroup.childCount; i++)
            WayPoints[i] = WayPointGroup.GetChild(i);

        co = MoneterAI();
        StartCoroutine(co);

        lastseenplayer = Time.time - 7f;
        StartCoroutine(GetSight());
    }

    void SetRandomPoint()
    {
        target = WayPoints[Random.Range(0, WayPoints.Length)];
    }

    IEnumerator GetSight()
    {
        float FAA = SightAngle / 2f;

        while (true)
        {
            float NSA2 = -SightAngle / 2f;

            while (NSA2 < FAA)
            {
                NSA2 += angle;
                float NSAMAX = (SightAngle - math.abs(NSA2 * 2)) * 0.5f;
                float NSA = -NSAMAX;

                while (NSA < NSAMAX)
                {
                    NSA += angle;

                    RaycastHit Hitinfo;
                    Vector3 ShotPos = transform.position + Vector3.up * 0.5f;
                    Vector3 ShotRad = Quaternion.Euler(NSA2, NSA + Head.eulerAngles.y, 0) * Vector3.forward;

                    Physics.Raycast(ShotPos, ShotRad, out Hitinfo, SightRange, obstacleMask);

                    if (Hitinfo.collider == null) continue;

                    if ((-SightAngle / DoorSightDivide < NSA) && (SightAngle / DoorSightDivide > NSA) && 
                        (Hitinfo.collider.CompareTag("Door") || Hitinfo.collider.CompareTag("LockedDoor")) && Hitinfo.distance < 2f)
                    {
                        StopCoroutine(co);

                        Agent.velocity = Vector3.zero;
                        Agent.speed = 0f;

                        Anim.SetBool("run", false);
                        isRunning = false;
                        Anim.SetTrigger("Attack");

                        AudioManager.instance.PlaySFX("BreakDoor", transform.position, 0.1f);

                        yield return new WaitForSeconds(0.7f);

                        Hitinfo.collider.tag = "Untagged";

                        if (Hitinfo.collider.TryGetComponent(out Animator a))
                            a.enabled = false;

                        if (!Hitinfo.collider.TryGetComponent(out Rigidbody rb))
                            rb = Hitinfo.collider.gameObject.AddComponent<Rigidbody>();

                        rb.isKinematic = false;
                        rb.AddForce((Hitinfo.collider.transform.position - transform.position) * 5f);

                        Destroy(Hitinfo.collider.gameObject, 3.5f);

                        yield return new WaitForSeconds(3.5f);
                        StartCoroutine(co);
                    }
                    
                    else if (Hitinfo.collider.CompareTag("Player"))
                    {
                        lastseenplayer = Time.time;
                        if (!target.CompareTag("Player"))
                        {
                            Anim.SetTrigger("DetectPlayer");
                            AudioManager.instance.PlaySFX("MonsterScreaming", transform.position, volume:0.7f);

                            Head.GetComponent<AimConstraint>().weight = 1f;

                            Agent.speed = 0;
                            StopCoroutine(co);

                            yield return new WaitForSeconds(2f);

                            target = Hitinfo.collider.transform;
                            StartCoroutine(co);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    IEnumerator MoneterAI()
    {
        if (target == null)
            SetRandomPoint();

        while (true)
        {
            Agent.SetDestination(target.position);

            if (Vector3.Distance(transform.position, target.position) > PlayerAttackRange)
            {
                Agent.speed = 5;
                Anim.SetBool("run", true);
                isRunning = true;
            }
            else
            {
                Anim.SetBool("run", false);
                isRunning = false;

                if (target.CompareTag("Player"))
                {
                    AudioManager.instance.PlaySFX("MonsterAttackScreaming", transform.position, volume:0.6f);

                    target.GetComponent<FpsController>().OnAttacked(Head.transform);

                    Agent.speed = 0;
                    Agent.velocity = Vector3.zero;
                    Anim.SetTrigger("Attack");

                    yield return new WaitForSeconds(5f);
                    SceneManager.LoadScene(gameObject.scene.name);
                }
                else
                {
                    SetRandomPoint();
                    yield return new WaitForSeconds(3.5f);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Update()
    {
        if (target.CompareTag("Player") && Time.time - lastseenplayer > 7f)
        {
            Head.GetComponent<AimConstraint>().weight = 0f;
            SetRandomPoint();
        }

        timer += Time.deltaTime;

        if (timer > 0.35f)
        {
            StartCoroutine(FootstepSound());
            timer = 0f;
        }
    }

    IEnumerator FootstepSound()
    {
        string[] SFXs = { "MonsterFootstep1", "MonsterFootstep2", "MonsterFootstep3", "MonsterFootstep4" };

        if (isRunning)
            AudioManager.instance.PlaySFX(SFXs[Random.Range(0, SFXs.Length)], transform.position, volume:2f);

        yield return null;
    }
}
