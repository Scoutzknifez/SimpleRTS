using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayMoney : MonoBehaviour
{
    public TMP_Text moneyDisplay;

    // Update is called once per frame
    void Update()
    {
        moneyDisplay.text = " $ " + GameManager.instance.money;
    }
}
