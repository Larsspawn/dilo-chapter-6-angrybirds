using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float Health = 50f;

    public UnityAction<GameObject> OnEnemyDestroyed = delegate { };

    private bool _isHit = false;

    private Rigidbody2D rigidbody;
    private GameController gameControl;
    private AudioSource audio;
    [SerializeField] private GameObject destroyFX;

    public int scoreValue = 1000;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody2D>();
        gameControl = FindObjectOfType<GameController>();
    }

    private void OnDestroy()
    {
        if (_isHit)
        {
            OnEnemyDestroyed(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Rigidbody2D>() == null) return;

        if (other.gameObject.tag == "Bird")     // Damage hit from bird
        {
            _isHit = true;
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Obstacle")     // Damage hit from obstacle
        {
            float damage = other.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 15;
            Health -= damage;
            //Debug.Log("OBS DMG  :  " + damage);

            if (damage > 4f)
            {
                int rand = Random.Range(1,3);

                if (rand == 1)
                    AudioManager.PlaySound(AudioManager.Sound.pigCollision1);
                else
                    AudioManager.PlaySound(AudioManager.Sound.pigCollision2);
            }
        }

        if (rigidbody.velocity.magnitude > 0)     // Damage hit when the pig is falling or thrown away
        {
            float damage = rigidbody.velocity.magnitude * 25;
            Health -= damage;
            //Debug.Log("FALL DMG  :  " + damage);

            if (damage > 5f)
            {
                int rand = Random.Range(1,3);

                if (rand == 1)
                    AudioManager.PlaySound(AudioManager.Sound.pigCollision1);
                else
                    AudioManager.PlaySound(AudioManager.Sound.pigCollision2);
            }
        }

        if (Health <= 0)
        {
            _isHit = true;

            int rand = Random.Range(1,4);

            if (rand == 1)
                AudioManager.PlaySound(AudioManager.Sound.pigDamage1);
            else if (rand == 2)
                AudioManager.PlaySound(AudioManager.Sound.pigDamage2);
            else if (rand == 3)
                AudioManager.PlaySound(AudioManager.Sound.pigDamage3);
            else
                AudioManager.PlaySound(AudioManager.Sound.pigDamage4);

            if (gameControl._isGameEnded == false)
            {
                gameControl.currScore += scoreValue;
                FindObjectOfType<UIController>().UpdateScore(gameControl.currScore);
            }

            Instantiate(destroyFX, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}