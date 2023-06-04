using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;

    protected float speed = 10f;
    protected float damage = 10f;
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
            OnHitObject(hit.collider, hit.point);
        }
    }

    protected virtual void OnHitObject(Collider c, Vector3 hitPoint)
    {
        HandleDamage(c);

        if (impactFX)
        {
            var particles = Instantiate(impactFX, transform.position, Quaternion.identity);
            float maxLifetime = particles.main.startLifetime.constantMax;
            foreach (Transform child in particles.transform)
            {
                float lifetime = child.GetComponent<ParticleSystem>().main.startLifetime.constantMax;
                if (lifetime > maxLifetime)
                    maxLifetime = lifetime;
            }
            Destroy(particles.gameObject, maxLifetime);
        }
        Destroy(gameObject);
    }

    protected virtual void HandleDamage(Collider c)
    {
        HealthSystem hs = c.GetComponent<HealthSystem>();
        if (hs)
        {
            hs.TakeDamage(damage);
        }
    }
}
