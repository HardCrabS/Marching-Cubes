using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;

    float speed = 10f;
    float damage = 10f;
    ParticleSystem impactFX;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    public void SetImpactFX(ParticleSystem impactFX)
    {
        this.impactFX = impactFX;
    }

    void CheckCollisions(float distance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider);
        }
    }

    void OnHitObject(Collider c)
    {
        HealthSystem hs = c.GetComponent<HealthSystem>();
        if (hs)
        {
            hs.TakeDamage(damage);
        }

        if (impactFX)
        {
            var particles = Instantiate(impactFX, transform.position, Quaternion.identity);
            Destroy(particles.gameObject, particles.main.duration);
        }
        Destroy(gameObject);
    }
}
