using UnityEngine;

public class ParallaxBg : MonoBehaviour {
    Transform _eagleCam; // Camera reference (of its transform)
    Vector3 _previousCamPos;

    // higher relative distance = less parallax (0.1 = 10x effect)
    public float relativeDistanceX = 0f; // Distance of the item (z-index based) 
    public float relativeDistanceY = 0f;

    public float smoothingX = 1f; // Smoothing factor of parallax effect
    public float smoothingY = 1f;

    void Awake () {
        _eagleCam = Camera.allCameras[1].transform;
    }
	
    void Update () {
        if (relativeDistanceX != 0f) {
            float parallaxX = (_previousCamPos.x - _eagleCam.position.x) / relativeDistanceX;
            Vector3 backgroundTargetPosX = new Vector3(transform.position.x + parallaxX, 
                transform.position.y, 
                transform.position.z);
			
            // Lerp to fade between positions
            transform.position = Vector3.Lerp(transform.position, backgroundTargetPosX, smoothingX * Time.deltaTime);
        }

        if (relativeDistanceY != 0f) {
            float parallaxY = (_previousCamPos.y - _eagleCam.position.y) / relativeDistanceY;
            Vector3 backgroundTargetPosY = new Vector3(transform.position.x, 
                transform.position.y + parallaxY, 
                transform.position.z);
			
            transform.position = Vector3.Lerp(transform.position, backgroundTargetPosY, smoothingY * Time.deltaTime);
        }

        _previousCamPos = _eagleCam.position;	
    }
}