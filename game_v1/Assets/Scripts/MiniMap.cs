using UnityEngine;

public class MiniMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Sets size and position of the in-game map camera
        transform.position = new Vector3(Generation.s_width / 2, Generation.s_height / 2, transform.position.z);
        GetComponent<Camera>().orthographicSize = Generation.s_height / 2;
    }
}
