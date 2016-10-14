using System;
using System.Collections.Generic;
using Windows.Gaming.Input;

public class GamepadHandler
{
	public GamepadHandler()
	{
        this.buttonStates = new Dictionary<GamepadButtons, bool>();
	}

    public Dictionary<GamepadButtons, bool> buttonStates { get; set; }

    public void ProcessReading(GamepadReading reading)
    {
        HandleLeftStickX(reading.LeftThumbstickX);
        HandleLeftStickY(reading.LeftThumbstickY);
        HandleRightStickX(reading.RightThumbstickX);
        HandleRightStickY(reading.RightThumbstickY);
        HandleLeftTrigger(reading.LeftTrigger);
        HandleRightTrigger(reading.RightTrigger);
        
        foreach (GamepadButtons btn in Enum.GetValues(typeof(GamepadButtons)))
        {
            bool state = reading.Buttons.HasFlag(btn);
            if (state != buttonStates[btn])
            {
                buttonStates[btn] = state;
                if (state) {
                    this.HandleButtonOn(btn);
                }
                else
                {
                    this.HandleButtonOff(btn);
                }
            }
        }
    }


    private void HandleButtonOn(GamepadButtons btn)
    {
        if (btn == GamepadButtons.A)
        {

        }
    }

    private void HandleButtonOff(GamepadButtons btn)
    {
        if (btn == GamepadButtons.A)
        {

        }
    }

    private void HandleLeftStickX(double value)
    {

    }

    private void HandleLeftStickY(double value)
    {

    }

    private void HandleRightStickX(double value)
    {

    }

    private void HandleRightStickY(double value)
    {

    }

    private void HandleLeftTrigger(double value)
    {

    }

    private void HandleRightTrigger(double value)
    {

    }
}
