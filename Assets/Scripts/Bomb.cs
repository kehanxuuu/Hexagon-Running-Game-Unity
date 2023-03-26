using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public ParticleSystem particleSmall;
    public ParticleSystem particleBig;
    public AudioClip bombSound;

    private void Awake()
    {
        particleSmall.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerCharacter>();

        if (player && player.isAlive)
        {
            AudioSource.PlayClipAtPoint(bombSound, transform.position);
            particleSmall.gameObject.SetActive(false);
            particleBig.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (Scene.recoverBomb)
        {
            particleSmall.gameObject.SetActive(true);
            particleBig.gameObject.SetActive(false);
        }
    }
}
