using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public float batteryLife = 100f; // Full battery life at start (100%)
    public float lifeSeconds = 420f; // Total lifetime of battery in seconds (default 7 minutes)
    public float blinkThreshold = 10f; // Battery level threshold to start blinking
    public float rechargeRate = 10f; // Battery recharge rate when pressing R
    public Light flashlightLight; // The spotlight attached as child of the flashlight

    private bool isOn = false; // The state of the flashlight (on/off)
    private bool isRecharging = false; // Whether the flashlight is being recharged
    private float timeSinceRelease = 0f; // Time since the R key was released
    private float batteryDrainRate; // Calculated battery drain rate based on lifeSeconds

    void Start()
    {
        flashlightLight = GetComponentInChildren<Light>(); // Assuming the spotlight is a child of this GameObject
        flashlightLight.enabled = false; // Start with the flashlight off

        // Calculate battery drain rate based on lifeSeconds and batteryLife
        batteryDrainRate = 100f / lifeSeconds;
    }

    void Update()
    {
        if (isRecharging)
        {
            RechargeBattery();
        }
        else if (isOn)
        {
            DrainBattery();
        }

        // Check for flashlight toggle (Q key)
        if (Input.GetKeyDown(KeyCode.Q) && batteryLife > 0f)
        {
            ToggleFlashlight();
        }

        // Check for flashlight recharge (R key)
        if (Input.GetKey(KeyCode.R) && batteryLife < 100f && !isOn)
        {
            StartRecharging();
        }

        // Stop recharging if the key R is released for 1.5 seconds
        if (Input.GetKeyUp(KeyCode.R))
        {
            timeSinceRelease = Time.time;
        }

        if (!Input.GetKey(KeyCode.R) && Time.time - timeSinceRelease > 1.5f)
        {
            StopRecharging();
        }

        // Check if battery is low enough to blink
        if (batteryLife <= blinkThreshold && batteryLife > 0)
        {
            BlinkFlashlight();
        }
        else
        {
            flashlightLight.enabled = isOn; // If flashlight is on, show the light; else, hide it
        }
    }

    // Toggle the flashlight on/off
    private void ToggleFlashlight()
    {
        isOn = !isOn;
        flashlightLight.enabled = isOn; // Turn on/off the spotlight
    }

    // Drain the battery while the flashlight is on
    private void DrainBattery()
    {
        batteryLife -= batteryDrainRate * Time.deltaTime;

        // Ensure battery doesn't go below 0
        if (batteryLife < 0)
        {
            batteryLife = 0;
        }
    }

    // Blink the flashlight when battery is low
    private void BlinkFlashlight()
    {
        if (Mathf.PingPong(Time.time * 2, 1) > 0.5f)
        {
            flashlightLight.enabled = false;
        }
        else
        {
            flashlightLight.enabled = true;
        }
    }

    // Start recharging the flashlight when R is pressed
    private void StartRecharging()
    {
        isRecharging = true;
        flashlightLight.enabled = false; // Turn off the flashlight while recharging
    }

    // Recharge the battery over time while holding R
    private void RechargeBattery()
    {
        if (batteryLife < 100f)
        {
            batteryLife += rechargeRate * Time.deltaTime;
        }

        if (batteryLife >= 100f)
        {
            batteryLife = 100f;
        }
    }

    // Stop recharging when R is released for 1.5 seconds
    private void StopRecharging()
    {
        isRecharging = false;
        flashlightLight.enabled = false; // Keep the flashlight off
    }

    // Optional: A method to reset the flashlight if needed
    public void ResetFlashlight()
    {
        batteryLife = 100f;
        isOn = false;
        flashlightLight.enabled = false;
    }
}
