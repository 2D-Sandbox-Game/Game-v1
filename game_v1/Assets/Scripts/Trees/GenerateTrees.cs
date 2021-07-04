using System.Collections.Generic;
using UnityEngine;

public class GenerateTrees : MonoBehaviour
{
    // static variables
    public static List<GameObject> s_trees = new List<GameObject>();

    // Public variables
    public Sprite CrownSprite;
    public Sprite TrunkSprite;
    public Sprite StumpSprite;

    // Private variables
    Generation.BlockType[,] _mapArr;
    int[] _highestPoints;

    // Start is called before the first frame update
    void Start()
    {
        _mapArr = Generation.s_perlinArr;
        _highestPoints = Generation.s_perlinHeight;

        GenTrees(_highestPoints);
    }

    /// <summary>
    /// Generates trees (as game objects) positioned on the surface of the world map.
    /// </summary>
    /// <param name="highestPoints"></param>
    void GenTrees(int[] highestPoints)
    {
        for (int x = 1; x < highestPoints.Length; x++)
        {
            // Probability of tree spawing: 10%
            if (TreeCanBePlaced(x, highestPoints) && Random.Range(0, 10) == 0)
            {
                GameObject tree = new GameObject();
                // Sets position of the game object to world map position
                tree.transform.position = new Vector3(x, highestPoints[x] + 1);
                tree.tag = "Tree";
                tree.name = "Tree";
                // Makes new tree a child of the game object this script is attached to
                tree.transform.parent = gameObject.transform;

                CreateTreeObject(Random.Range(3, 13), ref tree);
                // Add tree object to tree array
                s_trees.Add(tree);
            }
        }
    }

    /// <summary>
    /// Adds tree parts to the tree.
    /// </summary>
    /// <param name="trunkLength"></param>
    /// <param name="tree"></param>
    public void CreateTreeObject(int trunkLength, ref GameObject tree)
    {
        Vector3 pos = tree.transform.position;

        pos += new Vector3(0.5f, 0.5f, 0);
        AddTreePart("Stump", tree, StumpSprite, pos);

        for (int i = 0; i < trunkLength; i++)
        {
            pos += new Vector3(0, 1, 0);
            AddTreePart($"Trunk {i}", tree, TrunkSprite, pos);
        }

        pos += new Vector3(0, 2, 0);
        AddTreePart("Crown", tree, CrownSprite, pos);
    }

    /// <summary>
    /// Creates tree part game objects and adds them as childs of tree.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <param name="sprite"></param>
    /// <param name="pos"></param>
    void AddTreePart(string name, GameObject parent, Sprite sprite, Vector3 pos)
    {
        GameObject treePart = new GameObject();
        treePart.name = name;
        treePart.AddComponent<SpriteRenderer>().sprite = sprite;
        // Makes new tree part a child of the tree
        treePart.transform.parent = parent.transform;
        treePart.transform.position += pos;
        
        // Adds tree part to the 2D world map array
        if (name == "Crown")
        {
            for (int i = (int)pos.x - 1; i < (int)pos.x + 2; i++)
            {
                for (int j = (int)pos.y - 1; j < (int)pos.y + 2; j++)
                {
                    // Checks if coordinates are within bounds
                    if (i >= 0 && i < _mapArr.GetLength(0) && j >= 0 && j < _mapArr.GetLength(1))
                    {
                        _mapArr[i, j] = Generation.BlockType.Tree;
                    }                    
                }
            }
        }
        else
        {
            _mapArr[(int)pos.x, (int)pos.y] = Generation.BlockType.Tree;
        }
    }

    /// <summary>
    /// Checks if tree can be placed.
    /// </summary>
    /// <param name="xPos"></param>
    /// <param name="highestPoint"></param>
    /// <returns></returns>
    public bool TreeCanBePlaced(int xPos, int[] highestPoint)
    {
        // Checks if there is a tree within 2 blocks to the left or the right of the current position
        for (int i = xPos - 2; i <= xPos + 2; i++)
        {
            if (i >= 0 && i < highestPoint.Length)
            {
                Vector3 pos = new Vector3(i, highestPoint[i] + 1);

                if (IsOccupiedByTree(pos))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Checks if tree game object already exists at this position.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    bool IsOccupiedByTree(Vector3 pos)
    {
        // No trees exist yet
        if (s_trees.Count == 0)
        {
            return false;
        }

        foreach (GameObject tree in s_trees)
        {
            // Tree exists at postion
            if (tree.transform.position == pos && tree.name.Contains("Tree"))
            {
                return true;
            }
        }

        return false;
    }
}

