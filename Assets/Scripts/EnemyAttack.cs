using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] public GameObject bulletPrefab;
    public float speedRate;    
    public float cooldown = 2f;
    private EnemyMovement enemyMove;
    public List<GameObject> currentBullets = new List<GameObject>();
    public BoxCollider currentCollPlayer;

    // Start is called before the first frame update
    void Start()
    {
        enemyMove = GetComponent<EnemyMovement>();
    }

    float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        
        foreach (BoxCollider collPlayer in enemyMove.playerCollider)
        {
            if (collPlayer == null)
            {
                currentCollPlayer = null;
                continue;
            }
            currentCollPlayer = collPlayer;

            if (timer <= 0f)
            {
                ShootBullet();
                timer = cooldown;
            }
        }

        MoveBullets();
    }

    void ShootBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bulletComponent = newBullet.GetComponent<Bullet>();
        if (bulletComponent != null && currentCollPlayer != null)
        {
            bulletComponent.Initialize(currentCollPlayer.transform.position, speedRate);
        }
        currentBullets.Add(newBullet);
        Debug.Log("Bullet shot from " + transform.name + ": " + currentBullets.Count);
    }

    void MoveBullets()
    {
        for (int i = currentBullets.Count - 1; i >= 0; i--)
        {
            GameObject bullet = currentBullets[i];
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            {
                bulletComponent.MoveBullet();

                // Check if the bullet has reached the player
                if (bulletComponent.HasReachedTarget())
                {
                    Destroy(bullet);
                    currentBullets.RemoveAt(i);
                    Debug.Log("Bullet hit the player.");
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