using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public SlingShooter SlingShooter;
    public TrailController TrailController;
    public List<Bird> Birds;
    public List<Enemy> Enemies;
    public BoxCollider2D TapCollider;
    private Bird _shotBird;

    [Space]

    public int currScore;
    public int birdScore = 1000;

    [HideInInspector] public bool isPaused;
    [HideInInspector] public bool _isGameEnded = false;

    private UIController uiControl;

    private void Start()
    {
        uiControl = FindObjectOfType<UIController>();

        for(int i = 0; i < Birds.Count; i++)
        {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrail;
        }

        for(int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        TapCollider.enabled = false;

        SlingShooter.InitiateBird(Birds[0]);
        _shotBird = Birds[0];

        AudioManager.PlaySound(AudioManager.Sound.levelStart);
        AudioManager.PlayMusicLoop(AudioManager.Sound.ambient1);    // Plays the ambient sounds on levels
    
        SetPause(false);
    }   

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            
        }
    }

    private void OnMouseUp()
    {
        if(_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }

    public void ChangeBird()
    {
        TapCollider.enabled = false;

        if (_isGameEnded) return;

        Birds.RemoveAt(0);

        if(Birds.Count > 0)
        {
            SlingShooter.InitiateBird(Birds[0]);
            _shotBird = Birds[0];
        }

        CheckLevelFail();
    }

    public void CheckLevelFail()
    {
        if (Birds.Count == 0 && !_isGameEnded && Enemies.Count > 0)
        {
            StartCoroutine(InitiateLevelFailed());
        }
    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for(int i = 0; i < Enemies.Count; i++)
        {
            if(Enemies[i].gameObject == destroyedEnemy)
            {
                Enemies.RemoveAt(i);
                break;
            }
        }

        if(Enemies.Count == 0)
        {
            _isGameEnded = true;

            StartCoroutine(InitiateCelebration());
        }
    }

    public void AssignTrail(Bird bird)
    {
        TrailController.SetBird(bird);
        StartCoroutine(TrailController.SpawnTrail());
        TapCollider.enabled = true;
    }

    private IEnumerator InitiateCelebration()
    {
        yield return new WaitForSeconds(2f);

        AudioManager.PlaySound(AudioManager.Sound.levelClear);

        StartCoroutine(InitiateLevelCompleted());
    }

    private IEnumerator InitiateLevelCompleted()
    {
        yield return new WaitForSeconds(2f);

        AudioManager.PlaySound(AudioManager.Sound.levelComplete);

        for (int i = 0; i < Birds.Count; i++)
        {
            if (Birds[i].State == Bird.BirdState.Idle)
            {
                currScore += birdScore;
                uiControl.UpdateScore(currScore);
            }
        }

        uiControl.SetVisibility(uiControl.uiLevelCleared, true);
    }

    private IEnumerator InitiateLevelFailed()
    {
        yield return new WaitForSeconds(2f);

        if (_isGameEnded == false)
        {
            AudioManager.PlaySound(AudioManager.Sound.levelFailed);

            uiControl.SetVisibility(uiControl.uiLevelFailed, true);
        }
    }

    public void SetPause(bool state)
    {
        isPaused = state;

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
