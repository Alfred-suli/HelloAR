using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Unity.XR.CoreUtils;
using UnityEngine.XR.ARSubsystems;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    private XROrigin arOrigin;
    public ARRaycastManager aRRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        // 打印当前的触摸计数
        Debug.Log("Touch Count: " + Input.touchCount);

        // 检查触摸输入
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);  // 获取第一个触摸事件
            Debug.Log("Touch detected: " + touch.phase);

            // 如果触摸有效且位置有效，放置物体
            if (placementPoseIsValid && touch.phase == TouchPhase.Began)
            {
                PlaceObject();
            }
        }
    }

    private void PlaceObject()
    {
        Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        // Use the AR camera for screen center
        var screenCenter = arOrigin.Camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        // Perform raycast and check if we hit any AR planes
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes | TrackableType.FeaturePoint);

        placementPoseIsValid = hits.Count > 0;

        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            // Adjust object rotation based on camera bearing
            var cameraForward = arOrigin.Camera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
