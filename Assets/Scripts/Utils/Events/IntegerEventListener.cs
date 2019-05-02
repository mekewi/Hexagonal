using UnityEngine;

public class IntegerEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public IntegerEvent_SO gameEvent;

    [Tooltip("Response to Invoke When Evemt is raised.")]
    public IntegerEvent Response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }
    public void OnEventRaised(int intToSend)
    {
        Response.Invoke(intToSend);
    }

}
