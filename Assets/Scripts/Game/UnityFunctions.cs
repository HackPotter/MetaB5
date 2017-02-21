using UnityEngine;


public class UnityFunctions : MonoBehaviour
{
    public void SetCursor(Texture2D texture)
    {
        if (texture != null)
        {
            Cursor.SetCursor(texture, new Vector2(texture.width / 2, texture.height / 2), CursorMode.Auto);
        }
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}

