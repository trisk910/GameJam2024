using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public float batteryLife = 100f; //%
    public float lifeSeconds = 420f; // 7min
    public float lowBatteryWarning = 10f; // %
    public float rechargeAmount = 5f; // Recharge amount
    public Light flashlightLight;

    private bool isOn = false;
    private bool isEquipped = false;
    private float batteryDrainRate; 

    void Start()
    {
        flashlightLight = GetComponentInChildren<Light>(); 
        flashlightLight.enabled = false;
        batteryDrainRate = 100f / lifeSeconds;
    }

    void Update()
    {
        if (isEquipped && Input.GetKeyDown(KeyCode.Q) && batteryLife > 0f)
        {
            ToggleFlashlight();
        }
        if (isEquipped && Input.GetKeyDown(KeyCode.R) && batteryLife < 100f && !isOn)
        {
            RechargeBattery();
        }

        if (isOn)
        {
            DrainBattery();
        }
        if (batteryLife <= lowBatteryWarning && batteryLife > 0)
        {
            BlinkFlashlight();
        }
        else
        {
            flashlightLight.enabled = isOn;
        }
    }

    private void ToggleFlashlight()
    {
        isOn = !isOn;
        flashlightLight.enabled = isOn; 
    }

    private void DrainBattery()
    {
        batteryLife -= batteryDrainRate * Time.deltaTime;
        if (batteryLife < 0)
        {
            batteryLife = 0;
            isOn = false;
            flashlightLight.enabled = false;
        }
    }

    private void BlinkFlashlight()
    {
        if (Mathf.PingPong(Time.time * 2, 1) > 0.2f)
        {
            flashlightLight.enabled = false;
        }
        else
        {
            flashlightLight.enabled = true;
        }
    }
    private void RechargeBattery()
    {
        batteryLife += rechargeAmount;
        if (batteryLife > 100f)
        {
            batteryLife = 100f;
        }
    }
    public void SetEquipped(bool equipped)
    {
        isEquipped = equipped;
    }
}
