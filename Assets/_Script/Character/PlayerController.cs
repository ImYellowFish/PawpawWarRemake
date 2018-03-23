using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterComponent {
    public override void Init(Character ch)
    {
        base.Init(ch);

    }

    private float horizontalAxis;
    private void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        // TODO: do this in fixedUpdate?
        character.motor.Move(horizontalAxis);
        character.breast.helpThruster.currentHelpAxis = horizontalAxis;

        // TODO: Mario like jump
        if (Input.GetKeyDown(KeyCode.Space) && character.motor.isGrounded)
        {
            character.motor.AddAcceleration(character.stat.jumpAcceleration * Vector3.up);
        }
    }
}
