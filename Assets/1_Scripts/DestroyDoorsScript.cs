using UnityEngine;

public class DestroyDoorsScript : MonoBehaviour
{
    [Header("부서질 문들 설정")]
    [SerializeField] private GameObject Door1;
    [SerializeField] private GameObject Door2;

    Rigidbody rb1,rb2;
    void Awake()
    {
        rb1 = Door1.GetComponent<Rigidbody>();
        rb2 = Door2.GetComponent<Rigidbody>();
    }
    public void destoryDoor(Vector3 monsterPos){
        rb1.isKinematic = false;
        rb2.isKinematic = false;
        rb1.transform.GetComponent<Animator>().enabled = false;
        rb2.transform.GetComponent<Animator>().enabled = false;
        monsterPos = new Vector3(monsterPos.x, 0,monsterPos.z);
        rb1.AddForce((monsterPos-rb1.position)*-5f);
        rb2.AddForce((monsterPos-rb2.position)*5f);
        Destroy(Door1,4.5f);
        Destroy(Door2,4.5f);
    }
}
