using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance {get; private set;}
    [SerializeField] public GameObject bulletPrefab;
    public float SpeedRate = 5f;
    public List<GameObject> currentBullets;
    public Collider currentEnemyCollider;
    private MovementHandler player;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        player = GetComponent<MovementHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Collider collEnemy in player.enemyCollider)
        {
            if (collEnemy.tag != "Enemy" && collEnemy == null)
            {
                currentEnemyCollider = null;
                continue;
            }
            currentEnemyCollider = collEnemy;
        }
        MoveBullets();
    }

    public void ShootBullet()
    {
        Debug.Log("Player Attack With Projectile");
        Vector3 forwardPlayer = new Vector3(transform.localPosition.x + transform.forward.x + 0.5f, transform.localPosition.y + 0.5f, transform.position.z);
        GameObject newBullet = Instantiate(bulletPrefab, forwardPlayer, Quaternion.identity);
        Bullet bulletComponent = newBullet.GetComponent<Bullet>();
        if (bulletComponent != null && currentEnemyCollider != null)
        {
            bulletComponent.Initialize(currentEnemyCollider.transform.position, SpeedRate);
        }
        currentBullets.Add(newBullet);
        Debug.Log("Bullet shot from " + transform.name + ": " + currentBullets.Count);
    }

    private void MoveBullets()
    {
        for (int i = currentBullets.Count - 1; i >= 0; i--)
        {
            GameObject bullet = currentBullets[i];
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            {
                bulletComponent.MoveBullet();
                if (bulletComponent.HasReachedTarget())
                {
                    Destroy(bullet);
                    currentBullets.RemoveAt(i);
                    Debug.Log("Bullet Hit the Enemy");
                }                
            }
            else
            {
                Destroy(bullet);
                currentBullets.RemoveAt(i);
                Debug.Log("Bullet is cleared.");
            }
        }
    }
}
