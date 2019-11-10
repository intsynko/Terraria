using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrounGenerator : MonoBehaviour
{
    [SerializeField] private GameObject groundPref;
    [SerializeField] private GameObject stonePref;
    [SerializeField] private GameObject freePref;
    [SerializeField] private GameObject daimPref;
    [SerializeField] private GameObject player;
    [SerializeField] private Tree[] trees;

    private AudioSource audioSource;

    public bool[,,] matrix = new bool[2, 2 * 1000 , 2 * 1000 ];
    private int matrixOffset;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GenerateGround();
    }

    private void GenerateGround()
    {
        matrixOffset = matrix.GetLength(1) / 2;
        int[] heights = new int[500];
        int _offset = 200;
        
        
            int cur_x = -200;
            int cur_y = 0;
            while (cur_x <= 200)
            {
                int len = Random.Range(5, 30);
                int h = Random.Range(-10, 10);
                if (len % 2 != 0) len += 1;
                if (h % 2 != 0) h += 1;
                float k = (float)h / len;
                for (int plane = 1; plane >= 0; plane--)
                for (int x = cur_x; x < cur_x + len; x+=2)
                {
                    float y = cur_y + k * (x - cur_x);
                    if (y % 2 != 0) y += 1;
                    for (int j = -80; j <= y; j += 2)
                    {
                        RandGenerate(plane, new Vector3(x, j));
                    }
                    heights[x + _offset] = (int)y;
                }
                cur_x += len;
                cur_y += h;
                //if (cur_x % 2 != 0) cur_x = -1;
                //if (cur_y % 2 != 0) cur_y = -1;
            }
        

        var pp = player.transform.position;
        player.transform.position = new Vector3(pp.y, heights[(int)pp.x + _offset]);


        // Создание деревьев
        for (int k = -200; k <= 200; k += 2)
        {
            // с вероятностью 10% создаем дерево
            if (Random.RandomRange(0, 101) > 90)
            {
                Tree cur_tree = trees[Random.Range(0, trees.Length)];
                for (int i = 0; i < 7; i++)
                    for (int j = 0; j < 5; j++)
                    {
                        // создаем объект, вышем на него Block, и прогоняем через функцию создания дерева
                        GameObject b = Instantiate(freePref, transform.position + new Vector3(k + j * 2, heights[k + _offset] + i * 2), Quaternion.identity);
                        b.transform.SetParent(transform);
                        Block blck = b.AddComponent<Block>();
                        blck.audioSource = audioSource;
                        blck = cur_tree.GetTree(i, j, blck);
                        if (blck == null) Destroy(b);
                    }
            }
        }
    }

    private Vector3[] Bezie(Vector3[] p)
    {
        List<Vector3> a = new List<Vector3>();
        for (float t = 0; t < 1; t += 0.1f)
        {
            float x = (1 - t) * (1 - t) * p[0].x + 2 * (1 - t) * t * p[1].x + t * t * p[2].x;
            float y = (1 - t) * (1 - t) * p[0].y + 2 * (1 - t) * t * p[1].y + t * t * p[2].y;
            a.Add(new Vector3(x,y));
        }
        return a.ToArray();
    }

    private void RandGenerate(int plane, Vector3 position)
    {
        GameObject a = null;
        int rnd = Random.Range(0, 1000);
        if (rnd >= 0 && rnd < 800) a = groundPref;
        else
        if (rnd > 800 && rnd < 995) a = stonePref;
        else 
            if (position.y < -15)
                a = daimPref;
            else
                a = stonePref;
        if (a != null)
        {
            GameObject obj = Instantiate(a, transform.position + position, Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.GetComponent<Block>().plane = plane;
            obj.GetComponent<Block>().audioSource = audioSource;
            if (plane == 1)
            {
                obj.GetComponent<SpriteRenderer>().color = a.GetComponent<SpriteRenderer>().color - new Color(0.3f, 0.3f, 0.3f, 0);
                obj.GetComponent<Block>().isTexture = true;
            }
        }
    }

    public bool IsElementAtPosition(int plane, int i, int j)
    {
        return matrix[plane, i + matrixOffset, j + matrixOffset];
    }

    public void SetElementAtPosition(int plane, int i, int j)
    {
        //Debug.Log(plane + "  " + (i + offset) + "  "  + (j + offset));
        matrix[plane, i + matrixOffset, j + matrixOffset] = true;
    }

    public void DelElementAtPosition(int plane, int i, int j)
    {
        matrix[plane, i + matrixOffset, j + matrixOffset] = false;
    }

}
