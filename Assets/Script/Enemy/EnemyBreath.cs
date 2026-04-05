using UnityEngine;

public class EnemyBreath : MonoBehaviour
{
    Vector3 dir;
    float speed;

    public void Init(Vector3 direction, float moveSpeed)
    {
        dir = direction;
        speed = moveSpeed;

        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += dir * speed;
        Destroy(gameObject, 3.0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // É_ÉÅÅ[ÉWèàóù
        }

        Destroy(gameObject);
    }
    
}
