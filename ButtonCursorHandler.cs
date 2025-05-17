using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Default Cursor Settings")]
    public Texture2D defaultCursor; // Texture for the default cursor (e.g., 32x32 pixels)
    public Vector2 defaultCursorHotspot = new Vector2(17, 11); // Hotspot for the default cursor

    [Header("Pointing Hand Cursor Settings")]
    public Texture2D pointingHandCursor; // Texture for the pointing hand cursor (e.g., 32x32 pixels)
    public Vector2 pointingHandHotspot = new Vector2(20, 9); // Hotspot for the pointing hand cursor

    // Singleton instance for easy access
    public static CursorManager Instance { get; private set; }

    private bool isHovering; // Tracks if the cursor is currently hovering over an interactable element

    private void Awake()
    {
        // Set up the singleton instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep the CursorManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Set the default cursor when the game starts
        SetDefaultCursor();
    }

    public void SetPointingHandCursor()
    {
        if (pointingHandCursor != null)
        {
            Cursor.SetCursor(pointingHandCursor, pointingHandHotspot, CursorMode.Auto);
            isHovering = true;
            Debug.Log($"Set pointing hand cursor with hotspot: {pointingHandHotspot}, Texture: {pointingHandCursor.name}, Size: {pointingHandCursor.width}x{pointingHandCursor.height}");
        }
    }

    public void SetDefaultCursor()
    {
        if (defaultCursor != null)
        {
            Cursor.SetCursor(defaultCursor, defaultCursorHotspot, CursorMode.Auto);
            isHovering = false;
            Debug.Log($"Set default cursor with hotspot: {defaultCursorHotspot}, Texture: {defaultCursor.name}, Size: {defaultCursor.width}x{defaultCursor.height}");
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            isHovering = false;
            Debug.Log("Set system default cursor");
        }
    }

    public bool IsHovering()
    {
        return isHovering;
    }
}