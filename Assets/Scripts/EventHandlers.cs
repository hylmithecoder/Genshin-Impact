using UnityEngine;

public class EventHandlers : MonoBehaviour, IIinteract
{
    [Header("Event References")]
    [Tooltip("Object yang akan diinteraksi")]
    public GameObject eventObject;

    public void InteractMethod()
    {
        Debug.Log("This Is A Interact Method");
        foreach (Collider collider in MovementHandler.Instance.nearbyCollider)
        {
            IIinteract interactable = collider.GetComponent<IIinteract>();
            if (interactable != null)
            {
                MovementHandler.Instance.CanMoving = false;
                interactable.Interaksi();
                Debug.Log("Interact With: " + collider.name);
            }
            if (collider.name == "TransferPlayer")
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(MovementHandler.Instance.loadScene);
            }
        }
    }

    public void HoldMethod()
    {
        Debug.Log("This Is A Hold Method");
    }

    void IIinteract.Interaksi()
    {
        throw new System.NotImplementedException();
    }

    public void Jump()
    {
        if (MovementHandler.Instance.CanMoving == false)
        {
            return;
        }
        MovementHandler.Instance.rb.AddForce(Vector3.up * MovementHandler.Instance.jumpForce, ForceMode.Impulse);
    }

    public void HoldJump()
    {
        if (MovementHandler.Instance.CanMoving == false)
        {
            return;
        }
        MovementHandler.Instance.rb.AddForce(Vector3.up * MovementHandler.Instance.jumpForce * 100, ForceMode.Impulse);
    }
}