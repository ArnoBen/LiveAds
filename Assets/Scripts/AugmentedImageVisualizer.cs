using System;
using System.Collections;
using System.Collections.Generic;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AugmentedImageVisualizer : MonoBehaviour
{
    [SerializeField] VideoClip[] videoClips;
    public AugmentedImage Image;
    VideoPlayer videoPlayer;
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnStop;
    }

    private void OnStop(VideoPlayer source)
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Image == null || Image.TrackingState != TrackingState.Tracking)
            return;
        if (!videoPlayer.isPlaying)
        {
            videoPlayer.clip = videoClips[Image.DatabaseIndex];
            videoPlayer.Play();
        }
        transform.localScale = new Vector3(Image.ExtentX, Image.ExtentZ, 1);
    }
}
