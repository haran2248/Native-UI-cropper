using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System;

public class PickVideo : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoPlayer VideoPlayback;
    public string link;
    public RawImage thumbnailVid;
    private Texture2D thumbnail;
    bool thumbnailOk;
    public int vidHeight;
    public int vidWidth;
    //public Bitmap mp;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void pickVideo()
    {
        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
        {
            Debug.Log("Video path: " + path);
            if (path != null)
            {
                // Play the selected video
                Handheld.PlayFullScreenMovie("file://" + path);
                link = "file://" + path;
                Debug.Log(link);
            }
        }, "Select a video");
        
        Debug.Log("Permission result: " + permission);
        VideoPlayback.url =link;
        VideoPlayback.Stop();
        VideoPlayback.renderMode = VideoRenderMode.APIOnly;
        VideoPlayback.sendFrameReadyEvents = true;
        VideoPlayback.frameReady += FrameReady;
        VideoPlayback.Prepare();
        StartCoroutine(PrepareVideo());
    }
    void FrameReady(VideoPlayer vp, long frameIndex)
    {
        Debug.Log("FrameReady " + frameIndex);
        VideoPlayback.Pause();
        thumbnailVid.texture = vp.texture;
        thumbnailVid.texture = Get2DTexture();

        VideoPlayback.sendFrameReadyEvents = false; //To stop frameReady events
        vp = null;

        thumbnailOk = true;
    }

    IEnumerator PrepareVideo()
    {
        yield return new WaitUntil(() => VideoPlayback.isPrepared);

        Debug.Log("Video PlayBack");
        VideoPlayback.Play();

        vidWidth = Convert.ToInt32(VideoPlayback.width);
        vidHeight = Convert.ToInt32(VideoPlayback.height);

        VideoPlayback.isLooping = true;
        VideoPlayback.renderMode = VideoRenderMode.MaterialOverride;

        Debug.Log("Video height & width: " + vidWidth + ", " + vidHeight);

        yield return new WaitUntil(() => thumbnailOk);

        VideoPlayback.Play();

        GC.Collect();
    }

    private Texture2D Get2DTexture()
    {
        thumbnail = new Texture2D(thumbnailVid.texture.width, thumbnailVid.texture.height, TextureFormat.RGBA32, false);
        RenderTexture cTexture = RenderTexture.active;
        RenderTexture rTexture = new RenderTexture(thumbnailVid.texture.width, thumbnailVid.texture.height, 32);
        UnityEngine.Graphics.Blit(thumbnailVid.texture, rTexture);

        RenderTexture.active = rTexture;
        thumbnail.ReadPixels(new Rect(0, 0, rTexture.width, rTexture.height), 0, 0);
        thumbnail.Apply();

        UnityEngine.Color[] pixels = thumbnail.GetPixels();

        RenderTexture.active = cTexture;

        rTexture.Release();

        return thumbnail;
    }
}
