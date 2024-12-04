using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using SKCell;

public class PlayerController : MonoBehaviour
{
    public float horizontal = 0f;
    public float acceleration = 5f;
    public float decceleration = 200f;
    public float max_hspeed = 100f;
    public float max_hspeed_dash = 150f;
    public float speed = 2f;
    public float jump_power = 16f;
    public float current_speed_right = 0f;
    public float current_speed_left = 0f;
    public float wall_jumping_power = 10f;
    public float air_speed = 10f;


    //Double Tap Dash
    private float DashDirection = 0f;
    private float lastPressTimeLeft = 0f;
    private float lastPressTimeRight = 0f;
    private const float DOUBLE_CLICK_TIME = .3f;
    private float DashCoolDown = 0f;

    private float dashingspeed = 0f;
    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 45f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    public float dashingCooldownRef = 1f;
    [SerializeField] private TrailRenderer tr;


    [SerializeField] private bool isFacingRight = true;

    [SerializeField] private float vspeed = 0;
    //triple Jump

    private float jumpmomemtum = 0f;

    public bool doublejump = false;
    public bool triplejump = false;
    private float jump_reset_timer = 0.01f;
    public float jump_reset = 0.5f;
    public bool start_counting = false;


    private float jump_reset_timer_triple = 0.01f;
    public float jump_reset_triple = 0.5f;
    public bool start_counting_triple = false;


    //Detect previous key input
    private bool input_right = false;
    private bool input_left = false;


    private float jump_counter = 0f;



    private float max_hspeed_running = 1.3f;
    private float accerlation_running = 80f;
    public float current_running_speed = 1f;

    public float wall_jump_delay = 0.5f;
    public float wall_jump_delay_countdown = 0f;

    private bool isLeavingWall = false;
    private bool iswallsliding;
    private float wallSlidingSpeed = 4f;
    private float wallSliding_MaxHspseed = 3f;
    private bool momemtumreset = false;


    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.1f;
    private float wallJumpingAcceleration = 1f;

    private Vector2 wallJumpingPower = new Vector2(6f, 20f);
    private Vector2 jump_velocity = new Vector2(0f, 0f);

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundcheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform wallCheck_left;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private GameObject wallCheckOBJ;
    //Animations
    public Animator animator;


    //Check if flipping
    private bool isflipping = false;


    //reference to grappling gun
    /*
    public GrapplingGun grapplingGun;
    private float grappletimer = 0f;
    */


    //invinsible Time
    private float invisibletime = 0f;
    public bool isinvisible = false;


    [SerializeField] private SpriteRenderer spriteRenderer;

    //dialogues
    public bool istalking = false;

    //Cinemachine 
    private CinemachineImpulseSource impluseSrouce;
    //private void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    // }
    [Header("Feedbacks")]
    /// a feedback to call when moving
    [Tooltip("damage taken")]
    public MMFeedbacks chromatic;
    [Tooltip("jump")]
    public MMFeedbacks jumpFeedBack;
    [Tooltip("dash")]
    public MMFeedbacks dashFeedBack;
    public MMFeedbacks dashReadyFeedBack;
    //Visual Effects
    [SerializeField] ParticleSystem jumpvfx;
    [SerializeField] ParticleSystem jumpvfx2;
    [SerializeField] ParticleSystem damagevfx;
    [SerializeField] ParticleSystem walljumpvfx;

    //Visual Indicate
    //public SKSlider DashSliderLeft;
    //public SKSlider DashSliderRight;

    private void Start()
    {
        impluseSrouce = GetComponent<CinemachineImpulseSource>();
        wall_jump_delay_countdown = wall_jump_delay;
    }

