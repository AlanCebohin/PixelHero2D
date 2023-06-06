using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask selectedLayerMask;
    private bool isFacingRight = true, isGrounded;
    private Rigidbody2D playerRB;
    private Transform checkGroundPoint, transformPlayer;
    private Animator animatorStandingPlayer;
    private Animator animatorBallPlayer;
    private int IdSpeed, IdIsGrounded, IdShootArrow, IdCanDoubleJump;
    [SerializeField] private float ballModeWaitingTime;
    private float ballModeTimeCounter;
    private GameObject standingPlayer;
    private GameObject ballPlayer;

    [Header("Player Shoot")]
    [SerializeField] private ArrowController arrowController;
    private Transform transformArrowPoint;
    [SerializeField] private GameObject bomb;

    [Header("Player Dust")]
    [SerializeField] private GameObject dustMovement;
    private Transform transformDustPoint;
    private bool canDoubleJump, isIdle;

    [Header("Player Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    private float dashCounter;
    [SerializeField] private float dashCoolDownTime;
    private float dashCoolDownTimeCounter;

    [Header("Player After Image")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private SpriteRenderer afterImageSprite;
    [SerializeField] private Color afterImageColor;
    [SerializeField] private float afterImageLifeTime;
    [SerializeField] private float afterImageTimeBetween;
    private float afterImageTimeCounter;

    //Player Extras Tracker
    [SerializeField] private PlayerExtrasTracker playerExtrasTracker;


    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        transformArrowPoint = GameObject.Find("ArrowPoint").GetComponent<Transform>();
        transformPlayer = GetComponent<Transform>();
        playerExtrasTracker = GetComponent<PlayerExtrasTracker>();
    }

    private void Start()
    {
        standingPlayer = GameObject.Find("StandingPlayer");
        ballPlayer = GameObject.Find("BallMode");
        ballPlayer.SetActive(false);
        transformDustPoint = GameObject.Find("DustPoint").GetComponent<Transform>();
        checkGroundPoint = GameObject.Find("CheckGroundPoint").GetComponent<Transform>();
        animatorStandingPlayer = standingPlayer.GetComponent<Animator>();
        animatorBallPlayer = ballPlayer.GetComponent<Animator>();
        IdSpeed = Animator.StringToHash("speed");
        IdIsGrounded = Animator.StringToHash("isGrounded");
        IdShootArrow = Animator.StringToHash("shootArrow");
        IdCanDoubleJump = Animator.StringToHash("canDoubleJump");
    }

    void Update()
    {
        Move();
        Dash();
        Jump();
        Shoot();
        BallMode();
    }

    private void Move()
    {
        float inputX = Input.GetAxisRaw("Horizontal") * moveSpeed;

        playerRB.velocity = new Vector2(inputX, playerRB.velocity.y);

        if ((playerRB.velocity.x < 0 && isIdle || playerRB.velocity.x > 0 && isIdle) && isGrounded)
        {
            isIdle = false;
            Instantiate(dustMovement, transformDustPoint.position, Quaternion.identity);
        }
        if (playerRB.velocity.x == 0)
        {
            isIdle = true;
        }
        FlipDirection();
        SetMoveAnimation();
    }

    private void SetMoveAnimation()
    {
        if (standingPlayer.activeSelf)
            animatorStandingPlayer.SetFloat(IdSpeed, MathF.Abs(playerRB.velocity.x));
        if (ballPlayer.activeSelf)
            animatorBallPlayer.SetFloat(IdSpeed, MathF.Abs(playerRB.velocity.x));
    }    

    private void Jump()
    {
        isGrounded = Physics2D.Raycast(checkGroundPoint.position, Vector2.down, 0.2f, selectedLayerMask);

        if (isGrounded)
        {
            canDoubleJump = true;
        }

        if ((Input.GetButtonDown("Jump") && (isGrounded || (canDoubleJump && playerExtrasTracker.CanDoubleJump))))
        {
            if (isGrounded)
                Instantiate(dustMovement,transformDustPoint.position, Quaternion.identity);
            else
            {
                canDoubleJump = false;
                SetDoubleJumpAnimation();
            }
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
        }

        SetJumpAnimation();
    }

    private void SetJumpAnimation()
    {
        if (standingPlayer.activeSelf)
            animatorStandingPlayer.SetBool(IdIsGrounded, isGrounded);
        else if (ballPlayer.activeSelf)
            animatorBallPlayer.SetBool(IdIsGrounded, isGrounded);
    }

    private void SetDoubleJumpAnimation()
    {
        if (standingPlayer.activeSelf)
            animatorStandingPlayer.SetTrigger(IdCanDoubleJump);
        else if (ballPlayer.activeSelf)
            animatorBallPlayer.SetTrigger(IdCanDoubleJump);
    }
    
    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && standingPlayer.activeSelf)
        {
            Instantiate(arrowController, transformArrowPoint.position, transformArrowPoint.rotation);
            animatorStandingPlayer.SetTrigger(IdShootArrow);
        }
        if ((Input.GetButtonDown("Fire1") && ballPlayer.activeSelf) && playerExtrasTracker.CanDropBomb)
            Instantiate(bomb, transformPlayer.position, Quaternion.identity);
    }

    private void FlipDirection()
    {
        if (playerRB.velocity.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (playerRB.velocity.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transformPlayer.Rotate(0f, 180f, 0f);
    }

    private void Dash()
    {
        if (dashCoolDownTimeCounter > 0)
            dashCoolDownTimeCounter -= Time.deltaTime;
        else
        {
            if ((Input.GetButtonDown("Fire2") && standingPlayer.activeSelf) && playerExtrasTracker.CanDash)
                dashCounter = dashTime;
        }
        
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            
            if (isFacingRight)
                playerRB.velocity = new Vector2(dashSpeed * transformPlayer.localScale.x, playerRB.velocity.y);
            else
                playerRB.velocity = new Vector2(dashSpeed * (transformPlayer.localScale.x * -1), playerRB.velocity.y);

            afterImageTimeCounter -= Time.deltaTime;

            if (afterImageTimeCounter <= 0)
            {
                ShowAfterImage();
                FlipDirection();
            }
            dashCoolDownTimeCounter = dashCoolDownTime;
        }
    }

    private void ShowAfterImage()
    {
        SpriteRenderer afterImage = Instantiate(afterImageSprite, transformPlayer.position, transformPlayer.rotation);

        afterImage.sprite = playerSprite.sprite;
        
        afterImage.transform.rotation = playerSprite.transform.rotation;

        afterImage.color = afterImageColor;

        Destroy(afterImage.gameObject, afterImageLifeTime);

        afterImageTimeCounter = afterImageTimeBetween;
    }

    private void BallMode()
    {
        float inputY = Input.GetAxisRaw("Vertical");
        if ((inputY <= -.9 && !ballPlayer.activeSelf || inputY >= .9 && ballPlayer.activeSelf) && playerExtrasTracker.CanEnterBallMode)
        {
            ballModeTimeCounter -= Time.deltaTime;

            if (ballModeTimeCounter < 0)
            {
                standingPlayer.SetActive(!standingPlayer.activeSelf);
                ballPlayer.SetActive(!ballPlayer.activeSelf);
            }
        }
        else
            ballModeTimeCounter = ballModeWaitingTime;
    }
}
