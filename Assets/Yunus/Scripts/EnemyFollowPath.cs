using UnityEngine;

public class EnemyFollowPath : MonoBehaviour
{
    public Transform pathParent;
    public float speed = 2f;
    public int baseDamage = 1;
    public int maxHP = 10;
    public int coinReward = 1;
    float speedMultiplier = 1f;

    public void SetSpeedMultiplier(float m)
    {
        speedMultiplier = Mathf.Max(0f, m);
    }

    int hp;
    int index;
    Transform[] pts;

    void Start()
    {
        hp = maxHP;

        pts = new Transform[pathParent.childCount];
        for (int i = 0; i < pts.Length; i++)
            pts[i] = pathParent.GetChild(i);

        transform.position = pts[0].position;
        index = 1;
    }

    void Update()
    {
        if (index >= pts.Length) return;

        Vector3 target = pts[index].position;
        float step = speed * speedMultiplier * Time.deltaTime;

        if (Vector3.Distance(transform.position, target) <= step)
        {
            transform.position = target;
            index++;

            if (index >= pts.Length)
            {
                GameManager.I.DamageBase(baseDamage);
                Destroy(gameObject);
            }
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        GameManager.I.AddCoins(coinReward);
        if (hp <= 0) Destroy(gameObject);
    }
}
