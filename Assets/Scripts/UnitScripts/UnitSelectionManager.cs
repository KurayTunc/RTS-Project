using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> selectedUnitsList = new List<GameObject>();

    public LayerMask clickable;
    public LayerMask ground;
    public GameObject groundMaker;

    private Camera cam;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else
                {
                    SelectByClicking(hit.collider.gameObject);
                }
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    DeselectAll();
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedUnitsList.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMaker.transform.position = hit.point;
                groundMaker.SetActive(false);
                groundMaker.SetActive(true);
            }
        }
    }

    private void MultiSelect(GameObject birim)
    {
        if (!selectedUnitsList.Contains(birim))
        {
            selectedUnitsList.Add(birim);
            SelectUnit(birim, true);
        }
        else
        {
            SelectUnit(birim, false);
            selectedUnitsList.Remove(birim);
        }
    }

    public void DeselectAll()
    {
        foreach (var birim in selectedUnitsList)
        {
            SelectUnit(birim, false);
        }
        groundMaker.SetActive(false);
        selectedUnitsList.Clear();
    }

    internal void DragSelect(GameObject birim)
    {
        if (!selectedUnitsList.Contains(birim))
        {
            selectedUnitsList.Add(birim);
            SelectUnit(birim, true);
        }
    }

    private void SelectUnit(GameObject birim, bool isSelected)
    {
        TriggerSelectionIndicator_BirimGosterge(birim, isSelected);
        enableUnitMovement(birim, isSelected);
    }

    private void SelectByClicking(GameObject birim)
    {
        DeselectAll();
        selectedUnitsList.Add(birim);
        SelectUnit(birim, true);
    }

    private void enableUnitMovement(GameObject birim, bool shouldMove)
    {
        birim.GetComponent<UnitMovement>().enabled = shouldMove;
    }

    private void TriggerSelectionIndicator_BirimGosterge(GameObject birim, bool isVisible)
    {
        if (birim != null && birim.transform.childCount > 0)
        {
            birim.transform.GetChild(0).gameObject.SetActive(isVisible);
        }
        else
        {
            if (birim == null)
            {
                Debug.LogWarning("TriggerSelectionIndicator_BirimGosterge: Birim null!");
            }
            else
            {
                Debug.LogWarning("TriggerSelectionIndicator_BirimGosterge: Birimin alt nesnesi bulunamadý!");
            }
        }
    }
}