    void Update()
    {
        UpdateDashSlider();
        if (istalking)
        {
            current_speed_left = 0f;
            current_speed_right = 0f;
            animator.SetFloat("Speed", 0);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsWallSlide", false);
            return;

        }

        if (isinvisible)
        {
            invisibletime += Time.deltaTime;
            if (invisibletime >= 0.5f)
            {
                isinvisible = false;
                invisibletime = 0f;
                SetCollisionWithLayer(8, true);
                StopCoroutine(BlinkSprite()); // Stop blinking when no longer invisible
                SetSpriteAlpha(1f);

            }
            else
            {
                StartCoroutine(BlinkSprite());
                SetCollisionWithLayer(8, false);
            }
        }

        dashingCooldownRef += Time.deltaTime;
        //if (IsGrounded() || IsWalled() || IsWalled_Left() || iswallsliding)
        //{


        //        grapplingGun.Grapenabled = false;
        //        grapplingGun.startcounting = false;

        // }



        if (isWallJumping)
        {
            return;
        }



        if (!isWallJumping)
        {
            jump_velocity.x = Mathf.MoveTowards(jump_velocity.x, 0, decceleration * Time.deltaTime);
        }

        if (isDashing)
        {
            if ((Input.GetKey(KeyCode.D)) && (!Input.GetKey(KeyCode.A)))
            {
                current_speed_right += acceleration * Time.deltaTime;

            }

            if ((Input.GetKey(KeyCode.A)) && (!Input.GetKey(KeyCode.D)))
            {
                current_speed_left += acceleration * Time.deltaTime;

            }

        }


        if (IsGrounded())
        {
            jumpmomemtum = Mathf.MoveTowards(jumpmomemtum, 0, decceleration * Time.deltaTime);
        }


        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -max_hspeed_dash, max_hspeed_dash), rb.velocity.y);
        current_speed_right = Mathf.Clamp(current_speed_right, 0, max_hspeed);
        current_speed_left = Mathf.Clamp(current_speed_left, 0, max_hspeed);


        current_running_speed = Mathf.Clamp(current_running_speed, 0, max_hspeed_running);


        if (!isDashing)
        {

            //change gravity back
            float originalGravity = rb.gravityScale;
            rb.gravityScale = originalGravity;

            dashingspeed = Mathf.MoveTowards(dashingspeed, 0, decceleration * Time.deltaTime * 2);

            if ((Input.GetKey(KeyCode.D)) && (!Input.GetKey(KeyCode.A)) && !iswallsliding )
            {
                current_speed_right += acceleration * Time.deltaTime;
                rb.velocity = new Vector2(dashingspeed + current_speed_right * current_running_speed + jump_velocity.x + jumpmomemtum / 4, rb.velocity.y);

            }
            else
            {
                current_speed_right = Mathf.MoveTowards(current_speed_right, 0, decceleration * Time.deltaTime);
            }

            if ((Input.GetKey(KeyCode.A)) && (!Input.GetKey(KeyCode.D)) && !iswallsliding)
            {
                current_speed_left += acceleration * Time.deltaTime;
                rb.velocity = new Vector2(dashingspeed + -current_speed_left * current_running_speed + jump_velocity.x + jumpmomemtum / 4, rb.velocity.y);
            }
            else
            {
                current_speed_left = Mathf.MoveTowards(current_speed_left, 0, decceleration * Time.deltaTime);
            }
        }
        //Wall Stuff
        if(iswallsliding)
        {
            // Allow movement to leave the wall
            if (Input.GetKey(KeyCode.A) && IsWalled() && isFacingRight)
            {
                rb.velocity = new Vector2(transform.localScale.x * -wall_jumping_power, rb.velocity.y); // Move off the wall to the right
                Debug.Log("Go off wall left");
                isLeavingWall = true;
                //iswallsliding = false; // Exit wall sliding state
            }
            else if (Input.GetKey(KeyCode.D) && IsWalled() && !isFacingRight)
            {
                rb.velocity = new Vector2(transform.localScale.x * -wall_jumping_power, rb.velocity.y); // Move off the wall to the left
                Debug.Log("Go off wall right");
                isLeavingWall = true;
                //iswallsliding = false; // Exit wall sliding state
            }

        }
        else
        {
            isLeavingWall = false;
        }


        //Dashing
        DashCoolDown += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.A) && DashCoolDown >= 1f)
        {
            float timeSinceLastPress = Time.time - lastPressTimeLeft;
            if (timeSinceLastPress <= DOUBLE_CLICK_TIME)
            {
                //Double pressed
                if (!iswallsliding)
                {
                    DashDirection = -1f;
                    DashCoolDown = 0f;
                    dashFeedBack?.PlayFeedbacks();
                    StartCoroutine(Dash());
                    isinvisible = true;
                }

            }
            else
            {
                //Not Double


            }


            lastPressTimeLeft = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.D) && DashCoolDown >= 1f)
        {
            float timeSinceLastPress = Time.time - lastPressTimeRight;
            if (timeSinceLastPress <= DOUBLE_CLICK_TIME)
            {
                if (!iswallsliding)
                {
                    isinvisible = true;
                    DashDirection = 1f;
                    DashCoolDown = 0f;
                    dashFeedBack?.PlayFeedbacks();
                    StartCoroutine(Dash());
                }
            }
            else
            {
                //Not Double


            }


            lastPressTimeRight = Time.time;
        }



        if (Input.GetKey(KeyCode.LeftShift) && !isFacingRight && DashCoolDown >= 1f)
        {
            if (!iswallsliding)
            {
                isinvisible = true;
                DashDirection = -1f;
                DashCoolDown = 0f;
                dashFeedBack?.PlayFeedbacks();
                StartCoroutine(Dash());
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift) && isFacingRight && DashCoolDown >= 1f)
        {
            if (!iswallsliding)
            {
                isinvisible = true;
                DashDirection = 1f;
                DashCoolDown = 0f;
                dashFeedBack?.PlayFeedbacks();
                StartCoroutine(Dash());
            }
        }


        current_running_speed = 1.1f;
        horizontal = rb.velocity.x;

        Wallslide();
        WallJump();
        if (!isWallJumping)
        {

        }




        animator.SetFloat("Speed", (current_speed_left + current_speed_right));



        if (!isWallJumping)
        {
            Flip();
            Jump();
        }

        vspeed = rb.velocity.y;
        animator.SetFloat("VerticalSpeed", vspeed);
        if (!IsGrounded() && !iswallsliding)
        {
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }
        if (iswallsliding)
        {
            animator.SetBool("IsWallSlide", true);
        }
        else
        {
            animator.SetBool("IsWallSlide", false);
        }
    }


    public void talking()
    {
        current_speed_left = 0;
        current_speed_right = 0;
        istalking = true;
    }


    public void Stoptalking()
    {
        istalking = false;
    }

    private void Jump()
    {

        if (Input.GetButtonDown("Jump") && IsGrounded() && !iswallsliding)
        {
            //Disabled VFX
            //jumpvfx.Play();
            //jumpvfx2.Play();
            //jumpFeedBack?.PlayFeedbacks();
            rb.velocity = new Vector2(rb.velocity.x, jump_power);
            jumpmomemtum = rb.velocity.x;
            start_counting = true;
            jump_reset = jump_reset_timer;


        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f && !iswallsliding)
        {
            jumpmomemtum = rb.velocity.x;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetButtonDown("Jump") && !IsGrounded() && !iswallsliding && !isWallJumping && !doublejump && (jump_reset <= 0))
        {
            //Disabled VFX
            //jumpFeedBack?.PlayFeedbacks();
            //jumpvfx.Play();
            //jumpvfx2.Play();
            jumpmomemtum = rb.velocity.x;
            doublejump = true;
            rb.velocity = new Vector2(rb.velocity.x, jump_power);
            start_counting_triple = true;
            jump_reset_triple = jump_reset_timer_triple;

        }
        if (Input.GetButtonDown("Jump") && !IsGrounded() && !iswallsliding && !triplejump && !isWallJumping && (jump_reset_triple <= 0))
        {
            //Disabled VFX
            //jumpFeedBack?.PlayFeedbacks();
            //jumpvfx.Play();
            //jumpvfx2.Play();
            jumpmomemtum = rb.velocity.x;
            triplejump = true;
            rb.velocity = new Vector2(rb.velocity.x, jump_power);

        }


    }


    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundcheck.position, 0.3f, groundLayer);

    }

    private bool IsWalled()
    {
        Collider2D collider = wallCheck.GetComponent<Collider2D>();
        if (collider != null)
        {
            return collider.IsTouchingLayers(groundLayer);
        }
        return false;
    }

    private bool IsWalled_Left()
    {
        //return Physics2D.OverlapCircle(wallCheck_left.position, 0f, groundLayer);
        Collider2D collider = wallCheck.GetComponent<Collider2D>();
        if (collider != null)
        {
            return collider.IsTouchingLayers(groundLayer);
        }
        return false;
    }


    private void Wallslide()
    {
        if ((IsWalled() || IsWalled_Left()) && !IsGrounded() && !isWallJumping && !isLeavingWall)
        {
            iswallsliding = true;
            Debug.Log("wallsliding");
            Debug.Log(iswallsliding);
            // Limit vertical speed for wall sliding
            rb.velocity = new Vector2(0, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));



        }
        else
        {
            iswallsliding = false;
            momemtumreset = false;
        }

        /*
           if ((IsWalled() && !IsGrounded() && !Input.GetButtonDown("Jump") && !IsWalled_Left()))
           {
               iswallsliding = true;

               if (iswallsliding && !momemtumreset)
               {
                   current_speed_left = 0;
                   current_speed_right = 0;
                   momemtumreset = true;
               }

               rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
               //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

           }
           else if ((IsWalled_Left() && !IsGrounded() && !Input.GetButtonDown("Jump") && !IsWalled()))
           {
               iswallsliding = true;

               if (iswallsliding && !momemtumreset)
               {
                   current_speed_left = 0;
                   current_speed_right = 0;
                   momemtumreset = true;
               }

               rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
           }
           else
           {
               iswallsliding = false;
               momemtumreset = false;
           }
         */

    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }


        if (start_counting == true)
        {
            jump_reset -= Time.deltaTime;
        }
        if (jump_reset <= 0)
        {
            start_counting = false;
        }

        if (start_counting_triple == true)
        {
            jump_reset_triple -= Time.deltaTime;
        }
        if (jump_reset_triple <= 0)
        {
            start_counting_triple = false;
        }


        if (IsGrounded())
        {
            doublejump = false;
            triplejump = false;
        }
        else if (isWallJumping)
        {
            doublejump = false;
            triplejump = false;
        }




    }

    private void WallJump()
    {
        if (iswallsliding)
        {
            //Debug.Log("Hello");
            //iswallsliding = false;
            wallJumpingCounter = wallJumpingTime;

            //CancelInvoke(nameof(StopWallJumping));

        }
        else
        {

            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {

            //wall_jumping_power = 20f;
            isWallJumping = true;
            wallJumpingAcceleration += acceleration * Time.deltaTime;
            if (IsWalled())
            {
                //Disabled FX
                //jumpFeedBack?.PlayFeedbacks();
                //walljumpvfx.Play();
                Debug.Log("Walljump");
                //rb.AddForce(new Vector2(-40f,jump_power),ForceMode2D.Impulse);
                jump_velocity = new Vector2(transform.localScale.x * -wall_jumping_power, jump_power);
                rb.velocity = new Vector2(jump_velocity.x, jump_velocity.y);
            }




            Invoke(nameof(StopWallJumping), wallJumpingDuration);

            wallJumpingCounter = 0f;
            /*
            if(transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
           */

        }
        else if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
    }


    private void StopWallJumping()
    {
        wallJumpingAcceleration = 1f;
        isWallJumping = false;
    }


    private void Flip()
    {
        if (iswallsliding) return;
        if (((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f)) && !isflipping)
        {


            StartCoroutine(FlipOverTime(0.1f));
            isflipping = true;


        }

    }

    private IEnumerator FlipOverTime(float duration)
    {
        isFacingRight = !isFacingRight;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = new Vector3(-startScale.x, startScale.y, startScale.z);

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            // Optionally, add easing here
            // normalizedTime = Mathf.SmoothStep(0f, 1f, normalizedTime);
            transform.localScale = Vector3.Lerp(startScale, endScale, normalizedTime);
            yield return null;
        }

        // Ensure the localScale is set to the endScale when the loop is done
        isflipping = false;
        transform.localScale = endScale;
    }


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(DashDirection * dashingPower * current_running_speed, 0f);
        dashingspeed = rb.velocity.x;
        tr.emitting = true;
        spriteRenderer.color = new UnityEngine.Color(0, 0, 0, 1);
        Debug.Log($"IsGrounded: {IsGrounded()}");
        if (!IsGrounded())
        {
            Debug.Log("Triggering Air Dash Animation");
            animator.SetBool("IsDashingAir", true);
        }
        else
        {
            Debug.Log("Triggering Ground Dash Animation");
            animator.SetBool("IsDashing", true);
        }

        yield return new WaitForSeconds(dashingTime);
        animator.SetBool("IsDashingAir", false);
        animator.SetBool("IsDashing", false);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        StartCoroutine(SmoothTransitionToMaxSpeed());
        dashingCooldownRef = 0f;
        spriteRenderer.color = new UnityEngine.Color(1, 1, 1, 1);
        yield return new WaitForSeconds(dashingCooldown);
        dashReadyFeedBack?.PlayFeedbacks();
        canDash = true;

    }

    private IEnumerator SmoothTransitionToMaxSpeed()
    {
        float timeToTransition = 0.5f; // Duration of the transition
        float elapsedTime = 0;

        while (elapsedTime < timeToTransition)
        {
            current_speed_left = Mathf.Lerp(current_speed_left, max_hspeed, (elapsedTime / timeToTransition));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        current_speed_left = max_hspeed;
    }

    private void SetCollisionWithLayer(int layer, bool state)
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, layer, !state);
    }

    private IEnumerator BlinkSprite()
    {
        float blinkDuration = 0.1f; // Duration of each blink
        float minAlpha = 0.01f; // Minimum alpha value
        float maxAlpha = 1f; // Maximum alpha value

        while (isinvisible)
        {
            SetSpriteAlpha(minAlpha); // Make sprite more transparent
            yield return new WaitForSeconds(blinkDuration);
            SetSpriteAlpha(maxAlpha); // Reset to full opacity
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    private void SetSpriteAlpha(float alpha)
    {
        UnityEngine.Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    /*
    public void TakeDamage()
    {
        damagevfx.Play();
        chromatic?.PlayFeedbacks();
        CameraShakeManager.instance.CameraShake(impluseSrouce);
    }
    */


    private void UpdateDashSlider()
    {
        // Calculate the decimal percentage of bloodCount relative to maxBlood
        float DashCoolDownRef = (float) DashCoolDown / dashingCooldown;
        //DashSliderLeft.SetValue(DashCoolDownRef);
        //DashSliderRight.SetValue(DashCoolDownRef);
    }
}
