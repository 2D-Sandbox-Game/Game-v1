using UnityEngine;

public class SetBGSprite : MonoBehaviour
{
    // Private variables
    int _width, _height;

    // Start is called before the first frame update
    void Start()
    {
        _height = Generation.s_height;
        _width = Generation.s_width;
        
        // Scales the bachground sprite to the size of the map
        gameObject.transform.localScale = new Vector3(_width, _height);
        // Repositions the sprite to the map position
        gameObject.transform.position = new Vector3(_width / 2, _height / 2, 0);
    }
}
