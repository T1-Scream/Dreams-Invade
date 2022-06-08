using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuantumTek.EncryptedSave;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float Speed = 2f;
    public float standSpeed = 2f;
    public float crouchSpeed = 1f;

    [Header("Slope")]
    public float slopeForce = 5f;
    public float rayLength = 2f;

    [Header("Jump")]
    public float jumpForce = 3f;
    public float fallMultiplier = 1f;

    [Header("Crouch")]
    [SerializeField] private float crouchHeight = 0.3f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float timeToCrouch = 0.25f;

    [Header("Function Optional")]
    public bool canCrouch;
    public bool canJump;

    private Animator playerAni;
    private Rigidbody body;
    private CapsuleCollider cc;
    private KeyCode keyInputForward;
    private KeyCode keyInputBackward;
    private KeyCode keyInputLeft;
    private KeyCode keyInputRight;
    private KeyCode keyInputCrouch;
    private KeyCode keyInputJump;
    private Vector3 localForward;
    private Vector3 localBackward;
    private Vector3 localLeft;
    private Vector3 localRight;
    private Vector3 moveVectorForward;
    private Vector3 moveVectorBackward;
    private Vector3 moveVectorLeft;
    private Vector3 moveVectorRight;

    private Vector3 slopeMove;
    private RaycastHit hit;
    private Camera camera;
    private bool crouching;
    private bool jump;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * .9f);
    }

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        camera = GetComponentInChildren<Camera>();

        keyInputForward = (KeyCode)System.Enum.Parse(typeof(KeyCode), ES_Save.Load<string>("masterKeyForward"));
        keyInputBackward = (KeyCode)System.Enum.Parse(typeof(KeyCode), ES_Save.Load<string>("masterKeyBackward"));
        keyInputLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), ES_Save.Load<string>("masterKeyLeft"));
        keyInputRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), ES_Save.Load<string>("masterKeyRight"));
        keyInputCrouch = (KeyCode)System.Enum.Parse(typeof(KeyCode), ES_Save.Load<string>("masterKeyCrouch"));
        keyInputJump = (KeyCode)System.Enum.Parse(typeof(KeyCode), ES_Save.Load<string>("masterKeyJump"));

        if (playerAni != null)
        playerAni = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyInputCrouch) && canCrouch)
            StartCoroutine(CrouchStand());

        if (Input.GetKeyDown(keyInputJump))
            jump = true;
    }

    private void FixedUpdate()
    {
        MovementOrigin();

        float x = 0f;
        float z = 0f;
        body.velocity = new Vector3(0, body.velocity.y, 0);

        if (Input.GetKey(keyInputForward)) {
            z = +1f;
            body.velocity = new Vector3(moveVectorForward.x, body.velocity.y, moveVectorForward.z);
        }
        if (Input.GetKey(keyInputBackward)) {
            z = -1f;
            body.velocity = new Vector3(moveVectorBackward.x, body.velocity.y, moveVectorBackward.z);
        }
        if (Input.GetKey(keyInputLeft)) {
            x = -1f;
            body.velocity = new Vector3(moveVectorLeft.x, body.velocity.y, moveVectorLeft.z);
        }
        if (Input.GetKey(keyInputRight)) {
            x = +1f;
            body.velocity = new Vector3(moveVectorRight.x, body.velocity.y, moveVectorRight.z);
        }

        if (canJump) {
            if (IsGrounded() && jump) {
                body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jump = !jump;
            }

            if (body.velocity.y < 0)
                body.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        slopeMove = Vector3.ProjectOnPlane(transform.TransformDirection(x, 0f, z), hit.normal);
        if (OnSlope())
            body.MovePosition(transform.position + Time.deltaTime * slopeForce * slopeMove.normalized);
        else
            body.MovePosition(transform.position + Time.deltaTime * Speed * transform.TransformDirection(x, body.velocity.y, z).normalized);
    }

    private Vector3 MoveDirection(Vector3 direction)
    {
        Vector3 worldPos = direction - transform.position;
        return worldPos;
    }

    private void MovementOrigin()
    {
        localForward = transform.TransformPoint(Vector3.forward * Speed);
        localBackward = transform.TransformPoint(Vector3.back * Speed);
        localLeft = transform.TransformPoint(Vector3.left * Speed);
        localRight = transform.TransformPoint(Vector3.right * Speed);
        moveVectorForward = MoveDirection(localForward);
        moveVectorBackward = MoveDirection(localBackward);
        moveVectorLeft = MoveDirection(localLeft);
        moveVectorRight = MoveDirection(localRight);
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, cc.height / 2 * rayLength))
            if (hit.normal != Vector3.up)
                return true;

        return false;
    }

    private bool IsGrounded()
    {
        if (Physics.Raycast(transform.position + new Vector3(.1f, 0, 0), Vector3.down, out hit, .9f))
            if (hit.collider.tag == "Ground")
                return true;

        return false;
    }

    private IEnumerator CrouchStand()
    {
        if (crouching && Physics.Raycast(transform.position, Vector3.up, out hit, .9f))
            if (hit.collider.tag == "Top")
            yield break;

        float timeElapsed = 0;
        float targetHeight = crouching ? standingHeight : crouchHeight;
        float targetSpeed = crouching ? standSpeed :crouchSpeed;
        float currecntHeight = cc.height;

        while (timeElapsed < timeToCrouch) {
            timeElapsed += 0.5f;
            cc.height = Mathf.Lerp(currecntHeight, targetHeight, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        Speed = targetSpeed;
        cc.height = targetHeight;

        crouching = !crouching;
    }
}
