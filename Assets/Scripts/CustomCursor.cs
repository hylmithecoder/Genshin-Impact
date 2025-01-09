using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture; // Gambar kursor kustom
    public Vector2 hotSpot = Vector2.zero; // Titik aktif kursor

    void Start()
    {
        SetCustomCursor();
    }

    void SetCustomCursor()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }
}
