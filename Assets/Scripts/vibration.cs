using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour{
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 10, 100, 32), "Vibrate!"))
            Handheld.Vibrate();
    }
}