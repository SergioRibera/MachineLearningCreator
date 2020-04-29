using System;
using System.Collections.Generic;

namespace Machine_Learning_Creator_Enginer_Main
{
    public class ObjectMachineLearning
    {
        public List<object> Componentes;

        public ObjectMachineLearning()
        {
        }

        public ObjectMachineLearning(List<object> componentes)
        {
            Componentes = componentes;
        }

        public T GetComponent<T>()
        {
            Type tipe1 = typeof(T);
            foreach (var component in Componentes)
            {
                Type tipe2 = component.GetType();
                if (tipe2 == tipe1)
                    return (T)component;
            }
            return default;
        }
    }
}