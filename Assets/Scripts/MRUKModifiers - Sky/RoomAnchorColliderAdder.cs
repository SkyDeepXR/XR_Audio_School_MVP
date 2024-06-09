using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using Oculus.Interaction;
using Obi; 

public class RoomAnchorColliderAdder : MonoBehaviour
{
 // private async void Start()
 //    {
 //        await AddCollidersToAnchors();
 //    }
 //
 //    private async Task AddCollidersToAnchors()
 //    {
 //        var anchors = new List<OVRAnchor>();
 //        var fetchOptions = new OVRAnchor.FetchOptions
 //        {
 //            SingleComponentType = typeof(OVRSemanticLabels),
 //        };
 //
 //        var result = await OVRAnchor.FetchAnchorsAsync(fetchOptions);
 //
 //        if (result == null || result.Count == 0)
 //        {
 //            Debug.LogWarning("No anchors found or failed to fetch anchors.");
 //            return;
 //        }
 //
 //        foreach (var anchor in result)
 //        {
 //            if (!anchor.TryGetComponent(out OVRSemanticLabels labels))
 //                continue;
 //
 //            if (IsTargetLabel(labels.Labels))
 //            {
 //                await ProcessAnchor(anchor);
 //            }
 //        }
 //    }
 //
 //    private bool IsTargetLabel(IList<string> labels)
 //    {
 //        var targetLabels = new HashSet<string>
 //        {
 //            "WALL_FACE",
 //            "FLOOR",
 //            "TABLE",
 //            "OTHER",
 //            "STORAGE",
 //            "BED",
 //            "COUCH",
 //            "GLOBAL_MESH"
 //        };
 //
 //        foreach (var label in labels)
 //        {
 //            if (targetLabels.Contains(label))
 //                return true;
 //        }
 //        return false;
 //    }
 //
 //    private async Task ProcessAnchor(OVRAnchor anchor)
 //    {
 //        // Enable locatable/tracking
 //        if (!anchor.TryGetComponent(out OVRLocatable locatable))
 //            return;
 //
 //        await locatable.SetEnabledAsync(true);
 //
 //        // Localize the anchor
 //        if (locatable.TryGetSceneAnchorPose(out OVRPose scenePose))
 //        {
 //            var anchorObject = new GameObject("AnchorObject");
 //            anchorObject.transform.position = scenePose.position;
 //            anchorObject.transform.rotation = scenePose.orientation;
 //
 //            // Determine and add appropriate collider
 //            if (anchor.TryGetComponent(out OVRBounded3D bounded3D))
 //            {
 //                var boxCollider = anchorObject.AddComponent<BoxCollider>();
 //                boxCollider.size = bounded3D.BoundingBox.size;
 //                Debug.Log($"Added Box Collider to 3D volume anchor.");
 //            }
 //            else if (anchor.TryGetComponent(out OVRBounded2D bounded2D))
 //            {
 //                var planeObject = new GameObject("PlaneObject");
 //                planeObject.transform.SetParent(anchorObject.transform, false);
 //                var boxCollider = planeObject.AddComponent<BoxCollider>();
 //                boxCollider.size = new Vector3(
 //                    bounded2D.BoundingBox.size.x,
 //                    0.01f,
 //                    bounded2D.BoundingBox.size.y);
 //                Debug.Log($"Added Box Collider to 2D plane anchor.");
 //            }
 //        }
 //    }
 //   
}
