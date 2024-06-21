using System.Collections;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public AudioSource audioplayer;
    public AudioClip HeadclipToPlay;
    public AudioClip BodyclipToPlay;
    private Rigidbody rb;
    private bool isHit = false;
    private bool isStanding = true;
    private float timeSinceLastHit = 0f;
    private float autoFallTime = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isStanding && !isHit)
        {
            timeSinceLastHit += Time.deltaTime;
            if (timeSinceLastHit >= autoFallTime)
            {
                timeSinceLastHit = 0;
                StartCoroutine(FallAndReset());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !isHit)
        {
            isHit = true;
            isStanding = false;
            timeSinceLastHit = 0f;  // Reset the timer when hit

            if (!GameManager.Instance.isTiming)
            {
                GameManager.Instance.StartGame();
            }

            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.thisCollider.CompareTag("Head"))
                {
                    audioplayer.clip = HeadclipToPlay;
                    audioplayer.Play();
                    GameManager.Instance.AddScore(3);
                }
                else if (contact.thisCollider.CompareTag("Body"))
                {
                    audioplayer.clip = BodyclipToPlay;
                    audioplayer.Play();
                    GameManager.Instance.AddScore(1);
                }
            }

            StartCoroutine(FallAndReset());
        }
    }

    private IEnumerator FallAndReset()
    {
        float elapsedTime = 0;
        float fallDuration = 0.2f;
        float standDuration = 0.5f;
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(-90, 0, 0);

        while (elapsedTime < fallDuration)
        {
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / fallDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;


        yield return new WaitForSeconds(Random.Range(1, 3));

        elapsedTime = 0;
        while (elapsedTime < standDuration)
        {
            transform.rotation = Quaternion.Lerp(targetRotation, initialRotation, elapsedTime / standDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = initialRotation;



        isHit = false;
        isStanding = true;
        timeSinceLastHit = 0f;  // Reset the timer when standing up again
    }
}
