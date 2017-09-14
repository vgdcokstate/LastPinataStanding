using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

    public GameObject focalObject;

    public string collidesWithTag;

    public float rotationSpeedX = 0.125f;
    public float rotationSpeedY = 0.125f;
    public float cameraDistance = 30.0f;

    const float MAX_PHI = (float)(Mathf.PI/3f);
    const float MIN_PHI = (float)(-0.25f); //

    float cameraTheta;
    float cameraPhi;

    // Use this for initialization
    void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cameraTheta = 0.0f;
        cameraPhi = (float)(Mathf.PI / 6.0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        cameraTheta += rotationSpeedX * Input.GetAxis("Mouse X");
        cameraPhi += rotationSpeedY * Input.GetAxis("Mouse Y");

        Ray worldGeomRay = new Ray(focalObject.transform.position, transform.position - focalObject.transform.position);
        RaycastHit hit = new RaycastHit();
        float distance = cameraDistance;

        if (Physics.Raycast(worldGeomRay, out hit, cameraDistance))
        {
            if(hit.collider.tag == collidesWithTag)
            {
                distance = hit.distance;
            }
        }

        

        cameraPhi = Mathf.Clamp(cameraPhi, MIN_PHI, MAX_PHI);

        Vector3 deltaPosition = new Vector3(Mathf.Cos(cameraTheta)*Mathf.Cos(cameraPhi),
            Mathf.Sin(cameraPhi),
            Mathf.Sin(cameraTheta) * Mathf.Cos(cameraPhi));//Mathf.Cos(cameraPhi));

        transform.position = focalObject.transform.position + (distance*deltaPosition);
        transform.LookAt(focalObject.transform);
	}
}
