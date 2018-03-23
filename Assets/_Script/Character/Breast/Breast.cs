using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImYellowFish.Utility;

public class Breast : CharacterComponent {
    #region Components
    // References
    [Header("Readonly")]
    public Transform tr_player;
    public Transform tr_breastRoot;
    public Transform tr_breastOrigin;
    public Transform tr_breastBody;
    
    // TODO: move this into physics
    public Rigidbody rb_breast;
    public Rigidbody rb_player;
    public SpringJoint springJoint;

    // Components
    public SlaveComponentContainer<Breast, BreastComponent> slaveContainer = new SlaveComponentContainer<Breast, BreastComponent>();
    public BreastDrawer drawer;
    public BreastConstraint constraint;
    public BreastCollision collision;
    public BreastHelpThruster helpThruster;
    #endregion

    #region Lifecycle
    public override void Init(Character host)
    {
        base.Init(host);

        // setup transforms
        tr_player = transform;
        tr_breastRoot = transform.Find("Breast");
        tr_breastOrigin = tr_breastRoot.Find("Origin");
        tr_breastBody = tr_breastRoot.Find("Body");

        rb_player = tr_player.GetComponent<Rigidbody>();
        rb_breast = tr_breastBody.GetComponent<Rigidbody>();
        
        // setup components
        drawer = slaveContainer.CreateSlaveComponent<BreastDrawer>(this, tr_breastRoot.gameObject);
        constraint = slaveContainer.CreateSlaveComponent<BreastConstraint>(this, tr_breastRoot.gameObject);
        collision = slaveContainer.CreateSlaveComponent<BreastCollision>(this, tr_breastBody.gameObject);
        helpThruster = slaveContainer.AddExistingSlaveComponent<BreastHelpThruster>(this, tr_breastRoot.gameObject);
    }
    #endregion
}
