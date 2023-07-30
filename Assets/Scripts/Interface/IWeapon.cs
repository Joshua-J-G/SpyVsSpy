using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void PickupWeapon()
    {

    }

    public void DropWeapon()
    {

    }

    public MonoBehaviour ReturnScript();



    public void Shoot(LayerMask team);
}
