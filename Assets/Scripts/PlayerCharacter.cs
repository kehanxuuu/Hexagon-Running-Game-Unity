using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    //public bool isAlive;
    public bool inGround;
    //public float speed;
    public float jumpForce;
    public AudioClip jumpClip;
    //public AudioClip dieClip;
    public ParticleSystem[] particle = new ParticleSystem[5];
    //RunNormal, RunFast, RunSlow, Jump, Die
    public int coinCount;
    public int diamondCount;
    public Vector3 posSaved;
    public Quaternion rotSaved;
    public float gameTimeSaved;
    public int coinCountSaved;
    public int diamondCountSaved;
    public GameObject[] coinList; // recover coins from the last checkpoint
    public GameObject[] diamondList;
    public GameObject[] deadAIs; // recover AIs from the last checkpoint
    public int deadTimes;
    public float gameTime;
    public float percentage; // percentage of path passed
    public bool justRecovered; // used in checkpoint.cs

    //Rigidbody rigid;
    //Renderer render;
    //public Animator animator;
    PlayerHUD hud;
    Arrow arrow;
    GameObject AICollided;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        render = GetComponentInChildren<Renderer>();
        animator = GetComponentInChildren<Animator>();
        hud = FindObjectOfType<PlayerHUD>();
        arrow = FindObjectOfType<Arrow>();
        //collider = GetComponent<BoxCollider>();

        render.material.color = Color.yellow;
        SetParticleSystem(0);
        //particle[4].Stop();

        isAlive = false;
        gameObject.SetActive(false);

        recoverBegin();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("Player rot: "+transform.eulerAngles.y);
        if (animator)
        {
            animator.SetBool("InGround", inGround);
        }

        if (!isAlive) SetParticleSystem(-1);

        /*if (isAlive && !inGround) {
            SetParticleSystem(3);
        }*/
        
        //Debug.DrawLine(transform.position+transform.forward*0.2f, transform.position+transform.forward*0.4f, Color.red, 10f);
        
        //arrow.transform.position = transform.position + transform.forward*0.3f + new Vector3(0, 0.25f, 0);
        //arrow.transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    public void Jump()
    {
        if (!isAlive) return;
        float angle = transform.rotation.eulerAngles.y;
        rigid.velocity = new Vector3(Mathf.Sin(2*Mathf.PI*angle/360f), 0, Mathf.Cos(2*Mathf.PI*angle/360f)) * speed * 1.3f;
        //rigid.angularVelocity = Vector3.zero; 
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        AudioSource.PlayClipAtPoint(jumpClip, transform.position);
    }

    /*public void RayCastDown()
    {
        Ray playerRay = new Ray(transform.position, -transform.up);
        RaycastHit playerHit;

        if (Physics.Raycast(playerRay, out playerHit))
        {
            //Debug.DrawLine(playerRay.origin, playerHit.point, Color.green, 10);
            //Debug.Log("1:"+playerHit.collider.gameObject.name);
            Ray secondRay= new Ray(playerHit.point, playerRay.direction);
            RaycastHit hexHit;
            if (Physics.Raycast(secondRay, out hexHit))
            {
                //Debug.DrawLine(secondRay.origin, hexHit.point, Color.red, 10);
                //Debug.Log("2:"+hexHit.collider.gameObject.name);
                
            }
        }
    }*/

    public void SetParticleSystem(int num)
    {
        if (num>=0 && num<particle.Length-1)
            particle[num].gameObject.SetActive(true);
        for (int i=0; i<particle.Length-1; i++)
        {
            if (i!=num) particle[i].gameObject.SetActive(false);
        }
    }

    /*public void KeepParticleSystem(int num)
    {
        for (int i=0; i<particle.Length; i++)
        {
            if (particle[i].gameObject.activeSelf)
                particle[i].gameObject.SetActive(true);
        }
    }*/

    public void AddCoin()
    {
        coinCount += 1;
    }

    public void AddDiamond()
    {
        diamondCount += 1;
    }

    public void recoverCheckpoint()
    {
        render.enabled = true;
        //collider.enabled = true;
        if (AICollided != null)
        {
            Physics.IgnoreCollision(GetComponent<BoxCollider>(), AICollided.GetComponent<BoxCollider>(), false);
            AICollided = null;
        }
        SetParticleSystem(0);

        coinCount = coinCountSaved;
        diamondCount = diamondCountSaved;
        transform.position = posSaved;
        if(transform.position.x == 0 && transform.position.z == 0)
            justRecovered = false;
        else justRecovered = true;
        transform.rotation = rotSaved;
        gameTime = gameTimeSaved;
        //isAlive = true;
        for (int i=0; i<coinList.Length; i++)
        {
            coinList[i].SetActive(true);
        }
        coinList = new GameObject[0];

        for (int i=0; i<diamondList.Length; i++)
        {
            diamondList[i].SetActive(true);
        }
        diamondList = new GameObject[0];

        for (int i=0; i<deadAIs.Length; i++)
        {
            //deadAIs[i].SetActive(true);
            deadAIs[i].GetComponent<AICharacter>().Initialize();
            Physics.IgnoreCollision(GetComponent<BoxCollider>(), deadAIs[i].GetComponent<BoxCollider>(), false);
        }
        deadAIs = new GameObject[0];
    }

    public void recoverBegin()
    {
        render.enabled = true;
        //collider.enabled = true;
        if (AICollided != null)
        {
            Physics.IgnoreCollision(GetComponent<BoxCollider>(), AICollided.GetComponent<BoxCollider>(), false);
            AICollided = null;
        }
        SetParticleSystem(0);

        coinCount = 0;
        diamondCount = 0;
        transform.position = new Vector3(0, 0.5f, 0);
        transform.rotation = Quaternion.Euler(0, 30, 0);
        rigid.velocity = new Vector3(0, 0, 0);
        gameTime = 0;
        deadTimes = 0;
        coinList = new GameObject[0];
        diamondList = new GameObject[0];
        deadAIs = new GameObject[0];

        coinCountSaved = 0;
        diamondCountSaved = 0;
        posSaved = transform.position;
        rotSaved = transform.rotation;
        gameTimeSaved = 0;
        percentage = 0;
        justRecovered = false;
    }

    public void Die(GameObject obj=null)
    {
        isAlive = false;
        deadTimes += 1;
        //particle[4].Play();
        //AudioSource.PlayClipAtPoint(dieClip, transform.position);
        render.enabled = false;
        //collider.enabled = false;
        if (obj!=null)
            AICollided = obj;
        hud.GameEnd();
    }

    public void Stop()
    {
        Scene.gameStopped = true;
        animator.enabled = false;
        SetParticleSystem(-1); // close all particle system
        hud.GameStop();
    }

    public void GameComplete()
    {
        isAlive = false;
        gameObject.SetActive(false);
        hud.GameComplete();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
