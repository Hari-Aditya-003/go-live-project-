using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager2 : MonoBehaviour
{
    public GameObject TexasBlack, HarrisBurgBlack, NorthCarolinaBlack, NebraskaBlack, OregonBlack, Oregon, Nebraska, NorthCarolina, HarrisBurg, Texas;

    Vector2 TexasInitialPos, HarrisBurgInitialPos, NorthCarolinaInitialPos, NebraskaInitialPos, OregonInitialPos;
    // Start is called before the first frame update
    void Start()
    {
        TexasInitialPos = Texas.transform.position;
        HarrisBurgInitialPos = HarrisBurg.transform.position;
        NorthCarolinaInitialPos = Nebraska.transform.position;
        NebraskaInitialPos = Oregon.transform.position;
        OregonInitialPos = Oregon.transform.position;
    }

    // Update is called once per frame
    void DragTexas()
    {
        Texas.transform.position = Input.mousePosition;
    }
    void DragHarrisBurg()
    {
        HarrisBurg.transform.position = Input.mousePosition;
    }
    void DragNorthCarolina()
    {
        NorthCarolina.transform.position = Input.mousePosition;
    }
    void DragNebraska()
    {
        Nebraska.transform.position = Input.mousePosition;
    }
    void DragOregon()
    {
        Oregon.transform.position = Input.mousePosition;
    }
    public void DropTexas()
    {
        float Distance = Vector3.Distance(Texas.transform.position, TexasBlack.transform.position);
        if (Distance < 50) 
        {
            Texas.transform.position = TexasBlack.transform.position;
        }
        else
        {
            Texas.transform.position = TexasInitialPos;
        }
    }
    public void DropHarrisBurg()
    {
        float Distance = Vector3.Distance(HarrisBurg.transform.position, HarrisBurgBlack.transform.position);
        if (Distance < 50)
        {
            HarrisBurg.transform.position = HarrisBurgBlack.transform.position;
        }
        else
        {
            HarrisBurg.transform.position = HarrisBurgInitialPos;
        }
    }
    public void DropNorthCarolina()
    {
        float Distance = Vector3.Distance(NorthCarolina.transform.position, NorthCarolinaBlack.transform.position);
        if (Distance < 50)
        {
            NorthCarolina.transform.position = NorthCarolinaBlack.transform.position;
        }
        else
        {
            NorthCarolina.transform.position = NorthCarolinaInitialPos;
        }
    }
    public void DropNebraska()
    {
        float Distance = Vector3.Distance(Nebraska.transform.position, NebraskaBlack.transform.position);
        if (Distance < 50)
        {
            Nebraska.transform.position = NebraskaBlack.transform.position;
        }
        else
        {
            Nebraska.transform.position = NebraskaInitialPos;
        }
    }
    public void DropOregon()
    {
        float Distance = Vector3.Distance(Oregon.transform.position, OregonBlack.transform.position);
        if (Distance < 50)
        {
            Oregon.transform.position = OregonBlack.transform.position;
        }
        else
        {
            Oregon.transform.position = OregonInitialPos;
        }
    }

}
