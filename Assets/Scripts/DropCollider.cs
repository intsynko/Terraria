using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public delegate void ToolBarDelegate(Dictionary<Block, int> toolBarBlocks);

public class DropCollider : MonoBehaviour
{
    public GrounGenerator grounGenerator;
    public static event ToolBarDelegate UpdateToolBar;
    public List<GameObject> myObjects = new List<GameObject>();
    static Dictionary<Block, int> toolBarBlocks = new Dictionary<Block, int>();
    static List<GameObject> dropZoneObjects = new List<GameObject>();

    private void OnGUI()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Convert.ToInt32(position.x);
            int y = Convert.ToInt32(position.y);
            int plane = ToolBarController.CurrentType.plane;
            // если нет зарегистрированного элемента в данной позиции
            if (!grounGenerator.IsElementAtPosition(plane, x, y))
            {
                // берем текущий экземпляр
                GameObject a = ToolBarController.CurrentType.gameObject;
                if (a != null && myObjects.Contains(a))
                {
                    a.GetComponent<IObject>().Instanse(x, y);
                    myObjects.Remove(a);
                    updateToolBar();
                }
            }
        }
    }

    private void Update()
    {
        // список на удаление 
        var toAdd = dropZoneObjects.Where(obj => obj.GetComponent<IObject>().CurrentCondition == Condition.Dropped).ToList();
        if (toAdd.Count > 0)
        {
            // оставшиеся
            dropZoneObjects = dropZoneObjects.Except(toAdd).ToList();
            myObjects.AddRange(toAdd);
            updateToolBar();
            // те, кто в списке на удаление - удаляются
            foreach (GameObject obj in toAdd)
                StartCoroutine(obj.GetComponent<IObject>().EnterPlayer(transform));
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        IObject obj = collision.GetComponent<IObject>();
        if (obj != null)
            if (obj.CurrentCondition == Condition.Dropped)
            { 
                StartCoroutine(obj.EnterPlayer(transform));
                myObjects.Add(collision.gameObject);
                updateToolBar();
            }
            else
                dropZoneObjects.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IObject obj = collision.GetComponent<IObject>();
        if (obj != null)
            dropZoneObjects.Remove(collision.gameObject);
    }

    // обновление для toolBar кидаем новый словарь эл-нт: кол-во
    private void updateToolBar()
    {
        toolBarBlocks = new Dictionary<Block, int>();
        Dictionary<string, int> namesDict = new Dictionary<string, int>();
        foreach (GameObject obj in myObjects)
        {
            if (obj.GetComponent<Block>())
            {
                if (namesDict.ContainsKey(obj.GetComponent<Block>().name)) namesDict[obj.GetComponent<Block>().name] += 1;
                else namesDict.Add(obj.GetComponent<Block>().name, 1);
            }
        }
        foreach (var item in namesDict)
        foreach (GameObject obj in myObjects)
        {
            if (obj.GetComponent<Block>())
            {
                    if (obj.GetComponent<Block>().name == item.Key)
                    {
                        toolBarBlocks.Add(obj.GetComponent<Block>(), item.Value);
                        break;
                    }
            }
        }
        if (UpdateToolBar!= null) UpdateToolBar(toolBarBlocks);

    }

}
