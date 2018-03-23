using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImYellowFish.Utility;

/// <summary>
/// Controls the force and movement of the character.
/// </summary>
public class CharacterMotor : CharacterComponent {
    public bool isFacingRight;
    private Rigidbody rb;

    // ground check
    // TODO: more robust handling
    public bool isGrounded;
    private Transform groundCheck;
    private static readonly float groundCheckDist = 0.1f;
    private static readonly string groundLayer = "Ground";

    // Assumes for now only left and right move patterns
    // should there be more patterns or modes, we should abstract them into move modes class,
    // and take care of their interaction.
    private float maxHorizontalSpeed;

    [SerializeField]
    private Vector3 currentExternForce;
    public float currentMoveInputAxis;


    public override void Init(Character ch)
    {
        base.Init(ch);

        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("Cannot find rb component on: " + gameObject.name);

        groundCheck = transform.Find("GroundCheck");
        if (groundCheck == null)
            Debug.LogError("Cannot find ground check obj: " + gameObject.name);

        RevertMaxHorizontalSpeed();
    }

    public void AddForce(Vector3 force)
    {
        currentExternForce += force;
    }

    public void AddAcceleration(Vector3 accele)
    {
        currentExternForce += accele * rb.mass;
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;

        var scale = character.ch_transform.localScale;
        scale.x *= -1;
        character.ch_transform.localScale = scale;
    }

    /// <summary>
    /// Normal move command. Called during Update.
    /// </summary>
    /// <param name="axis"></param>
    public void Move(float axis)
    {
        AddForce(axis * character.stat.moveAcceleration * Vector3.right);
        currentMoveInputAxis = axis;
    }

    public void SetMaxHorizontalSpeed(float value)
    {
        maxHorizontalSpeed = value;
    }

    public void RevertMaxHorizontalSpeed()
    {
        maxHorizontalSpeed = character.stat.maxHorizontalSpeed;
    }

    private void Update()
    {
        if(groundCheck != null)
        {
            int layer = LayerMask.NameToLayer(groundLayer);
            isGrounded = Physics.Linecast(groundCheck.position, 
                groundCheck.position + Vector3.down * groundCheckDist, 1 << layer);
        }
    }

    private void FixedUpdate()
    {
        // apply and reset the force.
        rb.AddForce(currentExternForce);
        currentExternForce = Vector3.zero;
        currentMoveInputAxis = 0;

        var v = rb.velocity;
        v.x = Helper.ClampAbs(v.x, maxHorizontalSpeed);
        rb.velocity = v;
    }
}
