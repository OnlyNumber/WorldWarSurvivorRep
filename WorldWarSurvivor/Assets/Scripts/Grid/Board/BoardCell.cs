using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : Cell
{
    public GridObject gridObject;

    public bool IsObstacle;

    public bool IsVisible = true;

    public GameObject FullHidePlate;
    public GameObject HidePlate;
    public GameObject CellPlate;

    public GridObject ShowCell()
    {
        if (gridObject != null)
            gridObject.ShowWindowOfUnit();

        return gridObject;
    }

    public void CloseCell()
    {

    }


    #region  For of War
    public void FullHide()
    {
        FullHidePlate.SetActive(true);

        gridObject.Hide();
        CellPlate.SetActive(false);
        HidePlate.SetActive(false);

    }

    public void Hide()
    {
        HidePlate.SetActive(true);


        gridObject.Hide();
        CellPlate.SetActive(false);
        FullHidePlate.SetActive(false);
    }

    public void Show()
    {
        CellPlate.SetActive(true);
        gridObject.Show();

        HidePlate.SetActive(false);
        FullHidePlate.SetActive(false);
    }
    #endregion
}
