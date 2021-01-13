using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Builder : MonoBehaviour
{
    [SerializeField]
    private GameObject[] blocks;
    [SerializeField]
    private GameObject demoBlock;
    private ARRaycastManager raycastManager;
    [SerializeField]
    private LayerMask blockLayer;
    private int selectedBlock;
    // Start is called before the first frame update
    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    public void OnBuildButtonPressed() 
    {

        List<ARRaycastHit> arHits = new List<ARRaycastHit>();
        RaycastHit hitInfo;
        Ray rayToCast = Camera.main.ViewportPointToRay(new Vector2(.5f, .5f));
        if (Physics.Raycast(rayToCast, out hitInfo, 200f, blockLayer))
        {
            Vector3 buildablePosition = hitInfo.normal + hitInfo.transform.position;
            Quaternion buildableRotation = hitInfo.transform.rotation;
            Build(buildablePosition, buildableRotation);
        }
        else 
        {
            raycastManager.Raycast(Camera.main.ViewportPointToRay(new Vector2(.5f, .5f)), arHits, TrackableType.Planes);

            if (arHits.Count > 0)
            {
                Vector3 buildablePosition = new Vector3(Mathf.Round(arHits[0].pose.position.x /1)*1, arHits[0].pose.position.y, Mathf.Round(arHits[0].pose.position.z/1)*1);
                Quaternion buildableRotation = arHits[0].pose.rotation;
                Build(buildablePosition, buildableRotation);
            }
        }
        
    }

    public void SelectBlock(int block)
    {
        selectedBlock = block;

    }

    public void OnDestoryButtonPressed()
    {
        RaycastHit hitInfo;
        Ray rayToCast = Camera.main.ViewportPointToRay(new Vector2(.5f, .5f));
        if (Physics.Raycast(rayToCast, out hitInfo, 200f, blockLayer))
        {
            Destroy(hitInfo.collider.gameObject);
        }
    }

    void Build (Vector3 pos, Quaternion rot) 
    {
        Instantiate(blocks[selectedBlock], pos, rot);
    }
}
