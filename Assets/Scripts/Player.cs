using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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
        if (x + length - 1 >= GameManager.w || x < 0)
        {
            return;
        }
        if (!GameManager._tiles[new Vector2Int(x + length - 1, y)].IsFree && startCell.Value.x <= x)
        {
            return;
        }

        if (!GameManager._tiles[new Vector2Int(x, y)].IsFree && startCell.Value.x > x)
        {
            return;
        }
        foreach (var cord in vectArr)
        {
            GameManager._tiles[cord].IsFree = true;
        }
        
        trans.position = new Vector3(length * 0.5f + x,0.5f + y, -1);
        vectArr[0] = new Vector2Int(x, y);
        if (length > 1)
        {
            for (int i = 1; i < length; i++)
            {
                vectArr[i] = new Vector2Int(x + i, y);
            }
        }

        foreach (var cord in vectArr)
        {
            GameManager._tiles[cord].IsFree = false;
        }
        
    }
    
    private void OnMouseDrag()
    {
        //Debug.DrawRay(Camera.main.gameObject.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red, 1);
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero ,20, rayLayerMask.value);
        if (hit && startCell == null)
        {
            Cell targetCell = hit.transform.GetComponent<Cell>();
            startCell = new Vector2Int(targetCell.X, targetCell.Y);
        }
        else if (hit)
        {
            Cell targetCell = hit.transform.GetComponent<Cell>();
            if (targetCell.X != startCell.Value.x)
            {
                
                    Move(vectArr[0].x + (targetCell.X - startCell.Value.x), vectArr[0].y);
                    startCell = new Vector2Int(targetCell.X, targetCell.Y);
            }
        }
    }
    
    private void OnMouseUp()
    {
        startCell = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        trans = gameObject.transform;
        trans.localScale = new Vector2(length, 1);
        vectArr = new Vector2Int[length];
        this.Move(startPos.x, startPos.y);
    }
    
}
