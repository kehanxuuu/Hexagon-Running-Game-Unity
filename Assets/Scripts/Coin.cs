using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioClip pickupSound;
    //PlayerCharacter character;

    /*void Awake()
    {
        character = FindObjectOfType<PlayerCharacter>();
        Debug.Log(character.gameObject.name);
    }*/

    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerCharacter>();
        if (player)
        {
            player.AddCoin();
            Utils.Add(ref player.coinList, gameObject);
            gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
    }

    private void Update()
    {
        if (!Scene.gameStopped)
            transform.Rotate(new Vector3(0, 360, 0) * Time.deltaTime);
        if (Scene.coinRestart)
            transform.rotation = Quaternion.identity;
    }
}