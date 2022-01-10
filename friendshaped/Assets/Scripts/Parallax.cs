using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;

    private Transform cameraTransform;
    private Vector3 lastCameraPos;

    private float textureUnitSizeX;
    private float textureUnitSizeY;

    [SerializeField] private bool repeatX;
    [SerializeField] private bool repeatY;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPos = cameraTransform.position;

        GetTextureUnitSizeX();
        GetTextureUnitSizeY();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 movement = cameraTransform.position - lastCameraPos;

        transform.position += new Vector3(movement.x * parallaxEffectMultiplier.x,
            movement.y * parallaxEffectMultiplier.y);

        lastCameraPos = cameraTransform.position;

        if (repeatX && Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetX, transform.position.y);
        }

        if (repeatY && Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
        {
            float offsetY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
            transform.position = new Vector3(cameraTransform.position.x, transform.position.y + offsetY);
        }
    }

    private void GetTextureUnitSizeX()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;

        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    private void GetTextureUnitSizeY()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;

        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }
}