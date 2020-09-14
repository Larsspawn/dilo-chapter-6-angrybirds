using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        string tag = col.gameObject.tag;
        if (tag == "Bird" || tag == "Enemy" || tag == "Obstacle")
        {
            Destroy(col.gameObject);

            if (tag == "Bird")
                AudioManager.PlaySound(AudioManager.Sound.birdDestroyed);
            else if (tag == "Enemy")
                AudioManager.PlaySound(AudioManager.Sound.pigDestroyed);
        }
    }
}
