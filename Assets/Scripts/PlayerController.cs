using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerCharacter character;
    public float speedChangeTime = 1f;
    public AudioClip speedUpClip;
    public AudioClip speedDownClip;
    public AudioClip dieClip;
    //public AudioClip bombClip;
    public ParticleSystem bombParticle;
    public GameObject minimap;
    Vector3 turnPosition;
    Vector3 jumpHexPosition;
    public bool waitToTurn;
    public bool waitToTransmit;
    bool countJump;
    bool countSpeedUp;
    bool countSpeedDown;
    float timeJump;
    float speedMultiplier;
    float timeSpeed;
    bool turn_left, turn_right, stop, noTurn;
    CameraFollowAlign cameraScript;
    ActiveCamera activeCamera;
    int goReverseCount;
    Vector3 positionPreviousFrame;
    WrongDirection wrongDirText;

    public static string[] special_hex_name = {
        "speedUp(Clone)",
        "speedDown(Clone)",
        "jump(Clone)",
    };

    //public bool printInGround = false;

    public void Initialize()
    {
        waitToTurn = false;
        waitToTransmit = false;
        countJump = false;
        countSpeedUp = false;
        countSpeedDown = false;
        timeJump = 0;
        speedMultiplier = 1;
        timeSpeed = 0;
        noTurn = false;
        goReverseCount = 0;
        positionPreviousFrame = character.transform.position;
        wrongDirText = minimap.GetComponent<WrongDirection>();
    }

    void Awake()
    {
        character = FindObjectOfType<PlayerCharacter>();
        //cameraScript = FindObjectOfType<CameraFollowNoTurn>();
        cameraScript = FindObjectOfType<CameraFollowAlign>();
        activeCamera = FindObjectOfType<ActiveCamera>();
        Initialize();
    }
    
    // Update is called once per frame
    void Update()
    {
        if(!character.isAlive || Scene.gameStopped)
        {
            return;
        }
        stop = Input.GetKeyDown(KeyCode.X);
        if (stop) character.Stop();

        /*if (waitToTransmit)
        {
            Invoke("ControlLogic", 20f);
            waitToTransmit = false;
        }*/
        ControlLogic();

    }

    void ControlLogic()
    {
        character.gameTime += Time.deltaTime;

        //if (Scene.coinRestart)  Scene.coinRestart = false;

        Collider collider = character.HexagonCheck();
        if (collider==null) character.inGround = false;
        else {
            character.inGround = true;
            if (collider.GetComponent<HexagonParam>())
            {
                //cameraScript.follow = collider.GetComponent<HexagonParam>().follow;
                activeCamera.vcamF = collider.GetComponent<HexagonParam>().follow;
                //if (!cameraScript.follow)
                if (!activeCamera.vcamF)
                {
                    cameraScript.y_angle = collider.GetComponent<HexagonParam>().angle;
                    Vector3 dir_1 = Matrix4x4.Rotate(character.transform.rotation).MultiplyVector(Vector3.forward);
                    Vector3 dir_2 = Matrix4x4.Rotate(Quaternion.Euler(0, cameraScript.y_angle, 0)).MultiplyVector(Vector3.forward);
                    if (Vector3.Dot(dir_1, dir_2)<0)
                    {
                        cameraScript.y_angle = -(180-cameraScript.y_angle);
                        if (cameraScript.y_angle < -150) cameraScript.y_angle += 360;
                    }
                }
            }
        }

        if (collider==null && character.transform.position.y<0.5 && !countJump)
            //Debug.Log("Get null in collision detection");
            {
                character.particle[4].Play();
                AudioSource.PlayClipAtPoint(dieClip, transform.position);

                character.Die();
            }

        // Count Jump
        if (countJump)
        {
            timeJump += Time.deltaTime;

            if (timeJump >= 1f)
            {
                timeJump = 0;
                countJump = false;
                character.SetParticleSystem(0);
            }
        }

        if (countSpeedUp || countSpeedDown)
        {
            timeSpeed += Time.deltaTime;
            if (timeSpeed >= speedChangeTime)
            {
                timeSpeed = 0;
                speedMultiplier = 1;
                countSpeedUp = false;
                countSpeedDown = false;
                character.SetParticleSystem(0);
            }
        }
        /*if (printInGround)
            Debug.Log("waitToTurn: "+waitToTurn);*/

        if(!waitToTurn)
        {
            if(character.inGround) //如果转向，先不考虑脚下石块的功能
            {
                noTurn = false;
                switch(collider.gameObject.name)
                {
                    case "grass(Clone)":
                    case "stone(Clone)":
                    case "sand(Clone)":
                    case "water(Clone)":
                        character.Move(1*speedMultiplier);
                        //character.SetParticleSystem(0);
                        break;
                    case "speedUp(Clone)":
                        character.Move(2);
                        if (!countSpeedUp)
                        {
                            countSpeedUp = true;
                            speedMultiplier = 2f;
                            timeSpeed = Time.deltaTime;
                            character.SetParticleSystem(1);
                            AudioSource.PlayClipAtPoint(speedUpClip, character.transform.position);
                        }
                        break;
                    case "speedDown(Clone)":
                        character.Move(0.5f);
                        if (!countSpeedDown)
                        {
                            countSpeedDown = true;
                            speedMultiplier = 0.5f;
                            timeSpeed = Time.deltaTime;
                            character.SetParticleSystem(2);
                            AudioSource.PlayClipAtPoint(speedDownClip, character.transform.position);
                        }
                        break;
                    case "jump(Clone)":
                        if(!countJump)
                        {
                            character.Jump();
                            countJump = true;
                            speedMultiplier = 1;
                            timeSpeed = 0;
                            character.SetParticleSystem(3);

                            //printInGround = false;
                        }
                        break;
                    case "checkpoint(Clone)":
                        character.Move(1);
                        character.SetParticleSystem(0);
                        break;
                    case "start(Clone)":
                        character.Move(1);
                        character.SetParticleSystem(0);
                        break;
                    case "bomb(Clone)":
                        bombParticle.Play();
                        //AudioSource.PlayClipAtPoint(bombClip, character.transform.position);
                        character.Die();
                        break;
                    default:
                        break;
                }
            }
        }
        else {
            character.transform.position = Vector3.MoveTowards(character.transform.position, turnPosition, character.speed*Time.deltaTime*0.7f);
            /*if (printInGround)
            {
                Debug.Log("character.transform.position.x: "+character.transform.position.x);
                Debug.Log("turnPosition.x: "+turnPosition.x);
                Debug.Log("character.transform.position.z: "+character.transform.position.z);
                Debug.Log("turnPosition.z: "+turnPosition.z);
            }*/
            if (collider!=null && collider.gameObject.name == "jump(Clone)")
            {
                noTurn = true;
                if(Mathf.Abs(character.transform.position.x-turnPosition.x)<0.1 && Mathf.Abs(character.transform.position.z-turnPosition.z)<0.3)
                    waitToTurn = false; // make player jump in advance, not after reaching the center
            }
            else{
                if (character.transform.position == turnPosition)
                    waitToTurn = false;
            }
        }
        //Debug.Log("Pos1: "+character.transform.position);
        //Debug.Log("Pos2: "+turnPosition);

        // Turning Logic
        turn_left = Input.GetKeyDown(KeyCode.LeftArrow);
        turn_right = Input.GetKeyDown(KeyCode.RightArrow);
        if (collider!=null && !((IList)special_hex_name).Contains(collider.gameObject.name))
        {
            if (!noTurn && !countJump && !countSpeedUp && !countSpeedDown && (turn_left||turn_right))
            {
                if(character.inGround)
                {
                    float angle = turn_left? 300:60;
                    character.transform.rotation = Quaternion.Euler(0, angle, 0) * character.transform.rotation;
                    turnPosition = character.GetHexTurnPosition(collider);
                    waitToTurn = true;
                }
                //else Debug.Log("Get null in collision detection");
            }
        }

        //Debug.Log("positionPreviousFrame.z: "+ positionPreviousFrame.z);
        //Debug.Log("transform.position.z: "+ transform.position.z);
        if (collider!=null) {
            if (positionPreviousFrame.z > collider.transform.position.z)
            {
                goReverseCount += 1;
                //Debug.Log("goReverseCount: "+goReverseCount);
                if (goReverseCount > 10)
                    wrongDirText.ShowWarning();
            }
            else if (positionPreviousFrame.z < collider.transform.position.z)
            {
                goReverseCount = 0;
                wrongDirText.StopWarning();
            }

            positionPreviousFrame = collider.transform.position;
        }
    }

}
