using UnityEngine;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    public GameObject TexasBlack, HarrisBurgBlack, NorthCarolinaBlack, NebraskaBlack, OregonBlack, Oregon, Nebraska, NorthCarolina, HarrisBurg, Texas;
 
    Dictionary<GameObject, Vector2> initialPositions = new Dictionary<GameObject, Vector2>();   
    public float distanceThreshold = 50f;
    void Start()
    {
        
        StoreInitialPositions();
    }

    void Update()
    {
       
    }

    public void DragFunction(GameObject obj)
    {
      
        obj.transform.position = Input.mousePosition;
    }

    public void ManipulateObject(GameObject obj, GameObject objBlack)
    {
        
        Vector2 initialPos;
        if (initialPositions.TryGetValue(obj, out initialPos))
        {
           
            float distance = Vector3.Distance(obj.transform.position, objBlack.transform.position);

            
            if (distance < distanceThreshold)
            {
                obj.transform.position = objBlack.transform.position;
            }
            else
            {
                
                obj.transform.position = initialPos;
            }
        }
        else
        {
            Debug.LogWarning("Initial position not found for " + obj.name);
        }
    }

   
    void StoreInitialPositions()
    {
        initialPositions[Texas] = Texas.transform.position;
        initialPositions[HarrisBurg] = HarrisBurg.transform.position;
        initialPositions[NorthCarolina] = NorthCarolina.transform.position;
        initialPositions[Nebraska] = Nebraska.transform.position;
        initialPositions[Oregon] = Oregon.transform.position;
    }
}
