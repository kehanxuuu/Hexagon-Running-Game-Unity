using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    public AudioClip pickupSound;

    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerCharacter>();
        if (player)
        {
            player.AddDiamond();
            Utils.Add(ref player.diamondList, gameObject);
            gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume:0.7f);
        }
    }

    private void Start()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
    private void Update()
    {
        if (!Scene.gameStopped)
            transform.Rotate(new Vector3(0, 0, 180) * Time.deltaTime);
        if (Scene.coinRestart)
            transform.rotation = Quaternion.Euler(90, 0, 0);
    }
    
}
