using UnityEngine;

public class YellowBird : Bird
{
    [SerializeField]
    public float _boostForce = 100;
    public bool _hasBoost = false;
    public int maxObstacleDestroyedOnBoost = 7;

    /*private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.GetComponent<Obstacle>() != null && _hasBoost)
        {
            other.gameObject.GetComponent<Obstacle>().DestroyObstacle();
        }
    }*/

    private void Update()
    {
        if (_hasBoost)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.CompareTag("Obstacle") && maxObstacleDestroyedOnBoost > 0)
                {
                    colliders[i].gameObject.GetComponent<Obstacle>().DestroyObstacle();
                    maxObstacleDestroyedOnBoost -= 1;
                }
                else if (colliders[i].gameObject.CompareTag("Enemy"))
                {
                    Destroy(colliders[i].gameObject);
                    AudioManager.PlaySound(AudioManager.Sound.pigDamage1);
                }
            }
        }
    }

    public void Boost()
    {
        if (State == BirdState.Thrown && !_hasBoost)
        {
            RigidBody.AddForce(RigidBody.velocity * _boostForce);
            _hasBoost = true;
        }
    }

    public override void OnTap()
    {
        Boost();
    }

    public override void PlaySoundFlying()
    {
        AudioManager.PlaySound(AudioManager.Sound.bird03Flying);
    }

    public override void PlaySoundCollided()
    {
        int rand = Random.Range(0,10);

        if (rand <= 1)
            AudioManager.PlaySound(AudioManager.Sound.bird03Collision1);
        else if (rand > 1 && rand <= 3)
            AudioManager.PlaySound(AudioManager.Sound.bird03Collision1);
        else if (rand > 3 && rand <= 5)
            AudioManager.PlaySound(AudioManager.Sound.bird03Collision1);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.4f);
    }
}
