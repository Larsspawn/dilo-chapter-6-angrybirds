using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Thrown, HitSomething }
    [HideInInspector] public GameObject Parent;
    [HideInInspector] public Rigidbody2D RigidBody;
    [HideInInspector] public CircleCollider2D Collider;

    private BirdState _state;
    private float _minVelocity = 0.05f;
    private bool _flagDestroy = false;

    public UnityAction<Bird> OnBirdShot = delegate { };
    public UnityAction OnBirdDestroyed = delegate { };

    public BirdState State { get { return _state; } }

    [SerializeField] private GameObject destroyFX; 

    private void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<CircleCollider2D>();

        RigidBody.bodyType = RigidbodyType2D.Kinematic;
        Collider.enabled = false;
        _state = BirdState.Idle;
    }

    private void FixedUpdate()
    {
        if(_state == BirdState.Idle && 
            RigidBody.velocity.sqrMagnitude >= _minVelocity)
        {
            _state = BirdState.Thrown;
        }

        if ((_state == BirdState.Thrown || _state == BirdState.HitSomething) &&
            RigidBody.velocity.sqrMagnitude < _minVelocity &&
            !_flagDestroy)
        {
            //Hancurkan gameobject setelah 2 detik
            //jika kecepatannya sudah kurang dari batas minimum
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _state = BirdState.HitSomething;

        PlaySoundCollided();
    }

    private IEnumerator DestroyAfter(float second)
    {
        yield return new WaitForSeconds(second);

        AudioManager.PlaySound(AudioManager.Sound.birdDestroyed);
        Instantiate(destroyFX, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    public void MoveTo(Vector2 target, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot(Vector2 velocity, float distance, float speed)
    {
        Collider.enabled = true;
        RigidBody.bodyType = RigidbodyType2D.Dynamic;
        RigidBody.velocity = velocity * speed * distance;

        OnBirdShot(this);

        AudioManager.PlaySound(AudioManager.Sound.birdShot);
        PlaySoundFlying();
    }

    public virtual void PlaySoundFlying()
    {
        AudioManager.PlaySound(AudioManager.Sound.bird01Flying);
    }

    public virtual void PlaySoundCollided()
    {
        int rand = Random.Range(0,10);

        if (rand <= 1)
            AudioManager.PlaySound(AudioManager.Sound.bird01Collision1);
        else if (rand > 1 && rand <= 3)
            AudioManager.PlaySound(AudioManager.Sound.bird01Collision1);
        else if (rand > 3 && rand <= 5)
            AudioManager.PlaySound(AudioManager.Sound.bird01Collision1);
    }

    public virtual void OnTap()
    {
        //Do nothing
    }

    private void OnDestroy()
    {
        if(_state == BirdState.Thrown || _state == BirdState.HitSomething)
            OnBirdDestroyed();
    }
}
