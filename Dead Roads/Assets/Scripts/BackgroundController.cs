using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    // for clone‑guarding
    public bool isClone = false;

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

        // only the “real” (non‑clone) tile spawns the above/below copies
        if (!isClone)
            CreateVerticalClones();
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

    private void CreateVerticalClones()
    {
        // above
        GameObject top = Instantiate(
            gameObject,
            transform.position + Vector3.up * lengthY,
            transform.rotation,
            transform.parent
        );
        top.GetComponent<BackgroundController>().isClone = true;

        // below
        GameObject bot = Instantiate(
            gameObject,
            transform.position + Vector3.down * lengthY,
            transform.rotation,
            transform.parent
        );
        bot.GetComponent<BackgroundController>().isClone = true;
    }
}
