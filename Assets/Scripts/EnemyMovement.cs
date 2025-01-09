using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public List<BoxCollider> playerCollider;
    private bool isPlayerDetected;
    private BoxCollider player;
    public float speed = 2f;

    void Update()
    {
        if (player != null)
        {
            Debug.Log("I Am Enemy And I See You");
            transform.LookAt(player.transform);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Detected");
            playerCollider.Add(other.GetComponent<BoxCollider>());
            foreach (BoxCollider playerColl in playerCollider)
            {
                player = playerColl;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Exit");
            playerCollider.Remove(other.GetComponent<BoxCollider>());
            if (playerCollider.Count == 0)
            {
                player = null;
            }
        }
    }
}
