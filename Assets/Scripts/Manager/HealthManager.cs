using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Entity;

public class PlayerHealth : EntityBase
{
    [SerializeField] private TextMeshPro _healthText;

    public override void TakeDamage(float amount)
    {
        UpdateHealthUI();
        base.TakeDamage(amount);
    }

    private void UpdateHealthUI()
    {

    }

}


