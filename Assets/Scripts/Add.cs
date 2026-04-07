using UnityEngine;

public class Add : MonoBehaviour
{
    public void Start()
    {
        int a = 5;
        int b = 10;
        int sum = a + b;
        Debug.Log("The sum of " + a + " and " + b + " is: " + sum);
    }
}
