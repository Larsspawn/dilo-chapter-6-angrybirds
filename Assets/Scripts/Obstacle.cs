using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float health = 100f;
    public int scoreValue = 100;

    private Rigidbody2D rigidbody;
    private GameController gameControl;
    [SerializeField] private GameObject destroyFX;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        gameControl = FindObjectOfType<GameController>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        float damage = 0;

        if (other.gameObject.tag == "Bird" && other.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();

            if (rb.velocity.magnitude > 1f)
                damage = rb.velocity.magnitude * 15f;
        }
        else if(other.gameObject.tag == "Obstacle" && other.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();

            damage = rb.velocity.magnitude * 2f;
        }

        if (rigidbody.velocity.magnitude > 0)
        {
            damage += rigidbody.velocity.magnitude * 3.75f;
            //Debug.Log("FALL DMG  :  " + damage);
        }

        health -= damage;

        if (health <= 0)
        {
            DestroyObstacle();
        }
    }

    public void DestroyObstacle()
    {
        int rand = Random.Range(1,4);

        if (rand == 1)
            AudioManager.PlaySound(AudioManager.Sound.woodDestroyed1);
        else
            AudioManager.PlaySound(AudioManager.Sound.woodDestroyed1);

        if (gameControl._isGameEnded == false)
        {
            gameControl.currScore += scoreValue;
            FindObjectOfType<UIController>().UpdateScore(gameControl.currScore);
        }
        
        Instantiate(destroyFX, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
