
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject TexasBlack, HarrisBurgBlack, NorthCarolinaBlack, NebraskaBlack, OregonBlack, Oregon, Nebraska, NorthCarolina, HarrisBurg, Texas;

    Vector2 TexasInitialPos, HarrisBurgInitialPos, NorthCarolinaInitialPos, NebraskaInitialPos, OregonInitialPos;

    void Start()
    {
        TexasInitialPos = Texas.transform.position;
        OregonInitialPos = Oregon.transform.position;
        HarrisBurgInitialPos = HarrisBurg.transform.position;
        NorthCarolinaInitialPos = NorthCarolina.transform.position;
        NebraskaInitialPos = Nebraska.transform.position;
    }

    void Update()
    {
        // Example: Add Update method if you want to continuously update positions during the drag.
    }
    public void DraguFnction(GameObject obj)
    {
        obj.transform.position = Input.mousePosition;
    }
    public void ReleaseObject(GameObject obj, GameObject objBlack, Vector2 initialPos)
    {
        float distance = Vector3.Distance(obj.transform.position, objBlack.transform.position);
        if (distance < 50)
        {
            obj.transform.position = objBlack.transform.position;
        }
        else
        {
            obj.transform.position = initialPos;
        }
    }
    public void DropFnction(GameObject obj)
    {
        obj.transform.position = Input.mousePosition;
    }
}