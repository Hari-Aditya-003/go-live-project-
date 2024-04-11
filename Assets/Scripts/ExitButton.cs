using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Exiting game..."); // Optional: Log message for debugging
        Application.Quit(); // Quit the application
    }
}
