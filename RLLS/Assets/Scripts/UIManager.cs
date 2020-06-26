using TMPro;
using UnityEngine;

public class UIManager
{

    TextMeshProUGUI multiplierText;
    private const string MULTIPLIER_TEXT_OBJ = "Multiplier indicator";

    TextMeshProUGUI explanatoryText;
    private const string EXPLANATORY_TEXT_OBJ = "Multiplier explanation";


    public void Setup()
    {
        multiplierText = GameObject.Find(MULTIPLIER_TEXT_OBJ).GetComponent<TextMeshProUGUI>();
        Services.Events.Register<NewSpeedEvent>(UpdateMultiplierText);
        explanatoryText = GameObject.Find(EXPLANATORY_TEXT_OBJ).GetComponent<TextMeshProUGUI>();
    }


    private void UpdateMultiplierText(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(NewSpeedEvent), "Non-NewSpeedEvent in UpdateMultiplierText.");

        NewSpeedEvent speedEvent = e as NewSpeedEvent;

        multiplierText.SetText(speedEvent.newSpeed.ToString());
    }



    /// <summary>
    /// Change the text displayed next to the multiplier.
    /// </summary>
    /// <param name="newMessage">The new message</param>
    public void ChangeExplanatoryMessage(string newMessage)
    {
        explanatoryText.SetText(newMessage);
    }
}
