using Runtime.Enums;
using Runtime.Extensions;
using UnityEngine;

public class CursorManager : SingletonMonoBehaviour<CursorManager>
{
    public Texture2D bombCursor;
    public Texture2D unlockCursor;
    public Texture2D shuffleCursor;

    public void SetCursor(UtilityType utilityType)
    {
        switch (utilityType)
        {
            case UtilityType.Bomb:
                Cursor.SetCursor(bombCursor, Vector2.zero, CursorMode.Auto);
                break;
            case UtilityType.Unlock:
                Cursor.SetCursor(unlockCursor, Vector2.zero, CursorMode.Auto);
                break;
            // case UtilityType.Shuffle:
            //     Cursor.SetCursor(shuffleCursor, Vector2.zero, CursorMode.Auto);
            //     break;
        }
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}