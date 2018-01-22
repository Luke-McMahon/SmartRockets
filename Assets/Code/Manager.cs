using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static int Count = 0;
    public static int Lifespan = 500;
    public static int Generation = 0;
    public static float MaxFitness = 0.0f;

    public GameObject Population;

    public Text InfoText;

    private void Update()
    {
        InfoText.text = "Lifespan: " + Count + "/" + Lifespan +
                        "\nGeneration: " + Generation +
                        "\nMax Fitness: " + string.Format("{0:F1}", MaxFitness);
    }
}
