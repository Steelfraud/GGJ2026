using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIUtils
{

    public static void CopyRectTransformSize(RectTransform copyFrom, RectTransform copyTo)
    {
        copyTo.anchorMin = copyFrom.anchorMin;
        copyTo.anchorMax = copyFrom.anchorMax;
        copyTo.anchoredPosition = copyFrom.anchoredPosition;
        copyTo.sizeDelta = copyFrom.sizeDelta;
    }

    public static Vector2 GetAbsoluteSize(RectTransform rectTransform)
    {
        var corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        var width = Mathf.Abs(corners[2].x - corners[0].x);
        var height = Mathf.Abs(corners[2].y - corners[0].y);

        return new Vector2(width, height);
    }

    public static Vector2 GetGridSizeWithWidth(RectTransform gridRect, int elementsMax, float MaxCellSize = 9999f)
    {
        Vector2 size = GetAbsoluteSize(gridRect);
        float cell = size.x / elementsMax;

        if (cell > MaxCellSize)
        {
            cell = MaxCellSize;
        }

        Vector2 cellSize = new Vector2(cell, cell);
        return cellSize;
    }

}

public class TextUIPool : GenericUIPool<TextMeshProUGUI> { }

public class ButtonUIPool : GenericUIPool<Button> { }

public class ImageUIPool : GenericUIPool<Image> { }

public class GenericUIPool<T> where T : MonoBehaviour
{

    private T defaultItem;
    private Transform elementParent;
    private List<T> pooledItems = new List<T>();

    public List<T> ElementsInList => new List<T>(this.pooledItems);
    public List<T> ActiveElementsInList => new List<T>(this.pooledItems).FindAll(x => x.gameObject.activeSelf);

    public void SetupPool(T firstItem)
    {
        SetupPool(firstItem, firstItem.transform.parent);
    }

    public void SetupPool(T firstItem, Transform parent)
    {
        this.pooledItems.Add(firstItem);
        defaultItem = firstItem;
        this.elementParent = parent;
        ResetPool();
    }

    public void ResetPool()
    {
        foreach (T item in this.pooledItems) { item.gameObject.SetActive(false); }
    }
    
    public T GetNextItem()
    {
        T elementToGet = this.pooledItems.Find(x => x.gameObject.activeSelf == false);

        if (elementToGet == null)
        {
            elementToGet = GameObject.Instantiate(this.defaultItem.gameObject, this.elementParent).GetComponent<T>();
            this.pooledItems.Add(elementToGet);
        }

        elementToGet.gameObject.SetActive(true);
        return elementToGet;
    }

    public void ReturnToPool(T itemToReturn)
    {
        itemToReturn.gameObject.SetActive(false);
    }

}

public class RectTransformSettings
{

    private Vector2 anchorMin;
    private Vector2 anchorMax;
    private Vector2 anchoredPosition;
    private Vector2 sizeDelta;

    public void CopyRectTransformSize(RectTransform copyFrom)
    {
        this.anchorMin = copyFrom.anchorMin;
        this.anchorMax = copyFrom.anchorMax;
        this.anchoredPosition = copyFrom.anchoredPosition;
        this.sizeDelta = copyFrom.sizeDelta;
    }

    public void PasteRectTransformSize(RectTransform copyTo)
    {
        copyTo.anchorMin = this.anchorMin;
        copyTo.anchorMax = this.anchorMax;
        copyTo.anchoredPosition = this.anchoredPosition;
        copyTo.sizeDelta = this.sizeDelta;
    }

}

public class TransformSettings
{

    private Vector3 position;
    private Quaternion rotation;

    public void CopyTransform(Transform copyFrom, bool isLocal = false)
    {
        this.position = isLocal ? copyFrom.localPosition : copyFrom.position;
        this.rotation = isLocal ? copyFrom.localRotation : copyFrom.rotation;
    }

    public void PasteTransform(Transform copyTo, bool isLocal = false)
    {
        if (isLocal)
        {
            copyTo.localPosition = this.position;
            copyTo.localRotation = this.rotation;
        }
        else
        {
            copyTo.position = this.position;
            copyTo.rotation = this.rotation;
        }
    }

}

public class FloatingTextInfo
{

    public GameObject floatingTextParent;
    public string textToShow;
    public Color colorOfText;

    public FloatingTextInfo(GameObject gObj, string text, Color colour)
    {
        this.floatingTextParent = gObj;
        this.textToShow = text;
        this.colorOfText = colour;
    }

}

public interface IUIDescriptor
{

    string ObjectVisibleName { get; }
    string shortDescription { get; }
    string longDescription { get; }

    Sprite GetSprite();

}