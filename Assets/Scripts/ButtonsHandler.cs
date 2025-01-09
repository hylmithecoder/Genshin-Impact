using UnityEngine;
using System.Collections;

public class ButtonsHandler : MonoBehaviour 
{
    [Header("Buttons References")]
    private bool isHolding = false;
    public float holdTime = 1f; // Waktu hold
    protected float holdTimer = 0f;

    public UnityEngine.Events.UnityEvent onHoldComplete; // Event saat hold selesai
    public UnityEngine.Events.UnityEvent onHoldCancel;   // Event saat hold dibatalkan

    // Dipanggil oleh EventTrigger saat PointerDown
    public void StartHold()
    {
        isHolding = true;
        holdTimer = 0f; // Reset timer

        StartCoroutine(HoldCoroutine());
    }

    // Dipanggil oleh EventTrigger saat PointerUp
    public void StopHold()
    {
        isHolding = false; // Stop hold jika pointer dilepas
    }

    private IEnumerator HoldCoroutine()
    {
        while (isHolding)
        {
            holdTimer += Time.deltaTime;
            Debug.Log(holdTimer);
            if (holdTimer >= holdTime)
            {
                isHolding = false; // Hentikan hold setelah selesai
                Debug.Log("Hold Completed!");
                onHoldComplete.Invoke(); // Panggil event untuk hold selesai
                yield break; // Keluar dari coroutine
            }

            yield return null; // Tunggu frame berikutnya
        }

        Debug.Log("Tap Detected!");
        onHoldCancel.Invoke(); // Panggil event untuk tap
    }
}