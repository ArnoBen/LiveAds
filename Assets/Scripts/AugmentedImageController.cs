using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;
using System;

public class AugmentedImageController : MonoBehaviour
{
    [SerializeField] AugmentedImageVisualizer augmentedImageVisualizer;
    private Dictionary<int, AugmentedImageVisualizer> visualizers =
        new Dictionary<int, AugmentedImageVisualizer>();

    private List<AugmentedImage> images = new List<AugmentedImage>();

    void Update()
    {
        if (Session.Status != SessionStatus.Tracking)
            return;
        Session.GetTrackables<AugmentedImage>(images, TrackableQueryFilter.Updated);
        VisualizeTrackable();
    }

    private void VisualizeTrackable()
    {
        foreach (var image in images)
        {
            AugmentedImageVisualizer visualizer = null;
            visualizers.TryGetValue(image.DatabaseIndex, out visualizer);

            if (image.TrackingState == TrackingState.Tracking && visualizer == null)
            {
                Anchor anchor = image.CreateAnchor(image.CenterPose);
                visualizer = Instantiate(augmentedImageVisualizer, anchor.transform);
                visualizer.Image = image;
                visualizers.Add(image.DatabaseIndex, visualizer);
            }
            else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
            {
                visualizers.Remove(image.DatabaseIndex);
                Destroy(visualizer.gameObject);
            }
        }
    }
}
