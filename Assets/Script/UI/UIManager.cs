using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Make UI Manager Global Scripts")]
    public static UIManager instance { get; private set; }

    [Header("Settings")]
    public Transform spawnPosition;

    [Header("Dropdown")]
    public TMP_Dropdown dropDownLog;
    public TMP_Dropdown dropDownOutput;

    [Header("List Dropdown data")]
    public List<ObjectDataSO> listLogSize;
    public List<ObjectDataSO> listOutput;

    [Header("Quantity Input Field")]
    public TMP_InputField logQtyInputField;
    public TMP_InputField outputQtyIputField;

    

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }
    public void InstantiateObject(GameObject objects)
    {
        Instantiate(objects, spawnPosition.position, objects.transform.rotation);
    }

    public void StartProduce()
    {
        Debug.Log("Log dengan jenis : " + dropDownLog.options[dropDownLog.value].text + " dengan jumlah " + logQtyInputField.text);
        Debug.Log("Mengeluarkan Output: " + dropDownOutput.options[dropDownOutput.value].text + " dengan jumlah " + outputQtyIputField.text);
    }

    private void OnValidate()
    {
        dropDownLog.options.Clear();
        for (int i = 0; i < listLogSize.Count; i++)
        {
            dropDownLog.options.Add(new TMP_Dropdown.OptionData(listLogSize[i].name));
        }
        dropDownLog.RefreshShownValue();

        dropDownOutput.options.Clear();
        for (int i = 0; i < listOutput.Count; i++)
        {
            dropDownOutput.options.Add(new TMP_Dropdown.OptionData(listOutput[i].name));
        }
        dropDownOutput.RefreshShownValue();
    }
}
