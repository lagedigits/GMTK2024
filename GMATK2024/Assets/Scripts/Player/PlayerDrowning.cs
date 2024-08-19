using UnityEngine;

public class PlayerDrowning : MonoBehaviour
{
    [SerializeField] private float drownSpeed = 0.5f; // The speed at which the player sinks
    [SerializeField] private float sinkTime = 2.0f;   // How long it takes to fully sink
    [SerializeField] private float submergeStartThreshold = 0.4f; // Start drowning at 40% submersion
    [SerializeField] private float submergeThreshold = 0.9f; // 90% submersion threshold
    [SerializeField] private LayerMask waterLayer;    // LayerMask to identify the water
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    private Rigidbody2D rb;
    private bool isDrowning = false;
    private float sinkTimer = 0.0f;

    private Collider2D waterCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDrowning)
        {
            if (IsPlayerSubmerged(submergeThreshold))
            {
                SinkPlayer();
            }
        }
    }

    private void SinkPlayer()
    {
        sinkTimer += Time.deltaTime;

        // Gradually increase the downward force applied to the player
        float sinkForce = Mathf.Lerp(0, drownSpeed, sinkTimer / sinkTime);
        rb.velocity = new Vector2(rb.velocity.x, -sinkForce);

        // Optional: Add any other effects, such as fading the player out, etc.
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInWaterLayer(collision.gameObject))
        {
            print("Is In Acid");
            waterCollider = collision.collider;

            // Only start drowning if the player is submerged at least 40%
            if (IsPlayerSubmerged(submergeStartThreshold))
            {
                print("Is Submerged");
                StartDrowning();
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IsInWaterLayer(collision.gameObject))
        {
            StopDrowning();
        }
    }

    private void StartDrowning()
    {
        isDrowning = true;
        rb.gravityScale = 0.5f; // Optionally reduce gravity to simulate buoyancy
        sinkTimer = 0.0f;
    }

    private void StopDrowning()
    {
        isDrowning = false;
        rb.gravityScale = 1.0f; // Restore original gravity
    }

    private bool IsPlayerSubmerged(float threshold)
    {
        // Calculate the top position of the player's sprite
        float playerTop = playerSpriteRenderer.bounds.max.y;

        // Calculate the bottom position of the water
        float waterSurface = waterCollider.bounds.max.y;

        // Check if the player is submerged by the specified threshold
        return playerTop <= waterSurface - (playerSpriteRenderer.bounds.size.y * threshold);
    }

    private bool IsInWaterLayer(GameObject obj)
    {
        // Check if the object's layer matches the water layer
        return ((1 << obj.layer) & waterLayer) != 0;
    }
}