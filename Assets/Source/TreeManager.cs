using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{

	[SerializeField]
	private DirectoryItem Root;

	[SerializeField]
	private GameObject DirectoryItemPrefab;

	public string InputArray;
    // Start is called before the first frame update
    void Start()
    {
		Root.SetName("Root/");

	    CreateItem(Root, "SomeItem");
		var temp = CreateItem(Root, "newfolder/");
		CreateItem(Root, "SomeSecondItem");
		CreateItem(temp.GetComponent<DirectoryItem>(), "folder2/");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private GameObject CreateItem(DirectoryItem parent, string dirName)
	{
		if (dirName == null)
		{
			Debug.LogWarning("Trying to create dirItem without name.");
			return null;
		}

		if (parent == null)
			parent = Root;

		var temp = parent.AddChild(DirectoryItemPrefab, dirName);
		UpdatePositions();
		return temp;
	}

	private void UpdatePositions()
	{
		Root.UpdateChildPositions();
	}
}
