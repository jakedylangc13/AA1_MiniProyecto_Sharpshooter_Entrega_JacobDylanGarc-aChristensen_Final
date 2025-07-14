using UnityEngine;
using TMPro;

public class GunUIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI primaryAmmoText;
    public TextMeshProUGUI secondaryAmmoText;
    public TextMeshProUGUI reloadingText;

    [Header("References")]
    public PlayerGunModeSwitch gunSwitcher;

    private void Update()
    {
        if (gunSwitcher == null)
        {
            Debug.LogWarning("GunModeSwitch no está asignado");
            return;
        }

        // Mostrar balas del disparo primario (balas por carga)
        primaryAmmoText.text = $"Primario: {gunSwitcher.PrimaryCurrentAmmo}/10";

        // Mostrar balas del disparo secundario (balas en cargador y reserva)
        secondaryAmmoText.text = $"Secundario: {gunSwitcher.SecondaryCurrentAmmo}/{gunSwitcher.SecondaryTotalAmmo}";

        // Mostrar texto si está recargando
        reloadingText.text = gunSwitcher.IsReloading ? "Recargando..." : "";

        Debug.Log($"Ammo primaria: {gunSwitcher.PrimaryCurrentAmmo}, secundaria: {gunSwitcher.SecondaryCurrentAmmo}");
    }
}
