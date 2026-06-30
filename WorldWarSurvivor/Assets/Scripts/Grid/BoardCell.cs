using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : Cell
{    
    public GridObject gridObject;

    public GridObject ShowCell()
    {
        if (gridObject != null)
            gridObject.ShowActions();

        return gridObject;
    }

    public void CloseCell()
    {
        
    }
}
