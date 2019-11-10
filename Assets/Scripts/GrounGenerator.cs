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

    public bool[,,] matrix = new bool[3, 2 * 1000 , 2 * 1000 ];
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
            int len = Random.Range(7, 30);
            int h = Random.Range(-15, 15);
            float k = (float)h / len;
            // сначала генерируем задний план, потом передний
            foreach (int plane in new int[] { 2, 0 })
            {
                for (int x = cur_x; x < cur_x + len; x++)
                {
                    float y = cur_y + k * (x - cur_x);
                    if (y % 2 != 0) y += 1;
                    for (int j = -80; j <= y; j++)
                    {
                        RandGenerate(plane, new Vector3(x, j));
                    }
                    heights[x + _offset] = (int)y;
                }
            }
            cur_x += len;
            cur_y += h;
        }
        
        // ставим игрока чуть выше верхнего блока
        var pp = player.transform.position;
        player.transform.position = new Vector3(pp.y, heights[(int)pp.x + _offset] + 1);


        // Создание деревьев
        for (int k = -200; k <= 200; k += 2)
        {
            // с вероятностью 10% создаем дерево
            if (Random.RandomRange(0, 101) > 90)
            {
                Tree cur_tree = trees[Random.Range(0, trees.Length)];
                GameObject[,] blocks = cur_tree.GetTree();
                for (int i = 0; i < blocks.GetLength(0); i++)
                    for (int j = 0; j < blocks.GetLength(1); j++)
                    {
                        if (blocks[i, j] != null)
                        {
                            GameObject b = Instantiate(
                                blocks[i, j],
                                transform.position + new Vector3(k + j, heights[k + _offset] + i),
                                Quaternion.identity);
                            b.transform.SetParent(transform);
                            b.GetComponent<Block>().audioSource = audioSource;
                        }
                    }
                k += blocks.GetLength(0);
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
