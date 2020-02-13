using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DirectoryItem : MonoBehaviour
{
	private List<DirectoryItem> Children;

	private bool isDirectory = false;
	private bool isOpen = false;

	
	private void Awake()
	{
		Children = new List<DirectoryItem>();
	}
	
	public bool IsDirectory()
	{
		return isDirectory;
	}

	public string GetName()
	{
		return gameObject.name;
	}

	public void SetName(string newName)
	{
		name = newName;
		gameObject.GetComponentInChildren<Text>().text = newName;
		if (gameObject.name.EndsWith("/") || gameObject.name.EndsWith("\\"))
		{
			isDirectory = true;
			isOpen      = true;
		}
	}

	public List<DirectoryItem> GetChildren()
	{
		return Children;
	}

	private int GetChildCount()
	{
		return Children.Count;
	}

	public DirectoryItem GetChild(int n)
	{
		return Children[n];
	}

	public GameObject AddChild(GameObject prefab, string itemName)
	{
		if (!isDirectory)
		{
			Debug.LogWarning("Trying to create a child to non directory.");
			return null;
		}

		var dirItem = Instantiate(prefab, transform);
		var dirItemComponent = dirItem.GetComponent<DirectoryItem>();
		dirItemComponent.SetName(itemName);

		Children.Add(dirItemComponent);

		return dirItem;
	}

	public void ChangeOpenState()
	{
		isOpen = !isOpen;
		UpdateChildVisibility();
	}

	private void UpdateChildVisibility()
	{
		foreach (var child in Children)
		{
			child.gameObject.SetActive(isOpen);
		}
	}

	public float UpdateChildPositions()
	{
		var nextPos = 30f;
		foreach (var child in Children)
		{
			var rect = child.GetComponent<RectTransform>();
			rect.anchoredPosition = new Vector2(20, -nextPos);
			nextPos += child.UpdateChildPositions();
		}

		return nextPos;
	}
}
