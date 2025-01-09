using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 targetPosition;
    private float speed;

    public void Initialize(Vector3 targetPos, float bulletSpeed)
    {
        targetPosition = targetPos;
        speed = bulletSpeed;
    }

    public void MoveBullet()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public bool HasReachedTarget()
    {
        Debug.Log("Target Locate Player Now: "+targetPosition);
        return Vector3.Distance(transform.position, targetPosition) < 0.1f;
    }
}
