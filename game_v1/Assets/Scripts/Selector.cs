using UnityEngine;
using UnityEngine.Tilemaps;

public class Selector : MonoBehaviour
{
    // Public variables
    public Tilemap Tilemap;

    // Private variables
    TileBase _selectedTile = null;
    Vector3 _mousePos = Vector3.zero;
    Vector3Int _mousePosTranslated = Vector3Int.zero;
    Vector3Int _posSelectedTile = Vector3Int.zero;

    // Update is called once per frame
    void Update()
    {
        // World Space
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Converts worlds space click point to tile map click point
        _mousePosTranslated = Tilemap.WorldToCell(_mousePos);
        _selectedTile = Tilemap.GetTile(_mousePosTranslated);

        // Mouse position changed from the previous selection
        if (_mousePosTranslated != _posSelectedTile)
        {
            GetComponent<SpriteRenderer>().color = Color.clear;
            _posSelectedTile = _mousePosTranslated;

            // Activates the selector
            if (_selectedTile != null || IsNextToExistingTile(_posSelectedTile))
            {
                transform.position = new Vector3(0.5f + _posSelectedTile.x, 0.5f + _posSelectedTile.y);
                GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    /// <summary>
    /// Checks if position is next to existing tile in tilemap.
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    bool IsNextToExistingTile(Vector3Int tilePos)
    {
        Vector3Int[] directions = { new Vector3Int(-1, 0, 0), new Vector3Int(0, 1, 0), new Vector3Int(1, 0, 0), new Vector3Int(0, -1, 0) };

        foreach (var dir in directions)
        {

            if (Tilemap.GetTile(tilePos + dir) != null)
            {
                return true;
            }
        }

        return false;
    }
}
