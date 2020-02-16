using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DirectoryItem : MonoBehaviour
{
	[SerializeField]
	private List<DirectoryItem> Children;

	private bool isDirectory    = false;
	private bool isOpen         = false;
	private bool markedForDeath = false;

	private UnityAction updatePos;
	private GameObject  prefab;

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
		return text.EndsWith(value: "/") || text.EndsWith(value: "\\");
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

		name                                           = newName;
		gameObject.GetComponentInChildren<Text>().text = newName;
		if (gameObject.name.EndsWith(value: "/") || gameObject.name.EndsWith(value: "\\"))
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
		var textComponent = inputFieldGameObject.transform.Find(n: "Text").GetComponent<Text>();
		SetName(textComponent.text);
		inputFieldGameObject.SetActive(value: false);
	}

	public void AddChild(string name)
	{
		AddChildWithReturn(name);
	}

	private GameObject AddChildWithReturn(string name)
	{
		if (!isDirectory)
			return null;
		var dirItem          = Instantiate(prefab, transform);
		var dirItemComponent = dirItem.GetComponent<DirectoryItem>();

		dirItemComponent.SetUpdatePos(updatePos);
		dirItemComponent.SetName(name);
		dirItemComponent.SetPrefab(prefab);

		Children.Add(dirItemComponent);
		updatePos.Invoke();
		return dirItemComponent.gameObject;
	}

	public void AddChildFromPath(string path)
	{
		var paths = path.Split(new[] {'/'}, count: 2);
		if (paths.Length == 2)
			paths[0] += "/";

		var nextDirItem = FindChild(paths[0]);
		if (nextDirItem == null)
			nextDirItem = AddChildWithReturn(paths[0]);

		if (paths.Length == 2)
			nextDirItem.GetComponent<DirectoryItem>()?.AddChildFromPath(paths[1]);
	}

	private GameObject FindChild(string name)
	{
		foreach (var child in Children)
		{
			if (child.name == name)
				return child.gameObject;
		}

		return null;
	}

	public void ChangeOpenState()
	{
		if (!IsDirectory() || GetChildCount() <= 0)
			return;
		isOpen = !isOpen;
		UpdateChildVisibility();
		updatePos.Invoke();
	}

	public void DeleteThis()
	{
		gameObject.SetActive(value: false);
		markedForDeath = true;
		updatePos.Invoke();
		Destroy(gameObject);
	}

	private void UpdateChildVisibility()
	{
		if (HasChildren())
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

		for (var i = 0; i < GetChildCount(); i++)
		{
			var child = Children[i];
			if (child.IsMarkedForDeath())
			{
				Children.RemoveAt(i);
				i--;
				continue;
			}

			var rect = child.GetComponent<RectTransform>();
			rect.anchoredPosition = new Vector2(x: 20, y: -nextPos);
			rect.offsetMax        = new Vector2(x: 0,  y: rect.offsetMax.y);

			nextPos += child.UpdateChildPositions();
		}

		return nextPos;
	}
}
