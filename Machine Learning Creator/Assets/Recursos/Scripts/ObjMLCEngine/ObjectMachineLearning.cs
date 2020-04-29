using UnityEngine;
using System;
using System.Collections.Generic;

public class ObjectMachineLearning : MonoBehaviour
{
    public Transform parent = null;
    public List<object> Componentes;

    public ObjectMachineLearning()
    {
    }

    public ObjectMachineLearning(List<object> componentes)
    {
        Componentes = componentes;
    }

    internal void RealizeAction()
    {

    }
}