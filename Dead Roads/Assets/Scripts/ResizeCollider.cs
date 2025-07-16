using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class ResizeCollider : MonoBehaviour
{
    private SpriteRenderer _sr;
    private BoxCollider2D _bc;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _bc = GetComponent<BoxCollider2D>();
        UpdateCollider();
    }

    void OnValidate()
    {
        // called when you change values in the Inspector
        if (_sr == null || _bc == null)
        {
            _sr = GetComponent<SpriteRenderer>();
            _bc = GetComponent<BoxCollider2D>();
        }
        UpdateCollider();
    }

    void LateUpdate()
    {
        // in case you resize at runtime
        UpdateCollider();
    }

    private void UpdateCollider()
    {
        if (_sr.drawMode == SpriteDrawMode.Sliced || _sr.drawMode == SpriteDrawMode.Tiled)
        {
            // uses the SpriteRenderer.size (in world units) for sliced/tiled sprites
            _bc.size = _sr.size;
        }
        else
        {
            // fallback: use the sprite’s bounds (in world units) if not sliced/tiled
            _bc.size = _sr.sprite.bounds.size;
        }

        // keep the offset centered
        _bc.offset = Vector2.zero;
    }
}
