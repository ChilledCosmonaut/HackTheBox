using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attack : MonoBehaviour
{
    private GameObject targetPosition;//Hilfsvariablen zur vereinfachung des codes
    private GameObject target;
    private GameObject attacker;
    private GameObject weapon;
    private int part;
    private int weaponNumber;
    private int _hitPercentage;
    public int halfCover; //Gibt die Trefferwahrscheinlichkeit an
    public int fullCover;
    public GameObject attackUI;  //Sind die einzelnen UIElemente
    public GameObject LimbUI;
    

    public void setAttack(int part, int weaponNumber, GameObject attacker, GameObject target)
    {
       /* this.part = part;
        this.weaponNumber = weaponNumber;
        this.attacker = attacker;
        this.target = target;*/
    }

    public void attack(int part, int weaponNumber, GameObject attacker, GameObject target)//Wird bei ausgewählten Limb aufgerufen. I ist hier das entsprechende Limb
    {
        /*this.attacker = attacker;

        switch (target.GetComponent<UnitStat>().currentGrid.GetComponent<GridStat>().status)
        {
            case 1:
                _hitPercentage += halfCover;
                break;
            case 2:
                _hitPercentage += fullCover;
                break;
        }
        
        switch (weaponNumber)
        {
            case 1:
                weapon = attacker.GetComponent<UnitStat>().parts[0];
                break;
            case 2:
                weapon = attacker.GetComponent<UnitStat>().parts[1];
                break;
        }
        
        _hitPercentage += weapon.GetComponent<WeaponStat>().GetPercetage();
        
        _hitPercentage += weapon.GetComponent<WeaponStat>().GetPercetage();
        
        if (Random.Range(0,100) <= _hitPercentage)
        {
            target.GetComponent<UnitStat>().health[part] -= weapon.GetComponent<WeaponStat>().GetDamage();
        }

        Exit();*/
    }

    public void attack()
    {
        /*print("attack player");
        switch (target.GetComponent<UnitStat>().currentGrid.GetComponent<GridStat>().status)
        {
            case 1:
                _hitPercentage += halfCover;
                break;
            case 2:
                _hitPercentage += fullCover;
                break;
        }
        
        switch (weaponNumber)
        {
            case 1:
                weapon = attacker.GetComponent<UnitStat>().parts[0];
                break;
            case 2:
                weapon = attacker.GetComponent<UnitStat>().parts[1];
                break;
        }

        _hitPercentage += weapon.GetComponent<WeaponStat>().GetPercetage();
        
        if (Random.Range(0,100) <= _hitPercentage)
        {
            target.GetComponent<UnitStat>().health[part] -= weapon.GetComponent<WeaponStat>().GetDamage();
        }

        attacker.GetComponent<UnitStat>().actions -= 1;
        
        Exit(); */
    }

    public void SetOpponent(GameObject attacker, GameObject target) // Setzt das Script auf die aktuellen Partner
    {
       /* this.target = target;
        this.attacker = attacker;
        attackUI.SetActive(true);
        //hitPercentage = 0;
        switch (target.GetComponent<UnitStat>().currentGrid.GetComponent<GridStat>().status)
        {
            case 1:
                //hitPercentage += halfCover;
                break;
            case 2:
                //hitPercentage += fullCover;
                break;
        } */
    }

    void deactivateAttackUI()
    {
        attackUI.SetActive(false);
    }

    public void SelectWeapon(int weaponNumber) //Wählt entsprechende Waffe aus
    {
      /*  deactivateAttackUI();
        switch (weaponNumber)
        {
            case 1:
                weapon = attacker.GetComponent<UnitStat>().parts[0];
                break;
            case 2:
                weapon = attacker.GetComponent<UnitStat>().parts[1];
                break;
        }
        LimbUI.SetActive(true);
        //hitPercentage += weapon.GetComponent<WeaponStat>().GetPercetage();*/
    }

    public void Exit() //Setzt sicherheitshalber alles auf Null
    {
        //hitPercentage = 0;
        /*if (attacker.CompareTag("Player"))
        {
            attacker.GetComponent<PlayerController>().deactivateUI();
        }
        target = null;
        attacker = null;
        weapon = null; */
        
    }
}
