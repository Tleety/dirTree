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
	    Root.SetUpdatePos(UpdatePositions);
	    Root.SetPrefab(DirectoryItemPrefab);
	    Root.AddChild("0/");
	    Root.AddChild("1/");

	    UpdatePositions();
    }

	private void UpdatePositions()
	{
		Root.UpdateChildPositions();
	}

	public void CreateTreeFromTextInput(Text inputComponent)
	{
		CreateTreeFromString(inputComponent.text);
	}

	private void CreateTreeFromString(string paths)
	{
		var separatedPaths = paths.Split(' ');
		foreach (var path in separatedPaths)
		{
			//Send the path to Root and go recursively from there
		}
	}
}
