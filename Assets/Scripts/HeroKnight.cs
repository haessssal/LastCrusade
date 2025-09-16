using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class HeroKnight : MonoBehaviour 
{
    [SerializeField] float speed = 4.0f;
    [SerializeField] float jumpForce = 7.5f;
    [SerializeField] private float hp;
    [SerializeField] private float mpSpeed;
    [SerializeField] private float attackSpeed;

    private Animator animator;
    private Rigidbody2D body2d;
    private Sensor_HeroKnight groundSensor;
    private Sensor_HeroKnight wallSensorR1;
    private Sensor_HeroKnight wallSensorR2;
    private Sensor_HeroKnight wallSensorL1;
    private Sensor_HeroKnight wallSensorL2;
    private bool isWallSliding = false;
    private bool grounded = false;
    private int facingDirection = 1;
    private int currentAttack = 0;
    private float timeSinceAttack = 0.0f;
    private float delayToIdle = 0.0f;

    private HeroControls heroControls;
    private float inputX;

    public void SetCharacterStats(CharacterData data)
    {
        hp = data.hp;
        mpSpeed = data.mpSpeed;
        speed = data.moveSpeed;
        attackSpeed = data.attackSpeed;
        Debug.Log($"character stat: hp {hp} / mp {mpSpeed} / move {speed} / attack {attackSpeed}");
    }
    
    void Start ()
    {
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();

        heroControls = new HeroControls();
        heroControls.Player.Enable();
        
        // Move 값 저장
        heroControls.Player.Move.performed += ctx => inputX = ctx.ReadValue<Vector2>().x;
        heroControls.Player.Move.canceled += ctx => inputX = 0f;

        // Jump와 Attack 함수 연결
        heroControls.Player.Jump.performed += ctx => Jump();
        heroControls.Player.Attack.performed += ctx => Attack();
    }
    
    void Update ()
    {
        timeSinceAttack += Time.deltaTime;

        if (!grounded && groundSensor.State())
        {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }

        if (grounded && !groundSensor.State())
        {
            grounded = false;
            animator.SetBool("Grounded", grounded);
        }

        // -- Handle movement --
        float inputX = this.inputX;

        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            facingDirection = 1;
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            facingDirection = -1;
        }

        body2d.linearVelocity = new Vector2(inputX * speed, body2d.linearVelocity.y);
        animator.SetFloat("AirSpeedY", body2d.linearVelocity.y);

        // -- Handle Animations --
        isWallSliding = (wallSensorR1.State() && wallSensorR2.State()) || (wallSensorL1.State() && wallSensorL2.State());
        animator.SetBool("WallSlide", isWallSliding);
            
        if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            delayToIdle = 0.05f;
            animator.SetInteger("AnimState", 1);
        }
        else
        {
            delayToIdle -= Time.deltaTime;
            if(delayToIdle < 0)
                animator.SetInteger("AnimState", 0);
        }
    }

    private void Jump()
    {
        if (grounded)
        {
            animator.SetTrigger("Jump");
            grounded = false;
            animator.SetBool("Grounded", grounded);
            body2d.linearVelocity = new Vector2(body2d.linearVelocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }
    }

    private void Attack()
    {
        if(timeSinceAttack > 0.25f)
        {
            currentAttack++;

            if (currentAttack > 3)
                currentAttack = 1;

            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            animator.SetTrigger("Attack" + currentAttack);
            timeSinceAttack = 0.0f;
        }
    }

    private void Hurt()
    {
        animator.SetTrigger("Hurt");
    }

    private void Death()
    {
        animator.SetTrigger("Death");
    }

    void OnDisable()
    {
        heroControls.Player.Disable();
    }
}