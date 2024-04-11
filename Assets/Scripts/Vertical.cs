using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vertical : MonoBehaviour
{
    
    public int length;
    private Transform trans;
    private Vector2Int[] vectArr;
    public Vector2Int startPos;

    //Moving Logic
    public LayerMask rayLayerMask;
    private Vector2Int? startCell = null;
    
    private void Move(int x, int y)
    {
        //Border check
        if (y + length - 1 >= GameManager.h || y < 0)
        {
            return;
        }
        //IsFree check
        if (!GameManager._tiles[new Vector2Int(x, y + length - 1)].IsFree && startCell.Value.y <= y)
        {
             return;
        }

        if (!GameManager._tiles[new Vector2Int(x, y)].IsFree && startCell.Value.y > y)
        {
            return;
        }
        
        //Cell liberation
        foreach (var cord in vectArr)
        {
            GameManager._tiles[cord].IsFree = true;
        }
        
        trans.position = new Vector3(0.5f + x, (this.length * 0.5f) + y, -1);
        vectArr[0] = new Vector2Int(x, y);
        if (length > 1)
        {
            for (int i = 1; i < length; i++)
            {
                vectArr[i] = new Vector2Int(x, y + i);
            }
        }
        //Cell ocupation
        foreach (var cord in vectArr)
        {
            GameManager._tiles[cord].IsFree = false;
        }
        
    }
    //Raycasting
    private void OnMouseDrag()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero ,20, rayLayerMask.value);
        //Strart cell detected
        if (hit && startCell == null)
        {
            Cell targetCell = hit.transform.GetComponent<Cell>();
            startCell = new Vector2Int(targetCell.X, targetCell.Y);
        }
        //Next cell detected
        else if (hit)
        {
            Cell targetCell = hit.transform.GetComponent<Cell>();
            if (targetCell.Y != startCell.Value.y)
            {
                Move(vectArr[0].x, vectArr[0].y + (targetCell.Y - startCell.Value.y));
                startCell = new Vector2Int(targetCell.X, targetCell.Y);
            }
        }
    }
// End of raycasting
    private void OnMouseUp()
    {
        startCell = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        trans = gameObject.transform;
        trans.localScale = new Vector2(1, length);
        vectArr = new Vector2Int[length];
        Move(startPos.x, startPos.y);
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
