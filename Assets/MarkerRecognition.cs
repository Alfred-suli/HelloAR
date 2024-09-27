using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MarkerRecognition : MonoBehaviour
{
    public GameObject objectToPlace; // 放置的物体
    public ARTrackedImageManager trackedImageManager;
    public Vector3 objectScale = new Vector3(0.1f, 0.1f, 0.1f); // 调整后的缩放比例

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            // 生成物体并将其设置为图像标记的子物体
            GameObject placedObject = Instantiate(objectToPlace, trackedImage.transform.position, trackedImage.transform.rotation);
            placedObject.transform.SetParent(trackedImage.transform);
            placedObject.transform.localScale = objectScale; // 设置缩放
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            // 更新物体位置和缩放
            if (trackedImage.transform.childCount > 0)
            {
                Transform placedObject = trackedImage.transform.GetChild(0);
                placedObject.position = trackedImage.transform.position;
                placedObject.rotation = trackedImage.transform.rotation;
                placedObject.localScale = objectScale; // 保持缩放
            }
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            // 删除相关的物体
            if (trackedImage.transform.childCount > 0)
            {
                Destroy(trackedImage.transform.GetChild(0).gameObject);
            }
        }
    }
}
