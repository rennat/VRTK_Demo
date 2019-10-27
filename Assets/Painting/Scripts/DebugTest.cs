using UnityEngine;

public class DebugTest : MonoBehaviour
{
    public Draw draw;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            draw.StartLine();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            draw.EndLine();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            draw.Redo();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            draw.Undo();
        }
    }
}
