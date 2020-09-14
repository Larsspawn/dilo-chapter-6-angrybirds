using System.Collections;
using UnityEngine;

public class BoomBird : Bird
{
    [SerializeField]
    public float explosionForce = 200f;
    public float explosionRadius = 5f;
    public bool hasExploded = false;
    public GameObject explosionFX;

    [SerializeField] private CameraShake cameraShake;

    private void OnCollisionEnter2D(Collision2D other)
    {
        StartCoroutine(ExplodeWithDelay(1f));
    }

    public void Explode()
    {
        if (State == BirdState.Thrown && !hasExploded)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<Rigidbody2D>() != null)
                {
                    Vector2 explodeDir = (Vector2)(colliders[i].transform.position - transform.position).normalized;
                    float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
                    colliders[i].GetComponent<Rigidbody2D>().AddForce(explodeDir * (explosionRadius - distance) * explosionForce);
                
                    if (colliders[i].gameObject.CompareTag("Enemy"))
                    {
                        Destroy(colliders[i].gameObject, 0.5f);
                    }
                    else if (colliders[i].gameObject.CompareTag("Obstacle"))
                    {
                        Obstacle obstacle = colliders[i].GetComponent<Obstacle>();

                        obstacle.health -= (explosionRadius - distance) * 180f;

                        if (obstacle.health <= 0)
                            obstacle.DestroyObstacle();
                    }
                }
            }

            hasExploded = true;

            StartCoroutine(cameraShake.Shake(.15f, .4f));
            Instantiate(explosionFX, transform.position, Quaternion.identity);
            AudioManager.PlaySound(AudioManager.Sound.explosion1);

            Destroy(gameObject, .15f);
        }
    }

    public IEnumerator ExplodeWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Explode();
    }

    public override void OnTap()
    {
        Explode();
    }

    public override void PlaySoundFlying()
    {
        AudioManager.PlaySound(AudioManager.Sound.bird04Flying);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public override void PlaySoundCollided()
    {
        int rand = Random.Range(0,10);

        if (rand <= 1)
            AudioManager.PlaySound(AudioManager.Sound.bird04Collision1);
        else if (rand > 1 && rand <= 3)
            AudioManager.PlaySound(AudioManager.Sound.bird04Collision1);
        else if (rand > 3 && rand <= 5)
            AudioManager.PlaySound(AudioManager.Sound.bird04Collision1);
    }
}
