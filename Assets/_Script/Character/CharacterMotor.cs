using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : CharacterComponent {
    public bool isFacingRight;
    private Rigidbody rb;

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }

    public void AddAcceleration(Vector3 accele)
    {
        rb.AddForce(accele * rb.mass);
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;

        var scale = character.ch_transform.localScale;
        scale.x *= -1;
        character.ch_transform.localScale = scale;
    }
}
