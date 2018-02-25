using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImYellowFish.Utility;

/// <summary>
/// Handles collision info
/// </summary>
public class BreastCollision : BreastComponent {
    #region Config
    public static readonly string TAG_BREAST = "Breast";
    public static readonly string TAG_PLAYER = "Player";
    public static readonly string LAYER_BREAST = "Breast";
    public static readonly string LAYER_PLAYER = "Player";
    public static readonly int MAX_COUNT_COLLISION = 4;

    // TODO: move this to character stat
    public float enableCollisionMinDistance = 0f;
    public float collisionCooldownDuration = 1f;
    public float validCollideRange = 0.8f;
    public float collideSizeScale = 1.05f;
    #endregion

    [Header("Readonly")]
    public Collider breastCollider;
    public float breastColliderSize;

    public CooldownFlag collidePlayerCooldown = new CooldownFlag();
    public CooldownFlag collideBreastCooldown = new CooldownFlag();

    private int layerMaskPlayer;
    private int layerMaskBreast;
    private RaycastHit[] hitResults = new RaycastHit[MAX_COUNT_COLLISION];

    public override void Init(Breast host)
    {
        base.Init(host);

        breastCollider = host.tr_breastBody.GetComponent<Collider>();
        breastColliderSize = ((SphereCollider) breastCollider).radius;
        layerMaskPlayer = 1 << LayerMask.NameToLayer(LAYER_PLAYER);
        layerMaskBreast = 1 << LayerMask.NameToLayer(LAYER_BREAST);
        
        // Ignores the collision between player and breast
        Physics.IgnoreCollision(breastCollider, 
            host.tr_player.GetComponent<Collider>());
    }

    private void FixedUpdate()
    {
        collidePlayerCooldown.Update(Time.fixedDeltaTime);
        collideBreastCooldown.Update(Time.fixedDeltaTime);

        CheckForCollision();
    }

    public Vector3 d_dir;
    public Vector3 d_bodyPos;
    public float d_size;
    public float d_dist;

    private void CheckForCollision()
    {
        Vector3 dir = host.tr_breastOrigin.position - host.tr_breastBody.position;
        float dist = dir.magnitude;
        dir.Normalize();

        if (dist < enableCollisionMinDistance)
        {
            return;
        }

        int count = 0;
        // Check collision with player
        if (!collidePlayerCooldown.value)
        {
            count = Physics.SphereCastNonAlloc(host.tr_breastBody.position, breastColliderSize * collideSizeScale * host.tr_breastBody.localScale.x, dir, hitResults, dist * validCollideRange, layerMaskPlayer, QueryTriggerInteraction.Collide);
            for (int i = 0; i < count; i++)
            {
                OnCollidePlayer(hitResults[i]);
            }
        }

        if (!collideBreastCooldown.value)
        {
            // Check collision with breast
            count = Physics.SphereCastNonAlloc(host.tr_breastBody.position, breastColliderSize * collideSizeScale * host.tr_breastBody.localScale.x, dir, hitResults, dist * validCollideRange, layerMaskBreast, QueryTriggerInteraction.Collide);
            for (int i = 0; i < count; i++)
            {
                OnCollideBreast(hitResults[i]);
            }
        }

        d_dir = dir;
        d_size = breastColliderSize * collideSizeScale * host.tr_breastBody.localScale.x;
        d_bodyPos = host.tr_breastBody.position;
        d_dist = dist * validCollideRange;
    }

    private void OnCollidePlayer(RaycastHit hit)
    {
        var target = hit.collider.GetComponent<Character>();
        if (target == null || target == host.character)
        {
            return;
        }

        Debug.Log("Collide player.");
        target.TakeDamage(new Damage()
        {
            damage = 0,
            target = target,
            source = host.character,
            force = Vector3.zero,
        });

        collidePlayerCooldown.Activate(collisionCooldownDuration);
    }

    private void OnCollideBreast(RaycastHit hit)
    {
        var targetBreastCollision = hit.collider.GetComponent<BreastCollision>();
        if (targetBreastCollision == null || targetBreastCollision == this)
            return;

        Debug.Log("Collide breast.");

        collideBreastCooldown.Activate(collisionCooldownDuration);
    }

    //private void OnCollisionEnter(Collision col)
    //{
    //    if (col.gameObject.CompareTag(TAG_BREAST))
    //    {
    //        // On collide breast
    //        Debug.Log("Collide breast");
    //    }
    //    else if (col.gameObject.CompareTag(TAG_PLAYER))
    //    {
    //        // TODO: dispatch event
    //        // TODO: calc damage & force in util class
    //        Debug.Log("Collide player.");

    //        // On collide player
    //        var target = col.gameObject.GetComponent<Character>();
    //        target.TakeDamage(new Damage()
    //        {
    //            damage = 0,
    //            target = target,
    //            source = host.character,
    //            force = Vector3.zero,
    //        });
    //    }
    //}

    //private void OnTriggerEnter(Collider col)
    //{
    //    // TODO: dispatch event if required.
    //    Debug.Log("On trigger enter breast");
    //}
}
