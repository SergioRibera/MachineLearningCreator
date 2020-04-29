using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvolutionaryPerceptron;
using EvolutionaryPerceptron.MendelMachine;

public class NeuronalMLObject : BotHandler
{
    public bool fitnessDeltaTime = true;
    public bool isInteraciveObstacles = false;
    public bool isLearningWhereDataBase = false;
    [Header("Button for add sensor")]
    public List<SensorMLObject> Sensors = new List<SensorMLObject>();

    ObjectMachineLearning objectML;

    protected override void Start()
    {
        base.Start();
        objectML = GetComponent<ObjectMachineLearning>();
    }

    void Update()
    {
        var output = nb.SetInput(GetInputs());
        if (output[0, 0] > 0.5f)
        {
            objectML.RealizeAction();
        }
        if (fitnessDeltaTime)
        {
            nb.AddFitness(Time.deltaTime);
        }
        else
        {
            //Add Other Fitness
            nb.AddFitness(1f);
        }
    }

    double[,] GetInputs()
    {
        if (isInteraciveObstacles)
        {
            if (Sensors.Count > 0)
            {
                ObstacleML o = null;

                foreach (var sensor in Sensors)
                {
                    if (sensor.Obstacle)
                    {
                        o = sensor.Obstacle;
                    }
                }
                var n4 = transform.position;
                var n5 = o == null ? Vector3.zero : o.transform.position;

                return new double[1, 7] { { o.distanceToNeural, n4.x, n4.y, n4.z, n5.x, n5.y, n5.z } };
            }
        }
        else if (isLearningWhereDataBase)
        {

        }
        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            if (isInteraciveObstacles)
                nb.Destroy();
        }
    }
}
