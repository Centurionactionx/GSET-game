using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPosX, startPosY;
    private float lengthX, lengthY;
    public GameObject cam;
    [Range(0f, 1f)]
    public float parallaxEffect = 0.9f;

    void Start()
    {
        // record initial camera‑relative origin
        startPosX = cam.transform.position.x;
        startPosY = transform.position.y;  // keep the sprite's own Y

        // fetch sprite dimensions
        var bounds = GetComponent<SpriteRenderer>().bounds;
        lengthX = bounds.size.x;
        lengthY = bounds.size.y;
    }

    void FixedUpdate()
    {
        float camX = cam.transform.position.x;

        // compute horizontal parallax offset only
        float offsetX = camX * (parallaxEffect / 7f * 3f);

        // move this tile: X with parallax, Y stays fixed at startPosY
        transform.position = new Vector3(
            startPosX + offsetX,
            startPosY,
            transform.position.z
        );

        // horizontal wrap logic
        float movementX = camX * (1f - parallaxEffect);
        if (movementX > startPosX + lengthX)      startPosX += lengthX;
        else if (movementX < startPosX - lengthX) startPosX -= lengthX;
    }
}
