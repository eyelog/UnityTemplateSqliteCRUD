using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SampleSceneScript : MonoBehaviour
{

    private DBHelper dBHelper;
    private ItemModel currentModel;
    private List<ItemModel> currentList;

    public GameObject currentCard, listViewContainer, itemDialod;
    public TextMeshProUGUI currentItemIdTMP, currentItemValueTMP;
    private int currentItemId;
    private string currentItemValue;
    public Button buttonAdd, buttonUpdate, buttonDelete, buttonDialogCancel, buttonDialogOk;
    public TMP_InputField dialogInput;
    private bool isCreateMode = true;

    void Awake()
    {
        dBHelper = new DBHelper();

        buttonAdd.onClick.AddListener(onButtonAddClicked);
        buttonUpdate.onClick.AddListener(onButtonUpdateClicked);
        buttonDelete.onClick.AddListener(onButtonDeleteClicked);
        buttonDialogCancel.onClick.AddListener(onButtondialogCancelClicked);
        buttonDialogOk.onClick.AddListener(onButtondialogOkClicked);
    }

    void Start()
    {
        itemDialod.gameObject.SetActive(false);
        currentList = dBHelper.getAllItems();
        populateCurrentListItems();
    }

    private void populateCurrentListItems()
    {
        cleanViewList();

        foreach (var item in currentList)
        {
            GameObject itemObject = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/ListItem", typeof(GameObject)));
            itemObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Id = " + item.id;
            itemObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Value = " + item.val;
            itemObject.transform.GetComponent<Button>().onClick.AddListener(() => onItemClicked(item.id, item.val));
            itemObject.transform.SetParent(listViewContainer.transform, false);
        }

        if(currentList.Count > 0)
        {
            setCurrentItem(
                id: currentList[0].id,
                val: currentList[0].val
            );
        }
    }

    private void onItemClicked(int id, string val)
    {
        setCurrentItem(
            id: id,
            val: val
        );
    }

    private void onButtonAddClicked()
    {
        itemDialod.gameObject.SetActive(true);
        isCreateMode = true;
    }

    private void onButtonUpdateClicked()
    {
        itemDialod.gameObject.SetActive(true);
        isCreateMode = false;
        dialogInput.text = currentItemValue;
    }

    private void onButtonDeleteClicked()
    {
        currentList = dBHelper.deleteItem(currentItemId);
        populateCurrentListItems();
    }

    private void onButtondialogCancelClicked()
    {
        itemDialod.gameObject.SetActive(false);
    }

    private void onButtondialogOkClicked()
    {
        if(dialogInput.text != "")
        {
            if(isCreateMode)
            {
                currentList = dBHelper.addItem(
                    new ItemModel(
                        id: 0,
                        val: dialogInput.text
                    )
                );
            } else {
                currentList = dBHelper.updateItem(
                    new ItemModel(
                        id: currentItemId,
                        val: dialogInput.text
                    )
                );
            }

            populateCurrentListItems();
        }
        dialogInput.text = "";
        itemDialod.gameObject.SetActive(false);
    }

    private void setCurrentItem(int id, string val)
    {
        currentItemId = id;
        currentItemValue = val;

        currentItemIdTMP.text = currentItemId.ToString();
        currentItemValueTMP.text = currentItemValue;
    }

    private void cleanViewList()
    {
        if(listViewContainer.transform.childCount > 0){
            for (int i = 0; i < listViewContainer.transform.childCount; i++)
            {
                Destroy(listViewContainer.transform.GetChild(i).gameObject);
            }
        }
    }
}