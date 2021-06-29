using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTrees : MonoBehaviour
{
    public Sprite crownSprite;
    public Sprite trunkSprite;
    public Sprite stumpSprite;

    public static List<GameObject> trees = new List<GameObject>();

    Generation.BlockType[,] mapArr;
    int[] highestPoints;

    // Start is called before the first frame update
    void Start()
    {
        mapArr = Generation.perlinArr;
        highestPoints = Generation.perlinHeight;

        GenTrees(highestPoints);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenTrees(int[] highestPoints)
    {
        //int treeCount = 0;

        for (int x = 1; x < highestPoints.Length; x++)
        {
            if (TreeCanBePlaced(x, highestPoints) && Random.Range(0, 10) == 0)
            {
                GameObject tree = new GameObject();
                tree.transform.position = new Vector3(x, highestPoints[x] + 1);
                tree.tag = "Tree";
                tree.name = "Tree";
                tree.transform.parent = gameObject.transform;

                CreateTreeObject(Random.Range(3, 13), ref tree);

                trees.Add(tree);
            }
        }
    }

    public void CreateTreeObject(int trunkLength, ref GameObject tree)
    {
        Vector3 pos = tree.transform.position;

        pos += new Vector3(0.5f, 0.5f, 0);
        AddTreePart("Stump", tree, stumpSprite, pos);

        for (int i = 0; i < trunkLength; i++)
        {
            pos += new Vector3(0, 1, 0);
            AddTreePart($"Trunk {i}", tree, trunkSprite, pos);
        }

        pos += new Vector3(0, 2, 0);
        AddTreePart("Crown", tree, crownSprite, pos);
    }

    void AddTreePart(string name, GameObject parent, Sprite sprite, Vector3 pos)
    {
        GameObject treePart = new GameObject();
        treePart.name = name;
        treePart.AddComponent<SpriteRenderer>().sprite = sprite;
        treePart.transform.parent = parent.transform;
        treePart.transform.position += pos;
        
        if (name == "Crown")
        {
            for (int i = (int)pos.x - 1; i < (int)pos.x + 2; i++)
            {
                for (int j = (int)pos.y - 1; j < (int)pos.y + 2; j++)
                {
                    if (i >= 0 && i < mapArr.GetLength(0) && j >= 0 && j < mapArr.GetLength(1))
                    {
                        mapArr[i, j] = Generation.BlockType.Tree;
                    }                    
                }
            }
        }
        else
        {
            mapArr[(int)pos.x, (int)pos.y] = Generation.BlockType.Tree;
        }
    }

    public bool TreeCanBePlaced(int xPos, int[] highestPoint)
    {
        for (int i = xPos - 2; i < xPos + 2; i++)
        {
            if (i >= 0 && i < highestPoint.Length)
            {
                Vector3 pos = new Vector3(i, highestPoint[i] + 1);

                if (IsOccupiedByTree(pos))
                {
                    //Debug.Log(pos);
                    return false;
                }
            }
        }

        return true;
    }

    bool IsOccupiedByTree(Vector3 pos)
    {
        if (trees.Count == 0)
        {
            return false;
        }

        foreach (GameObject tree in trees)
        {
            if (tree.transform.position == pos && tree.name.Contains("Tree"))
            {
                return true;
            }
        }

        return false;
    }
}

