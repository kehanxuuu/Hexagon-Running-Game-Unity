using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : Character
{
    Arrow arrow;
    public AudioClip playerDieClip;
    public AudioClip selfDieClip;
    public ParticleSystem dieParticle;
    public Material AIWin;
    public Material AILose;
    int colorChange = 100;
    int count;
    bool playerDie;
    PlayerCharacter playerScript;
    public Vector3 Reborn1, Reborn2;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        render = GetComponentInChildren<Renderer>();
        arrow = GetComponentInChildren<Arrow>();
        //collider = GetComponent<BoxCollider>();
        Initialize();
    }

    void FixedUpdate()
    {
        if (animator)
        {
            if (Scene.gameStopped)
                animator.SetBool("InGround", false);
            else
                animator.SetBool("InGround", true);
        }
        arrow.transform.position = transform.position + transform.forward*0.3f + new Vector3(0, 0.25f, 0);
        arrow.transform.localRotation = Quaternion.Euler(90, 0, 0);

        if (Scene.gameStopped)
            return;
        count += 1;
        if (count < colorChange)
        {
            //render.material.color = Color.grey;
            render.material = AIWin;
            playerDie = true;
        }
        else{
            //render.material.color = Color.cyan;
            render.material = AILose;
            playerDie = false;
            if (count == 2*colorChange) count = 0;
        }
    }

    public void Stop()
    {
        animator.enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Character")
        {
            if (playerDie)
            {
                var player = collision.gameObject.GetComponent<PlayerCharacter>();
                if (player && player.isAlive)
                {
                    if (player.diamondCount > 0)
                    {
                        dieParticle.Play();
                        player.diamondCount -= 1;
                        AudioSource.PlayClipAtPoint(playerDieClip, transform.position);

                        // AI Change Place
                        Vector3 distance = Reborn1 - collision.gameObject.transform.position;
                        if (Mathf.Sqrt(distance.sqrMagnitude) > 3*Scene.xDistanceHex)
                            Invoke("AIReborn1", 0.1f);
                        else Invoke("AIReborn2", 0.1f);
                    }
                    else {
                        player.particle[4].Play();
                        AudioSource.PlayClipAtPoint(playerDieClip, transform.position);
                        player.Die(gameObject);
                        Physics.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider>(), GetComponent<BoxCollider>(), true);
                    }
                }
            }
            else {
                // AI Die
                render.enabled = false;
                arrow.gameObject.SetActive(false);
                isAlive = false;
                dieParticle.Play();
                AudioSource.PlayClipAtPoint(selfDieClip, transform.position);
                Physics.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider>(), GetComponent<BoxCollider>(), true);
                playerScript = collision.gameObject.GetComponent<PlayerCharacter>();
                Utils.Add(ref playerScript.deadAIs, gameObject);
                playerScript.coinCount += 10;
            }
        }
    }

    void AIReborn1()
    {
        transform.position = Reborn1;
    }

    void AIReborn2()
    {
        transform.position = Reborn2;
    }

    public void Initialize()
    {
        render.enabled = true;
        arrow.gameObject.SetActive(true);
        isAlive = true;
        dieParticle.Stop();
        count = 0;
        playerDie = true;
    }

}
