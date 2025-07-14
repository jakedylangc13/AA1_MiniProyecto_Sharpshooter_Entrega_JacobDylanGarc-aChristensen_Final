using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 30;

    private void OnTriggerEnter(Collider other)
    {
        var gun = other.GetComponentInChildren<PlayerGunModeSwitch>();

        if (gun != null)
        {
            gun.AddAmmo(ammoAmount); 
            Destroy(gameObject); 
        }
    }
}
