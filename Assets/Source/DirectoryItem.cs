using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DirectoryItem : MonoBehaviour
{
	[SerializeField]
	private List<DirectoryItem> Children;

	private bool isDirectory = false;
	private bool isOpen = false;
	private bool markedForDeath = false;

	private UnityAction updatePos;
	private GameObject prefab;

	private void Awake()
	{
		Children = new List<DirectoryItem>();
	}

	public bool IsDirectory()
	{
		return isDirectory;
	}

	public bool IsMarkedForDeath()
	{
		return markedForDeath;
	}
	
	private bool EndsWithSlash(string text)
	{
		return (text.EndsWith("/") || text.EndsWith("\\"));
	}

	private int GetChildCount()
	{
		return Children.Count;
	}

	private bool HasChildren()
	{
		return GetChildCount() > 0;
	}

	public void SetName(string newName)
	{
		if (!EndsWithSlash(newName) && HasChildren())
			return;

		name = newName;
		gameObject.GetComponentInChildren<Text>().text = newName;
		if (gameObject.name.EndsWith("/") || gameObject.name.EndsWith("\\"))
		{
			isDirectory = true;
			isOpen      = true;
		}
		else
		{
			isDirectory = false;
			isOpen      = false;
		}
	}

	public void SetUpdatePos(UnityAction updatePosAction)
	{
		updatePos = updatePosAction;
	}

	public void SetPrefab(GameObject prefabDirItem)
	{
		prefab = prefabDirItem;
	}

	public void OnNameInputEnd(GameObject inputFieldGameObject)
	{
		var textComponent = inputFieldGameObject.transform.Find("Text").GetComponent<Text>();
		SetName(textComponent.text);
		inputFieldGameObject.SetActive(false);
	}

	public void AddChild(string name)
	{
		if(!isDirectory)
			return;
		var dirItem = Instantiate(prefab, transform);
		var dirItemComponent = dirItem.GetComponent<DirectoryItem>();

		dirItemComponent.SetUpdatePos(updatePos);
		dirItemComponent.SetName(name);
		dirItemComponent.SetPrefab(prefab);

		Children.Add(dirItemComponent);
		updatePos.Invoke();
	}

	public void ChangeOpenState()
	{
		if(!IsDirectory() || GetChildCount() <= 0)
			return;
		isOpen = !isOpen;
		UpdateChildVisibility();
		updatePos.Invoke();
	}

	public void DeleteThis()
	{
		gameObject.SetActive(false);
		markedForDeath = true;
		updatePos.Invoke();
		Destroy(gameObject);
	}

	private void UpdateChildVisibility()
	{
		if(GetChildCount() <= 0)
			return;
		foreach (var child in Children)
		{
			child.gameObject.SetActive(isOpen);
		}
	}

	public float UpdateChildPositions()
	{
		if (!gameObject.activeSelf)
			return 0;
		var nextPos = 105f;
		if (!isOpen)
			return nextPos;

		for(var i = 0; i < GetChildCount(); i++)
		{
			var child = Children[i];
			if (child.IsMarkedForDeath())
			{
				Children.RemoveAt(i);
				i--;
				continue;
			}
			var rect = child.GetComponent<RectTransform>();
			rect.anchoredPosition = new Vector2(20, -nextPos);
			rect.offsetMax = new Vector2(0, rect.offsetMax.y);
			nextPos += child.UpdateChildPositions();
		}

		return nextPos;
	}
}
