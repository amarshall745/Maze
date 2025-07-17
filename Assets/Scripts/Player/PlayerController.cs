using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;


    public float moveSpeed, gravityModifier, jumpPower, runSpeed = 12f;
    public CharacterController charCon;

    private Vector3 moveInput;

    public Transform camTrans;

    public float mouseSensitivity, buttonDelay = 2f;
    public bool invertX;
    public bool invertY;

    private bool canJump, canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    public Animator anim;

    private bool canPressButton;

    [Header("Raycast")]
    public Transform raycastPoint;
    public float raycast10 = 10f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        canPressButton = false;
    }

    void Update()
    {
        //Movement
        float yStore = moveInput.y;
        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horiMMove = transform.right * Input.GetAxis("Horizontal");
        moveInput = horiMMove + vertMove;
        moveInput.Normalize();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveInput = moveInput * runSpeed;
        }
        else
        {
            moveInput = moveInput * moveSpeed;
        }
        moveInput.y = yStore;
        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;
        charCon.Move(moveInput * Time.deltaTime);
        anim.SetFloat("moveSpeed", moveInput.magnitude);
        anim.SetBool("onGround", canJump);


        //Jumping
        if (charCon.isGrounded)
        {
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }
        canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGround).Length > 0;
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            moveInput.y = jumpPower;
            canDoubleJump = true;
        }
        else if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            moveInput.y = jumpPower;
            canDoubleJump = false;
        }

        //Mouse movement
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        if (invertX) mouseInput.x = -mouseInput.x;
        if (invertY) mouseInput.y = -mouseInput.y;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));


        //Raycast
        RayCast();





    }

    public void RayCast()
    {
        Ray ray = new Ray(raycastPoint.position, raycastPoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {

            if (hit.distance <= raycast10)
            {
                Debug.Log("Less than 10");
                if (hit.collider.CompareTag("Pickup"))
                {
                    Debug.Log("Pickup");
                }
            }

        }
    }

}
