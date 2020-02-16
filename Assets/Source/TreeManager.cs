using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Handles any general tree functionality. Mostly the initialization of the tree.
/// </summary>
public class TreeManager : MonoBehaviour
{
	// The root Directory Item, used to set up the tree and starting point for updates and path child creation.
	[SerializeField]
	private DirectoryItem Root;

	// The directory item prefab. Used to create new children copies.
	[SerializeField]
	private GameObject DirectoryItemPrefab;

	// Start is called before the first frame update
	// Set up root with the needed name, update and directory item prefab. Then create some children for good measure.
	private void Start()
	{
		Root.SetName(newName: "Root/");
		Root.SetUpdatePos(UpdatePositions);
		Root.SetPrefab(DirectoryItemPrefab);
		Root.AddChild(name: "first/");
		Root.AddChild(name: "second/");
		Root.AddChild(name: "third");

		UpdatePositions();
	}

	/// <summary>
	/// Updates positions of Directory Items recursively starting with Root.
	/// </summary>
	private void UpdatePositions()
	{
		Root.UpdateChildPositions();
	}

	/// <summary>
	/// Used to create a tree from a path.
	/// The path comes from the input component at the bottom of the screen.
	/// </summary>
	/// <param name="inputComponent">The text component which the path is taken from.</param>
	public void CreateTreeFromTextInput(Text inputComponent)
	{
		CreateTreeFromString(inputComponent.text);
	}

	/// <summary>
	/// Used to create a tree from a path from a string.
	/// More than one string can be sent in separated by a space.
	/// </summary>
	/// <param name="paths"></param>
	private void CreateTreeFromString(string paths)
	{
		var separatedPaths = paths.Split(' ');
		foreach (var path in separatedPaths)
		{
			Root.AddChildFromPath(path);
		}
	}
}
