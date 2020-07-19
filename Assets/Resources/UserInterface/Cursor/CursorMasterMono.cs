using UnityEngine;

public class CursorMasterMono : MonoBehaviour
{
    public Texture2D RepairCursor;
    public Texture2D DismantleCursor;
    public Texture2D DefaultCursor;

    public void UseRepair() => Cursor.SetCursor(RepairCursor, Vector2.zero, CursorMode.Auto);
    public void UseDefault() => Cursor.SetCursor(DefaultCursor, Vector2.zero, CursorMode.Auto);
    public void UseDismantle() => Cursor.SetCursor(DismantleCursor, Vector2.zero, CursorMode.Auto);
}
