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

    int[,] mapArr;
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
        int treeCount = 0;

        for (int x = 1; x < highestPoints.Length; x++)
        {
            if (TreeCanBePlaced(x, highestPoints) && Random.Range(0, 10) == 0)
            {
                GameObject tree = CreateTreeObject(Random.Range(3, 13));
                tree.tag = "Tree";
                tree.name = $"Tree {treeCount++}";
                tree.transform.position = new Vector3(x, highestPoints[x] + 1);
                tree.transform.parent = gameObject.transform;


                //foreach (GameObject treePart in tree.GetComponentInChildren<Transform>())
                //{
                //    //Generation.perlinArr[(int)treePart.transform.position.x, (int)treePart.transform.position.y] = (int)Generation.BlockType.Tree;
                //}

                trees.Add(tree);
            }
        }
    }

    GameObject CreateTreeObject(int trunkLength)
    {
        GameObject tree = new GameObject();
        Vector3 pos = new Vector3(0, 0, 0);

        pos += new Vector3(0.5f, 0.5f, 0);
        AddTreePart("Stump", tree, stumpSprite, pos);

        for (int i = 0; i < trunkLength; i++)
        {
            pos += new Vector3(0, 1, 0);
            AddTreePart($"Trunk {i}", tree, trunkSprite, pos);
        }

        pos += new Vector3(0, 2, 0);
        AddTreePart("Crown", tree, crownSprite, pos);

        return tree;
    }

    void AddTreePart(string name, GameObject parent, Sprite sprite, Vector3 pos)
    {
        GameObject treePart = new GameObject();
        treePart.name = name;
        treePart.AddComponent<SpriteRenderer>().sprite = sprite;
        treePart.transform.parent = parent.transform;
        treePart.transform.position += pos;

        //Generation.perlinArr[(int)pos.x, (int)pos.y] = (int)Generation.BlockType.Tree;
    }

    bool TreeCanBePlaced(int xPos, int[] highestPoint)
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
            if (tree.transform.position == pos)
            {
                return true;
            }
        }

        return false;
    }
}

