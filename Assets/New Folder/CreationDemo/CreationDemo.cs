using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.Video;

public class CreationDemo : MonoBehaviour
{
	public RawImage croppedImageHolder;
	public float minAspectRatio, maxAspectRatio;
	public GameObject TrimUI;
	public GameObject videoPlayerButton;
	public RangeSlider rangeSlider;
	public VideoPlayer videoPlayer;
    private Texture2D thumbnail;
    private string videoPath;
	private string videoURL;
	private long startPoint, endPoint;
    private float fps;
    private float nfs;
    private double dur;
    bool thumbnailOk;
    //public Image i1, i2, i3, i4, i5, i6, i7, i8, i9;
    public RawImage[] img = new RawImage[1];
    

    private void Start()
    {
		videoPlayerButton.GetComponent<Button>().onClick.AddListener(PlayVideoPlayer);
		TrimUI.SetActive(false);
	}

    public void PickVideo()
	{
        if (NativeGallery.IsMediaPickerBusy())
            return;
        GetVideo();
	}

	private void GetVideo()
    {
		NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
		{
			Debug.Log("Video Path: " + path);
			if (path != null)
			{
				Texture2D texture2D = NativeGallery.GetVideoThumbnail(videoPath: path, maxSize: -1, captureTimeInSeconds: -1.0, markTextureNonReadable: false);
				NativeGallery.VideoProperties videoProperties = NativeGallery.GetVideoProperties(videoPath: path);
				Debug.Log("Video Properties duration: " + videoProperties.duration);
                nfs = videoProperties.duration * fps;
                Debug.Log("nfs:" + nfs);
                Debug.Log("Video Properties rotation: " + videoProperties.rotation);
				Debug.Log("Video Properties height: " + videoProperties.height);
				Debug.Log("Video Properties width: " + videoProperties.width);
                //videoPlayer.renderMode = VideoRenderMode.APIOnly;
               /* videoPlayer.sendFrameReadyEvents =false;
                int c=0;
                //videoPlayer.frameReady += FrameReady;
                videoPlayer.frame = 0;
                img[0].texture = videoPlayer.texture;
                */
                videoPlayer.Prepare();
                videoPath = path;
				PrepareVideo(path);
			}
		}, "Select a video", "video/*");
	}
    void FrameReady(VideoPlayer vp, long frameIndex)
    {
        Debug.Log("FrameReady " + frameIndex);
        //var textureToCopy =         // Perform texture copy here ...
        if (frameIndex == 0)
        {
            Debug.Log("thumbnail added");
           // img[0].texture = textureToCopy;
        }
        //vp.frame = frameIndex + 30;
    }
    /*private Texture2D Get2DTexture()
    {
        thumbnail = new Texture2D(img[0].texture.width, img[0].texture.height, TextureFormat.RGBA32, false);
        RenderTexture cTexture = RenderTexture.active;
        RenderTexture rTexture = new RenderTexture(img[0].texture.width, img[0].texture.height, 32);
        UnityEngine.Graphics.Blit(img[0].texture, rTexture);

        RenderTexture.active = rTexture;
        thumbnail.ReadPixels(new Rect(0, 0, rTexture.width, rTexture.height), 0, 0);
        thumbnail.Apply();

        UnityEngine.Color[] pixels = thumbnail.GetPixels();

        RenderTexture.active = cTexture;

        rTexture.Release();

        return thumbnail;
    }*/
    private void PrepareVideo(string path)
    {
		videoURL = path;
		videoPlayer.url = videoURL;
        img[0].texture = videoPlayer.texture;
		videoPlayer.prepareCompleted += OnVideoPrepared;
        img[0].texture = videoPlayer.texture;
        videoPlayer.Prepare();
        img[0].texture = videoPlayer.texture;
    }

	private void OnVideoPrepared(VideoPlayer vp)
	{
		rangeSlider.MinValue = 0;
		rangeSlider.MaxValue = vp.frameCount;
		rangeSlider.HighValue = vp.frameCount;
		TrimUI.SetActive(true);

        /*for(int i=1;i<=9;i++)
        {
            Texture2D t = (Texture2D)videoPlayer.texture;
            img[i-1].sprite= Sprite.Create(t, new Rect(0.0f, 0.0f, t.width, t.height), new Vector2(0.5f, 0.5f), 100.0f);
            videoPlayer.frame += (int)JumpRate;
            Debug.Log("frameNo:"+videoPlayer.frame);
        }*/
        /*i1.sprite = img[0].sprite;
        i2.sprite = img[1].sprite;
        i3.sprite = img[2].sprite;*/
        //img[0].texture = videoPlayer.texture;
        img[0].texture = vp.texture;
        videoPlayer.prepareCompleted -= OnVideoPrepared;
        img[0].texture = videoPlayer.texture;
    }

	public void TrimVideo(Single start, Single end)
    {
		StopVideoPlayer();
		startPoint = (long)start;
		endPoint = (long)end;
		//PlayVideoPlayer();
	}

	private void Update()
    {
        if(videoPlayer.isPlaying)
        {
			if (videoPlayer.frame == endPoint)
            {
				Debug.Log("Endpoint Reached!");
				StopVideoPlayer();
			}
        }
    }

	private void PlayVideoPlayer()
    {
		videoPlayer.url = videoURL;
		videoPlayer.frame = startPoint;
		videoPlayer.Play();
		videoPlayer.frame = startPoint;
		//TODO: Update UI
		//videoPlayerButton.SetActive(false);
	}

	private void StopVideoPlayer()
    {
		videoPlayer.Stop();
		//TODO: Update UI
		videoPlayerButton.SetActive(true);
	}
}
