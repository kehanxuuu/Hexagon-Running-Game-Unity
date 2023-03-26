using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCheckpoint : MonoBehaviour
{
    public ParticleSystem particle;
    public AudioClip checkPointSound;

    void Awake()
    {
        particle.gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerCharacter>();

        if (player)
        {
            //UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
            AudioSource.PlayClipAtPoint(checkPointSound, transform.position);
            player.GameComplete();
        }
    }
}
