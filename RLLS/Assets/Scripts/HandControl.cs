using UnityEngine;

public class HandControl : MonoBehaviour
{

    private Vector3 handScreenPos = new Vector3(0.0f, 0.0f, 0.0f);
    private Rigidbody rb;

    public void Setup()
    {
        Services.Events.Register<MouseEvent>(MoveHands);
        rb = GetComponent<Rigidbody>();
    }

    private void MoveHands(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(MouseEvent), "Non-MouseEvent in MoveHands.");

        MouseEvent mouseEvent = e as MouseEvent;


        //handScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        rb.MovePosition(Camera.main.ScreenToWorldPoint(new Vector3(mouseEvent.Pos.x, mouseEvent.Pos.y, 10.0f)));
    }
}



