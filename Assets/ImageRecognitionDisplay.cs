using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class ARImageTracker : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public Text displayNameText;
    public GameObject markerPrefab; // 用于显示框或标记的预制体
    public float displayDuration = 3.0f;

    private void OnEnable()
    {
        // Register the event handler
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        // Unregister the event handler
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Handle added tracked images
        foreach (var trackedImage in eventArgs.added)
        {
            // Display the name of the detected image
            ShowImageName(trackedImage);
            // Create a marker at the tracked image position
            CreateMarker(trackedImage);
        }

        // Handle updated tracked images
        foreach (var trackedImage in eventArgs.updated)
        {
            // Update the position of the text and marker if needed
            UpdateMarker(trackedImage);
        }
    }

    private void ShowImageName(ARTrackedImage trackedImage)
    {
        displayNameText.text = trackedImage.referenceImage.name;
        displayNameText.transform.position = trackedImage.transform.position + Vector3.up * 0.1f; // 适当调整文本位置
        displayNameText.gameObject.SetActive(true);

        // Start a coroutine to hide the text after a certain time
        StartCoroutine(HideNameAfterDelay());
    }

    private void CreateMarker(ARTrackedImage trackedImage)
    {
        // Instantiate the marker prefab at the tracked image's position and rotation
        GameObject marker = Instantiate(markerPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
        marker.transform.parent = trackedImage.transform; // 使标记成为 trackedImage 的子物体
    }

    private void UpdateMarker(ARTrackedImage trackedImage)
    {
        // Update the marker's position if necessary (e.g., if it's a child of trackedImage)
        // You could also change the appearance based on the tracking state here
    }

    private IEnumerator HideNameAfterDelay()
    {
        // Wait for the display duration
        yield return new WaitForSeconds(displayDuration);

        // Hide the text
        displayNameText.gameObject.SetActive(false);
    }
}
