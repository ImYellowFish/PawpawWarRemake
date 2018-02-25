using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImYellowFish.Utility;

[RequireComponent(typeof(CharacterStat))]
public class Character : MonoBehaviour
{
    /// <summary>
    /// If enabled, init will be called on Awake(), for debug use.
    /// </summary>
    public bool debug_initOnAwake;

    #region Components
    public SlaveComponentContainer<Character, CharacterComponent> slaveContainer = new SlaveComponentContainer<Character, CharacterComponent>();

    /// <summary>
    /// Battle related character stats, such as speed, hp etc.
    /// Includes init values and current values.
    /// </summary>
    [Header("SubComponents")]
    [ReadOnly]
    public CharacterStat stat;

    /// <summary>
    /// Controls the movement of the character.
    /// </summary>
    [ReadOnly]
    public CharacterMotor motor;

    /// <summary>
    /// Controls damage & skill
    /// </summary>
    [ReadOnly]
    public Battler battler;

    /// <summary>
    /// Receives player input and invoke actions.
    /// </summary>
    [ReadOnly]
    public PlayerController controller;

    /// <summary>
    /// Manages everything about the breast
    /// </summary>
    [ReadOnly]
    public Breast breast;
    #endregion

    #region Info
    /// <summary>
    /// The relating battleManager
    /// </summary>
    [ReadOnly]
    public BattleManager battleManager;

    /// <summary>
    /// The central transform of the character.
    /// </summary>
    [ReadOnly]
    public Transform ch_transform;

    [ReadOnly]
    public Rigidbody ch_rigidbody;

    /// <summary>
    /// Basic info about the player, such as hero type, name, and playerIndex.
    /// </summary>
    [Header("Readonly")]
    public CharacterInfo info;

    /// <summary>
    /// Which player is controlling this character.
    /// </summary>
    public int playerIndex;
    #endregion

    #region Operations
    public void TakeDamage(Damage damage)
    {
        battler.TakeDamage(damage);
    }
    #endregion

    #region Event system
    /// <summary>
    /// Character events.
    /// </summary>
    public enum Event
    {
        /// <summary>
        /// Character falls back to the ground
        /// </summary>
        ToGround = 20,

        /// <summary>
        /// Character jumps off or gets hit into the air
        /// </summary>
        OffGround = 21,
    }
    /// <summary>
    /// The event dispatcher
    /// </summary>
    public Dispatcher<Event> dispatcher = new Dispatcher<Event>(false, new EventComparer());
    public class EventComparer : IEqualityComparer<Event>
    {
        public bool Equals(Event a, Event b)
        {
            return (int)a == (int)b;
        }

        public int GetHashCode(Event a)
        {
            return (int)a;
        }
    }
    #endregion

    #region Lifecycle
    public void Init(CharacterInfo info, BattleManager battleManager)
    {
        
        this.info = info;
        this.battleManager = battleManager;
        ch_transform = transform;
        ch_rigidbody = GetComponent<Rigidbody>();

        playerIndex = info.playerIndex;

        // Add sub components & init
        stat = AddExistingSubComponent<CharacterStat>();
        motor = CreateSubComponent<CharacterMotor>();
        battler = CreateSubComponent<Battler>();
        breast = CreateSubComponent<Breast>();

        // TODO: use character init param class to do this
        if (info.willCreateController)
            controller = CreateSubComponent<PlayerController>();
    }

    public void CleanUp()
    {
        battler.CleanUp();
        motor.CleanUp();
        stat.CleanUp();
    }

    private void Awake()
    {
        // if debugging,
        // use inspector info to initialize the character
        if (debug_initOnAwake)
            Init(info, null);
    }

    private void OnDestroy()
    {
        if (debug_initOnAwake)
            CleanUp();
    }
    #endregion

    #region Helper
    /// <summary>
    /// Creates and init a component
    /// </summary>
    private T CreateSubComponent<T>() where T : CharacterComponent
    {
        return slaveContainer.CreateSlaveComponent<T>(this);
    }

    /// <summary>
    /// Get and init an existing component 
    /// </summary>
    private T AddExistingSubComponent<T>() where T : CharacterComponent
    {
        return slaveContainer.AddExistingSlaveComponent<T>(this);
    }

    #endregion
}
