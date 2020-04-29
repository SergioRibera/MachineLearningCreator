using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using LibraryPersonal;
using TMPro;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System;
using EvolutionaryPerceptron.MendelMachine;

public class DataManager : MonoBehaviour
{
    public Settings settings;
    public GameObject PrefabProyectUI;
    public GameObject MLCMenu;
    public GameObject DDFMenu;
    public GameObject LoadingMenu;
    public Slider progressBarInit;

    [SerializeField]
    public static ProyectoMLC proyectoSeleccionado = null;

    Idioma idioma;
    Transform scene;
    MendelMachine mendelMachine;

    private void Awake()
    {
        if (Datos.Exist(UtilNames.DATA_SETTINGS, ExtensionFile.sets))
        {
            settings = Datos.Load<Settings>(UtilNames.DATA_SETTINGS, ExtensionFile.sets);
        }
        else
        {
            settings = new Settings();
            Debug.Log(settings.Idioma.ToString());
            settings.Save<Settings>(UtilNames.DATA_SETTINGS, ExtensionFile.sets);
        }
        proyectoSeleccionado = Datos.GetProyect(Datos.ReadFromTempFile().Trim());
        LoadProyectSeleccionado();
    }
    private void Start()
    {
        if (Datos.Exist(settings.Idioma.ToString(), ExtensionFile.lan))
        {
            idioma = Datos.Load<Idioma>(settings.Idioma.ToString(), ExtensionFile.lan);
        }
        List<TextMeshProUGUI> objetosTexto = FindObjectsOfType<TextMeshProUGUI>().ToList();
        //objetosTexto.Find(t => t.name == "Text_Loading").text = idioma.Contenido.GetValue("Text_Loading");
        progressBarInit.maxValue = idioma.Contenido.Count;

        ChangeIdiomaInApp();
        DDFMenu.SetActive(false);
        LoadingMenu.SetActive(false);
        scene = GameObject.FindGameObjectWithTag("SceneMLC").transform;
        mendelMachine = Instantiate(new GameObject("New Neural ML Object"), Vector3.zero, Quaternion.identity).AddComponent<MendelMachine>();
    }
    void ChangeIdiomaInApp()
    {
        List<TextMeshProUGUI> objetosTexto = FindObjectsOfType<TextMeshProUGUI>().ToList();
        for (int i = 0; i < idioma.Contenido.Count; i++)
        {
            foreach (var item in objetosTexto)
            {
                if (item.name == idioma.Contenido[i, true])
                {
                    item.text = idioma.Contenido[i, false];
                }
            }
            progressBarInit.value = i;
            progressBarInit.GraphicUpdateComplete();
        }
        Debug.Log("Loading Proyects Finish ....");
    }

    List<ObjectMachineLearning> objetosInScene = new List<ObjectMachineLearning>();
    void LoadProyectSeleccionado()
    {
        DialogWindow loader = new DialogWindow(TypeDialogWindow.Loader, loadedTextMessage: "Cargando El Proyecto                " + proyectoSeleccionado.proyectName);
        loader.Open();
        foreach (var obj in proyectoSeleccionado.objetosDeMachineLearning)
        {
            if(obj.parent)
            {
                GameObject go = Instantiate(obj).gameObject;
                go.transform.SetParent(scene);
            }else if (scene.Find(obj.parent.name))
            {
                GameObject go = Instantiate(obj).gameObject;
                go.transform.SetParent(obj.parent);
            }
        }
        Thread.Sleep(2000);
        loader.Close();
        MLCMenu.SetActive(true);
        DDFMenu.SetActive(false);
        Camera.main.gameObject.GetComponent<MouseLoockUnityEditor>().enabled = true;
    }
    
    [ContextMenu("Create Neural")]
    public void CreateNeuralMLObject()
    {
        GameObject newObject = new GameObject("New Neural ML Object");
        newObject.transform.position = Vector3.zero;
        newObject.transform.rotation = Quaternion.identity;
        newObject.transform.parent = scene;
        proyectoSeleccionado.objetosDeMachineLearning.Add((ObjectMachineLearning) newObject.AddComponent(typeof(ObjectMachineLearning)));
        newObject.AddComponent(typeof(Brain));
        newObject.AddComponent(typeof(NeuronalMLObject));
        newObject.GetComponent<ObjectMachineLearning>().parent = scene;
        Save();
    }

    [ContextMenu("Create Obstacle")]
    public void CreateObstacleMLObject()
    {
        GameObject newObject = new GameObject("New Obstacle ML Object");
        newObject.transform.position = Vector3.zero;
        newObject.transform.rotation = Quaternion.identity;
        newObject.transform.parent = scene;
        newObject.tag = "Obstacle";
        proyectoSeleccionado.objetosDeMachineLearning.Add((ObjectMachineLearning)newObject.AddComponent(typeof(ObjectMachineLearning)));
        newObject.AddComponent(typeof(ObstacleML));
        newObject.GetComponent<ObjectMachineLearning>().parent = scene;
        Save();
    }

    public static void Save()
    {
        Datos.SaveProyect(proyectoSeleccionado);
    }

    public void Quit()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
            return;
        }
        Application.Quit();
    }
    private void OnApplicationQuit()
    {
        //Datos.DeleteTempFile();
    }
}