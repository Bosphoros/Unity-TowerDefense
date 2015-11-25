using UnityEngine;
using System.Collections;

public class ForwardEnemy : MonoBehaviour
{

    public LivingThing target;
    public float movementSpeed;
    public float range;
    public int damage;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = target.transform.position;
        float speed = movementSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);

        if (Vector3.Distance(target.transform.position, transform.position) <= range)
        {
            target.Damage(damage);
            GetComponent<Poolable>().Reset();
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target.transform.position);
    }
}