using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 10;
    public GameObject bullet;
    public Transform pistol;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public void Fire()
    {
        GameObject spawnedBullet = Instantiate(bullet, pistol.position, pistol.rotation);
        spawnedBullet.GetComponent<Rigidbody>().velocity = speed * pistol.forward;
        audioSource.PlayOneShot(audioClip);
        Destroy(spawnedBullet, 2);

    }

}
