using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
		Root.SetName(newName: "Root/");

	    var temp = CreateItem(Root, dirName: "1/");
	    temp = CreateItem(temp.GetComponent<DirectoryItem>(), dirName: "1.1/");
	    CreateItem(temp.GetComponent<DirectoryItem>(), dirName: "1.1.1");
	    CreateItem(temp.GetComponent<DirectoryItem>(), dirName: "1.1.2");
		temp = CreateItem(Root, dirName: "2/");
		CreateItem(Root, dirName: "3");
		temp = CreateItem(temp.GetComponent<DirectoryItem>(), dirName: "2.1/");
	    CreateItem(temp.GetComponent<DirectoryItem>(), dirName: "2.1.1");

	    UpdatePositions();
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

		var temp = parent.AddChild(DirectoryItemPrefab, dirName, UpdatePositions);

		return temp;
	}

	private void UpdatePositions()
	{
		Root.UpdateChildPositions();
	}
}
