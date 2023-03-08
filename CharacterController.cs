using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    new Rigidbody rigidbody;
    [SerializeField] private float thrust = 20f;
    private bool grounded = false;
    public float speed;
    public float gravityModifier;
    public float speedModifier = 0.6f;

    public float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    public float jumpBufferTime = 0.2f;
    public float jumpBufferCounter;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        //realistischere Fallgeschwindigkeit
        Physics.gravity *= gravityModifier;
    }

    //Ermittelt, ob sich der Spieler auf dem Boden befindet
    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }

    void Update()
    {

        //Fällt der Spieler und erreicht die Position -15 auf der y-Achse, spawnt er wieder am Startpunkt
        if(transform.position.y < -15)
        {
            transform.position = new Vector3(0, 5, 0);
        }

        //Drückt der Spieler Leertaste, kurz bevor er sich wieder auf dem Boden befindet, springt er trotzdem
        if (Input.GetKeyDown("space"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        
        //Drückt der Spieler Leertaste, kurz nachdem er den Boden verlässt, kann er trotzdem noch springen
        if (grounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rigidbody.AddForce(transform.up * thrust);

            jumpBufferCounter = 0f;
        }
        if (Input.GetKeyUp("space"))
        {
            coyoteTimeCounter = 0f;
        }

        //Ist der Spieler in der Luft, verlangsamt sich die Bewegungsgeschwindigkeit
        if (!grounded)
        {
            HandleJumpSpeed();
        }
        else
        {
            HandleMovement();
        }
    }

    void HandleJumpSpeed()
    {
        
        float xDirection2 = Input.GetAxis("Horizontal") * speedModifier;
        float zDirection2 = Input.GetAxis("Vertical")* speedModifier;

        Vector3 jumpDirection = new Vector3(xDirection2, 0.0f, zDirection2);
        transform.position += jumpDirection * speed *Time.deltaTime;
    }

    void HandleMovement()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0.0f, zDirection);
        transform.position += moveDirection.normalized * speed * Time.deltaTime;
    }
}
