using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingManager : MonoBehaviour
{
    [SerializeField] GameObject[] potentialMatches;
    [SerializeField] Transform target;
    [SerializeField] Animator doorAnimator;
    [SerializeField] ParticleSystem congrats;
    [SerializeField] Transform player;
    [SerializeField] AudioClip sad;
    public AudioClip happy;
    private AudioSource audio;

    private int index = -1;
    // Start is called before the first frame update
    void Start()
    {
        getNextUser();
        audio = GetComponent<AudioSource>();
    }

    public void getNextUser()
    {
        index += 1;
        if (index < potentialMatches.Length)
        {
            
            StartCoroutine(transpaortUser(potentialMatches[index].transform));
            
        }
        
    }

    public void OnGrab()
    {
        StartCoroutine(checkIsMatch());
    }

    IEnumerator checkIsMatch()
    {
        yield return new WaitForSeconds(2);
        potentialMatches[index].SetActive(false);
        if (Vector3.Distance(potentialMatches[index].transform.position, player.position) >= 1)
        {
            audio.PlayOneShot(sad);
            Debug.Log("BAD MATCH");
        }
        else
        {
            audio.PlayOneShot(happy);
            congrats.Play();
            yield return new WaitForSeconds(3);
            congrats.Stop();
            congrats.Clear();
        }
        getNextUser();
    }

    IEnumerator transpaortUser(Transform user)
    {
        float totalMovementTime = 3f; //the amount of time you want the movement to take
        float currentMovementTime = 0f;//The amount of time that has passed
        Vector3 orgin = user.position;
        doorAnimator.SetBool("character_nearby", true);
        while (Vector3.Distance(user.position, target.position) > 0.1)
        {
            currentMovementTime += Time.deltaTime;
            user.position = Vector3.Lerp(orgin, target.position, currentMovementTime / totalMovementTime);
            yield return null;
        }
        Debug.Log("close doors!!");
        doorAnimator.SetBool("character_nearby", false);
    }
}
