using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerMovement>().UpdateHp += PlayerMovement_UpdateHp;
    }

    private void PlayerMovement_UpdateHp(float hpKf)
    {
        slider.value = hpKf;
    }
}
