using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickImage : MonoBehaviour
{
    // Start is called before the first frame update
    public RawImage img;
    public Sprite sprte;
    public RectTransform panel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PickImg(int maxSize)
    {
        img.rectTransform.anchoredPosition=new Vector2(0,0);

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                /*sprte = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                img.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width,texture.height), new Vector2(0.5f, 0.5f), 100.0f);*/
                img.texture = texture;
                Debug.Log(texture.width);
                Debug.Log(texture.height);
                float w = texture.width;
                float h = texture.height;
                float panelWidth = panel.rect.width;
                float panelHeight = panel.rect.height;
                Debug.Log(panelWidth);
                Debug.Log(panelHeight);
                if(w>h)
                {
                    h= panelWidth / w * h;
                    w= panelWidth;
                    w = panelHeight / h * w;
                    h = panelHeight;

                }
                else if(h>=w)
                {
                    w= w* panelHeight / h;
                    h = panelHeight;
                    h = panelWidth / w * h;
                    w = panelWidth;
                }
                img.rectTransform.sizeDelta = new Vector2(w,h);

                /*texture.LoadImage(img);*/
                // Assign texture to a temporary quad and destroy it after 5 seconds
                /*GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                quad.transform.forward = Camera.main.transform.forward;
                quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                Material material = quad.GetComponent<Renderer>().material;
                if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                    material.shader = Shader.Find("Legacy Shaders/Diffuse");

                material.mainTexture = texture;

                Destroy(quad, 5f);

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                Destroy(texture, 5f);
                */
            }
        });

        Debug.Log("Permission result: " + permission);
    }
}
