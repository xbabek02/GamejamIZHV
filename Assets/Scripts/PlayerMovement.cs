using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float walkSpeed = 4f;
    
    
    public float flightSpeed = 5f;
    public float flightMaxSpeed = 10f;
    public float deltaSpeed = 0.8f;

    public Transform cam;
    public bool inTheAir;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private bool IsStinging;
    private bool IsSpining;
    public bool IsAttacking { get; private set; }

    private float spinDelay = 1.2f;
    private float currentSpinTime = 0f;


    private float stingDelayGround = 0.8f;
    private float stingDelayAir = 3f;
    private float currentStingTime = 0f;

    private bool takeOff;

    private float gravity = -3.5f;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        IsAttacking = (IsStinging || IsSpining) ? true : false;

        //aply gravity on the ground
        if (!inTheAir && !IsSpining)
        {
            controller.Move(new Vector3(0f, gravity * Time.deltaTime, 0f));
        }

        //blockades
        if (IsStinging)
        {
            currentStingTime += Time.deltaTime;
            if (currentStingTime >= (inTheAir ? stingDelayAir : stingDelayGround))
            {
                IsStinging = false;
                currentStingTime = 0f;
            }
            return;
        }

        if (IsSpining)
        {
            currentSpinTime += Time.deltaTime;
            if (currentSpinTime >= spinDelay)
            {
                IsSpining = false;
                currentSpinTime = 0f;
            }
            return;
        }

        //actions
        if (Input.GetKeyDown(KeyCode.F))
        {
            Sting();
            return;
        }

        else if (Input.GetKeyDown(KeyCode.R) && !inTheAir)
        {
            Spin();
            return;
        }

        //movements
        if (inTheAir)
        {
            AdjustSpeed();
            MovementInTheAir();

            if (!takeOff && (controller.collisionFlags & CollisionFlags.Below) != 0)
            {
                inTheAir = false;
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
                animator.SetBool("InTheAir", false);
            }
        }

        else
        {
            MovementOnTheGround();

            if (Input.GetKeyDown(KeyCode.Space) && !inTheAir)
            {
                inTheAir = true;
                animator.SetBool("InTheAir", true);
                takeOff = true;
                StartCoroutine(DelayLanding());
            }
        }
    }

    private void MovementOnTheGround()
    {
        //movement on the ground

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * walkSpeed * Time.deltaTime);
        }

        //animation on the ground
        animator.SetFloat("Speed", direction.magnitude * walkSpeed);
    }

    private void MovementInTheAir()
    {
        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");
        
        Vector3 direction = new Vector3(0f, 0f, 1f).normalized;

        float targetAngleY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float targetAngleX = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.x;

        //float angleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref turnSmoothVelocity, turnSmoothTime);
        //float angleX = Mathf.SmoothDampAngle(transform.eulerAngles.x, targetAngleX, ref turnSmoothVelocity, turnSmoothTime);
        
        transform.rotation = Quaternion.Euler(targetAngleX, targetAngleY, 0f);

        Vector3 moveDir = Quaternion.Euler(targetAngleX, targetAngleY, 0f) * Vector3.forward;
        controller.Move(moveDir * flightSpeed * Time.deltaTime);
    }

    private void AdjustSpeed()
    {
        if (Input.mouseScrollDelta.y < 0f)
        {
            flightSpeed -= deltaSpeed;
            if (flightSpeed < 0)
            {
                flightSpeed = 0;
            }
        }
        else if (Input.mouseScrollDelta.y > 0f)
        {
            flightSpeed += deltaSpeed;
            if (flightSpeed > flightMaxSpeed)
            {
                flightSpeed= flightMaxSpeed;
            }
        }
    }

    private void Sting()
    {
        animator.SetTrigger("Stinger");
        IsStinging = true;
    }

    private void Spin()
    {
        animator.SetTrigger("SpinAttack");
        IsSpining = true;
        StartCoroutine(SpinMovement());
    }

    private IEnumerator SpinMovement()
    {
        float targetAngle = Mathf.Atan2(0, 1) * Mathf.Rad2Deg + cam.eulerAngles.y;

        Vector3 moveDirForward = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        Vector3 moveDirUp = moveDirForward + new Vector3(0f, 0.2f, 0f);
        Vector3 moveDirDown = moveDirForward + new Vector3(0f, -0.2f, 0f);
        
        while (IsSpining)
        {
            if (currentSpinTime < spinDelay / 2)
            {
                controller.Move(3 * Time.deltaTime * moveDirUp);
            }
            else
            {
                controller.Move(3 * Time.deltaTime * moveDirDown);
            }
            
            yield return null;
        }
        yield return null;
    }

    IEnumerator DelayLanding()
    {
        yield return new WaitForSeconds(2.5f);
        takeOff = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (inTheAir && collision.gameObject.layer == 6)
        {
           
            inTheAir = false;
            transform.rotation = Quaternion.Euler(0f, transform.rotation.y, 0f);
        }
    }
}
