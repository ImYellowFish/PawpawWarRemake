using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImYellowFish.Utility;

public class BreastDraft : CharacterComponent
{
    #region Configs
    public Transform breastRootTransform;
    [Header("Hitbox")]
    public float hitBox_radius;
    public float hitBoxEnableMinOffsetDist = 0.5f;

    public float hitStunRatePerHit = 1f;
    public float hitStunDuration = 3f;

    [Header("Physics")]
    public float hitDisableDuration;
    public float hitForceBreastFactor;
    public float hitForcePushFactor;
    public float hitForceSelfFactor = 0.5f;
    public float hitCooldown;
    public float hitForceBoobInterCollision = 5f;
    public float hitCooldownBoolInterCollision = 1f;

    [Header("Lock mode")]
    public float springLockSpeedToLength = 1f;
    public float springLockMovePenalty = 0f;
    public float springLockMovePenaltyMaxLength = 15f;

    #endregion

    #region Reference components
    [ReadOnly]
    public Rigidbody breast_rigidbody;
    [ReadOnly]
    public SpringJoint spring;
    [ReadOnly]
    public BreastDrawer drawer;
    [ReadOnly]
    public BreastConstraint constraint;

    public Transform playerTransform { get; private set; }
    public Transform tipTransform { get; private set; }

    public Rigidbody breastRigidBody { get; private set; }
    private Rigidbody ch_rigidbody;
    private SpringJoint springJoint;

    #endregion

    private int hurtBoxMask;
    private int boobMask;
    private RaycastHit[] hitResults = new RaycastHit[4];

    private Vector3 springLockPos;

    [ReadOnly]
    public bool springEnabled = true;
    private float hitCooldownTimer;
    private float boobHitCooldownTimer;
    private float initMass;
    private float initSpringk;

    public override void Init(Character ch)
    {
        base.Init(ch);

        drawer = GetComponent<BreastDrawer>();
        constraint = GetComponent<BreastConstraint>();
        ch_rigidbody = character.ch_rigidbody;
        breastRigidBody = GetComponent<Rigidbody>();
        springJoint = GetComponent<SpringJoint>();

        tipTransform = transform;
        playerTransform = tipTransform.parent;

        initMass = breastRigidBody.mass;
        initSpringk = springJoint.spring;

        hurtBoxMask = 1 << LayerMask.NameToLayer("HurtBox");
        boobMask = 1 << LayerMask.NameToLayer("Boob");

        drawer.Init(this);
        // TODO:
        constraint.Init(this);
    }

    void Update()
    {
        if (!springEnabled)
        {
            transform.position = springLockPos;
            float dist = (breastRootTransform.position - springLockPos).magnitude;
            springLockMovePenalty = 1f - Mathf.Clamp01(dist / springLockMovePenaltyMaxLength);
        }
        else
        {
            springLockMovePenalty = 1f;
        }
    }

    void FixedUpdate()
    {
        CheckForCollision();
    }

    private void CheckForCollision()
    {

        if (hitCooldownTimer > 0)
        {
            hitCooldownTimer -= Time.deltaTime;
            if (hitCooldownTimer <= 0)
            {
                ResetBreastMass();
            }
            return;
        }

        // spring is locked in position
        if (!springEnabled)
        {
            breastRigidBody.velocity = Vector3.zero;
            return;
        }


        Vector3 dir = breastRootTransform.position - tipTransform.position;
        dir.z = 0;
        float dist = dir.magnitude;
        dir.Normalize();

        if (dist < hitBoxEnableMinOffsetDist)
        {
            return;
        }

        int count = Physics.SphereCastNonAlloc(tipTransform.position, hitBox_radius, dir, hitResults, dist * 0.8f, hurtBoxMask, QueryTriggerInteraction.Collide);
        for (int i = 0; i < count; i++)
        {
            OnHitPlayer(hitResults[i]);
        }

        if (boobHitCooldownTimer > 0)
        {
            boobHitCooldownTimer -= Time.deltaTime;
            return;
        }


        count = Physics.SphereCastNonAlloc(tipTransform.position, hitBox_radius, dir, hitResults, dist * 0.8f, boobMask, QueryTriggerInteraction.Collide);
        for (int i = 0; i < count; i++)
        {
            OnHitBreast(hitResults[i]);
        }
    }

    private void OnHitPlayer(RaycastHit rayHit)
    {

        Transform hitPlayerTr = rayHit.collider.transform.parent;
        var hitPlayer = hitPlayerTr.GetComponent<Character>();

        // disable self fire
        if (hitPlayerTr == playerTransform)
            return;

        Vector3 selfPlayerPos = breastRootTransform.position;
        Vector3 hitPlayerPos = hitPlayerTr.position;
        Vector3 breastVelocity = breastRigidBody.velocity;

        // TODO:
        Debug.Log("hit player.");
        var forceTarget = breastVelocity;
        var forceSelf = -breastVelocity;

        //var forceTarget = DataManager.Instance.physics.GetBreastPushForceTarget(breastVelocity);
        //var forceSelf = DataManager.Instance.physics.GetBrestPushForceSelf(breastVelocity);

        hitPlayerTr.GetComponent<Rigidbody>().AddForce(forceTarget);
        ch_rigidbody.AddForce(forceSelf);

        hitPlayer.TakeDamage(new Damage()
        {
            force = forceTarget,
            source = character,
            target = hitPlayer,
            damage = 0,
        });
        // hitPlayer.OnGetHit(breastVelocity, this, player);
        SetBreastCooldown();
    }

    public void SetBreastCooldown()
    {
        hitCooldownTimer = hitCooldown;
        springJoint.spring = initSpringk / 50;
        breastRigidBody.mass = initMass / 50;
    }

    public void ResetBreastMass()
    {
        springJoint.spring = initSpringk;
        breastRigidBody.mass = initMass;
    }

    private void OnHitBreast(RaycastHit rayHit)
    {
        Transform hitPlayerTr = rayHit.collider.transform.parent;
        var hitPlayer = hitPlayerTr.GetComponent<Character>();

        if (hitPlayerTr == playerTransform)
            return;


        Vector3 breastVelocity = breastRigidBody.velocity;
        var forceVelocity = breastVelocity * hitForceBreastFactor * hitForceBoobInterCollision;
        hitPlayer.GetComponent<Rigidbody>().AddForce(forceVelocity);

        Vector3 otherBreastVelocity = rayHit.collider.GetComponent<Rigidbody>().velocity;
        var otherForceVelocity = otherBreastVelocity * hitForceBoobInterCollision;
        breastRigidBody.AddForce(otherForceVelocity);

        // TODO:
        Debug.Log("breast collision");

        //character.OnBreastCollision(breastVelocity, otherBreastVelocity, this, hitPlayer);
        //hitPlayer.OnBreastCollision(otherBreastVelocity, breastVelocity, this, player);

        boobHitCooldownTimer = hitCooldownBoolInterCollision;
    }

    public void EnableSpring()
    {
        if (!springEnabled)
        {
            transform.position = springLockPos;

            var dir = breastRootTransform.position - springLockPos;
            dir.z = 0;
            breastRigidBody.velocity = dir * springLockSpeedToLength;

            springEnabled = true;
            breastRigidBody.isKinematic = false;
        }

    }

    public void DisableSpring()
    {
        if (springEnabled)
        {
            springEnabled = false;
            breastRigidBody.isKinematic = true;
            springLockPos = transform.position;
        }
    }

    public void OnPlayerFlip()
    {
        drawer.Update();
    }
}
