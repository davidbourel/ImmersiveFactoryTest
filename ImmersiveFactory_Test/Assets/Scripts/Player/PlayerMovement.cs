using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    [Space(10)]
    [Header("PLAYER VALUES")]
    [Space(10)]

   

    [Space(10)]

    [SerializeField] private float WalkSpeed = 5f;
    [SerializeField] private float RunSpeedFactor = 3f;

    [Space(10)]

    [SerializeField] private float timeToHaveMaxSpeed = 2f;
    [SerializeField] private AnimationCurve walkCurve;
    [SerializeField] private AnimationCurve runCurve;

    [SerializeField] private LayerMask layerWall;

    [Space(10)]
    [Header("CLIMB VALUES")]
    [Space(10)]

    [SerializeField] private float ClimbSpeed = 5f;
    [SerializeField] private float ClimbSpeedFactor = 2f;

    [Space(10)]
    [Header("JUMP VALUES")]
    [Space(10)]

    [SerializeField] private float JumpHeight = 2f;
    [SerializeField] private float GroundDistance = 0.2f;
    [SerializeField] private bool canUseMultiJump = true;
    [SerializeField] private int maxNumberOfJump = 2;

    [Space(10)]

    [SerializeField] private LayerMask Ground;
    [SerializeField] private Transform _groundChecker;



    private Rigidbody body = null;

    private bool isGrounded = true;

    private float xMov = 0f;
    private float yMov = 0f;
    private Vector3 velocity = Vector3.zero;

    private float RunSpeedFactorValue = 1.0f;
    private float ClimbSpeedFactorValue = 1.0f;

    private int countJump = 0;

    private float moveDuration;
    private bool isMoving;
    private float currentVelocity = 0;

    private float moveRunningDuration;
    private float moveWalkingDuration;
    private bool isRunning = false;
    private bool playerCanMove = false;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (playerCanMove)
        {
            //Check If the player is walking or Running
            CheckPlayerRun();
            CheckPlayerWalk();

            //PlayerMovement Vertical and Horizontal
            PlayerXYMovement();

            //PlayerJump
            isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

            bool wallRight = Physics.Raycast(_groundChecker.transform.position, _groundChecker.transform.right, 2f,layerWall);
            bool wallLeft = Physics.Raycast(_groundChecker.transform.position, -_groundChecker.transform.right, 2f,layerWall);

            if (wallLeft || wallRight && countJump != 0)
            {
                countJump = 0;
            }

            if (Input.GetButtonDown("Jump") && (isGrounded || canUseMultiJump))
            {
                if (isGrounded || wallLeft || wallRight)
                {
                    body.velocity = Vector3.zero;
                    body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
                    countJump = 1;
                }
                else if (canUseMultiJump && countJump < maxNumberOfJump)
                {
                    body.velocity = Vector3.zero;
                    body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
                    countJump++;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (velocity != Vector3.zero)
        {
            body.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
        }
    }

    private void CheckPlayerWalk()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)
        {
            moveDuration += Time.deltaTime;
            currentVelocity = walkCurve.Evaluate(moveDuration / timeToHaveMaxSpeed);
        }
        else
        {
            moveDuration = 0;
            currentVelocity =  1;
        }
    }

    private void CheckPlayerRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isRunning)
        {
            isRunning = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }


        if (isRunning)
        {
            moveRunningDuration += Time.deltaTime;
            RunSpeedFactorValue = 1 + runCurve.Evaluate(moveRunningDuration / timeToHaveMaxSpeed) * RunSpeedFactor;
            ClimbSpeedFactorValue = ClimbSpeedFactor;
            moveWalkingDuration = 0;
        }
        else
        {
            moveWalkingDuration += Time.deltaTime;
            moveRunningDuration = 0;
            RunSpeedFactorValue = 1 + RunSpeedFactor - runCurve.Evaluate(moveWalkingDuration / timeToHaveMaxSpeed) * RunSpeedFactor;
            ClimbSpeedFactorValue = 1;
        }
    }

    private void PlayerXYMovement()
    {
        bool touchRightWall = Physics.Raycast(_groundChecker.transform.position, _groundChecker.transform.right, 2f,layerWall);
        bool touchLeftWall = Physics.Raycast(_groundChecker.transform.position, -_groundChecker.transform.right, 2f,layerWall);

        if ( !touchRightWall && Input.GetAxis("Horizontal") > 0)
        { xMov = Input.GetAxis("Horizontal"); }
        else if (!touchLeftWall && Input.GetAxis("Horizontal") < 0)
        { xMov = Input.GetAxis("Horizontal"); }
        else { xMov = 0; }

        if(touchLeftWall){CanvasManager.Instance.EnableLeftWallImage(true);}else{CanvasManager.Instance.EnableLeftWallImage(false);}
        if (touchRightWall) { CanvasManager.Instance.EnableRightWallImage(true); } else { CanvasManager.Instance.EnableRightWallImage(false); }

        yMov = Input.GetAxis("Vertical");

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * yMov;
        Vector3 movClimb = transform.up * yMov;

        if (Physics.Raycast(_groundChecker.transform.position, _groundChecker.transform.forward, 2f,layerWall) && yMov > 0)
        {
            body.velocity = Vector3.zero;
            velocity = (movHorizontal + movClimb).normalized * ClimbSpeed * ClimbSpeedFactorValue * currentVelocity;
            body.useGravity = false;
        }
        else
        {
            velocity = (movHorizontal + movVertical).normalized * WalkSpeed * RunSpeedFactorValue * currentVelocity;
            body.useGravity = true;
        }
    }

    public void ResetPlayerVelocity()
    {
        body.velocity = Vector3.zero;
    }

    public bool PlayerCanMove
    {
        get
        {
            return playerCanMove;
        }

        set
        {
            playerCanMove = value;
        }
    }

   

}