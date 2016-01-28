using UnityEngine;
using System.Collections;

public class ForwardEnemy : Enemy
{

    public LivingThing target;
    public float movementSpeed;
    public int damage;
    public float freezeTime;

    // Use this for initialization
    void Start()
    {
        freezeTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
		LivingThing status = gameObject.GetComponent<LivingThing> ();
		if (status != null) {
			if (status.life > 0) {
				if (freezeTime <= 0) {
					Vector3 targetPos = target.transform.position;
					float speed = movementSpeed * Time.deltaTime;
					transform.position = Vector3.MoveTowards (transform.position, targetPos, speed);
				} else {
					freezeTime -= Time.deltaTime;
				}
			}
		}
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target.transform.position);
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player") && body.life > 0)
        {
            Player p = c.gameObject.GetComponent<Player>();
            p.Damage(damage);
            gameObject.SetActive(false);
        }
        
    }
}