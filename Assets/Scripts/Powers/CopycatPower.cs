using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopycatPower : MonoBehaviour, IPowerable
{
    private GameObject[] gameObjects;
    private Dictionary<string, Mesh> copyables = new Dictionary<string,Mesh>();

    private Mesh baseMesh;
    public bool guardForm;

    public Aspect aspect;


    void Start()
    {

        baseMesh = gameObject.GetComponent<MeshFilter>().mesh;
        gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in gameObjects)
        {
            ICopyable copyable = obj.GetComponent<ICopyable>();
            if (copyable != null)
            {
                copyables.Add(obj.name, copyable.Copy());
            }
        }
    }

    public void ActivatePower1()
    {
        GetComponent<MeshFilter>().mesh = ChooseForm("test");
    }

    public void ActivatePower2()
    {
        return;
    }


    // right now the dictionary stores a string, mesh pair, but
    // future implementations might use an enum.
    // the string that is passed in is the name of the object the player
    // is colliding with.
    private Mesh ChooseForm(string nameOfObject)
    {
        // set the player's mesh in case the player is not touching anything.
        Mesh form = baseMesh;

        // search the dictionary for the nameOfObject
        Mesh returnedMesh;
        if (copyables.TryGetValue(nameOfObject, out returnedMesh))
        {
            returnedMesh = copyables[nameOfObject];
            form = returnedMesh;
        }

        if (nameOfObject == null)
        {
            aspect.aspectName = Aspect.aspect.Player;
        }
        else if (nameOfObject == "Guard")
        {
            aspect.aspectName = Aspect.aspect.Enemy;
        }
        else
        {
            aspect.aspectName = Aspect.aspect.Object;
        }

        return form;
    }
}