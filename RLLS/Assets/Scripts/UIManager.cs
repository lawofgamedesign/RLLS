using TMPro;
using UnityEngine;

public class UIManager
{

    TextMeshProUGUI multiplierText;
    private const string MULTIPLIER_TEXT_OBJ = "Multiplier indicator";


    public void Setup()
    {
        multiplierText = GameObject.Find(MULTIPLIER_TEXT_OBJ).GetComponent<TextMeshProUGUI>();
        Services.Events.Register<NewSpeedEvent>(UpdateMultiplierText);
    }


    private void UpdateMultiplierText(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(NewSpeedEvent), "Non-NewSpeedEvent in UpdateMultiplierText.");

        NewSpeedEvent speedEvent = e as NewSpeedEvent;

        multiplierText.SetText(speedEvent.newSpeed.ToString());
    }
}
