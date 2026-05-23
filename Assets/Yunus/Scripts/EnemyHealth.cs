using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("HP")]
    public int startHP = 1;
    public int currentHP;

    [Header("Sprites by HP (index = HP)")]
    // Size should be maxHP+1. We'll use 0..5 (0 is unused or can be empty).
    public Sprite[] spriteByHP; // spriteByHP[1]=red, [2]=blue, [3]=green, [4]=yellow, [5]=pink

    [Header("Speed Multiplier by HP (index = HP)")]
    public List<float> speedMultByHP = new(); // element 1 = 1.0, 2=1.25, 3=1.5, 4=1.75, 5=2.0


    private SpriteRenderer sr;
    EnemyFollowPath mover;

    public static int AliveCount { get; private set; }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetAliveCount() => AliveCount = 0;


    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        mover = GetComponent<EnemyFollowPath>();
    }

    void Start()
    {
        currentHP = Mathf.Clamp(startHP, 1, spriteByHP.Length - 1);
        UpdateSprite();
    }

    void OnEnable() { AliveCount++; }
    void OnDisable() { AliveCount--; }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            Die();
            return;
        }

        UpdateSprite();
    }

    void UpdateSprite()
    {
        // Clamp to valid range just in case
        int hp = Mathf.Clamp(currentHP, 1, spriteByHP.Length - 1);
        sr.sprite = spriteByHP[hp];
        UpdateSpeed();
    }

    void UpdateSpeed()
    {
        if (mover == null) return;

        // If you didn't set the list in Inspector, just keep 1x speed
        if (speedMultByHP == null || speedMultByHP.Count == 0)
        {
            mover.SetSpeedMultiplier(1f);
            return;
        }

        // Clamp HP to list range
        int hp = Mathf.Clamp(currentHP, 1, speedMultByHP.Count - 1);

        float mult = speedMultByHP[hp];
        mover.SetSpeedMultiplier(mult);
    }


    void Die()
    {
        Destroy(gameObject);
    }
}
