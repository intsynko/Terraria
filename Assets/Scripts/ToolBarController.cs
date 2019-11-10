using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class ToolBarController : MonoBehaviour
{
    public static Block CurrentType = null; // текущий выбранный тип
    static int cur_num_btn = 0; // текущая выбранная внопка

    public Button[] ToolBarButtons; // кнопки
    private Dictionary<Block, int> BarBlocks; // текущий набор эл-тов

    private void Start()
    {
        DropCollider.UpdateToolBar += DropCollider_UpdateToolBar;
        ToolBarButtons = new Button[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            ToolBarButtons[i] = transform.GetChild(i).GetComponent<Button>();
    }

    // обновление списка элементов
    private void DropCollider_UpdateToolBar(Dictionary<Block, int> toolBarBlocks)
    {
        BarBlocks = toolBarBlocks;
        OffButtons();
        int i = 0;
        foreach (KeyValuePair<Block, int> keyValue in toolBarBlocks)
        {
            ToolBarButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = keyValue.Key.DropSprite;
            ToolBarButtons[i].transform.GetChild(1).GetComponent<Text>().text = keyValue.Value + "";
            ToolBarButtons[i].gameObject.SetActive(true);
            i++;
        }
        UpdateCurType(cur_num_btn);
    }

    // выелючить все кнопки
    private void OffButtons()
    {
        foreach (Button button in ToolBarButtons)
            button.gameObject.SetActive(false);
    }

    // снять выделение со всех кнопок, кроме вызвавшей под номером num
    public void DisableToggles(int num)
    {
        for (int i = 0; i < ToolBarButtons.Length; i++)
        {
            if (i == num)
                ToolBarButtons[i].GetComponent<Outline>().enabled = true;
            else
                ToolBarButtons[i].GetComponent<Outline>().enabled = false;
        }

        UpdateCurType(num);
    }

    // обновление текущего выбранного экземпляра
    void UpdateCurType(int num)
    {
        int j = 0;
        foreach (KeyValuePair<Block, int> keyValue in BarBlocks)
        {
            if (j == num) { CurrentType = keyValue.Key; break; }
            j++;
        }
        cur_num_btn = num;
    }
}
