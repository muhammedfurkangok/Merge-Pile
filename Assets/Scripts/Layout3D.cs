using System;
using System.Collections.Generic;
using Runtime;
using UnityEngine;
using UnityEngine.Serialization;


public enum LayoutDirection
{
    x,
    y,
    z
    
}
public class Layout3D : MonoBehaviour
{
    public List<Layout3DElement> layoutElements = new List<Layout3DElement>();
    public LayoutDirection axis;
    public float spacing;
    public bool reverse;
    public Vector3 startPosition;
    public Vector3 defaultRotation;

    public void RebuildLayout()
    {
        PopulateElementsFromChildren();

        LinearLayout();
    }

    private void LinearLayout()
    {
        Vector3 nextPosition = Vector3.zero;
        Vector3 pos = Vector3.zero;
        List<Layout3DElement> activeChilds = layoutElements.FindAll(item => item.gameObject.activeSelf);
        
        for(int i = 0; i< activeChilds.Count; i++)
        {
            Vector3  dimensions = activeChilds[i].dimension;
            pos  = Vector3.zero - activeChilds[i].centerOffset;
            switch (axis)
            {
                case LayoutDirection.x:
                    nextPosition.x = i > 0 ? nextPosition.x + dimensions.x / 2 : nextPosition.x;
                    pos.x =(reverse ? -1 : 1) * nextPosition.x - activeChilds[i].centerOffset.x;
                    nextPosition.x += dimensions.x / 2 + spacing;
                    break;
                case LayoutDirection.y:
                    nextPosition.y = i > 0 ? nextPosition.y + dimensions.y / 2 : nextPosition.y;
                    pos.y = (reverse ? -1 : 1) * nextPosition.y - activeChilds[i].centerOffset.y;
                    nextPosition.y += dimensions.y / 2 + spacing;
                    break;
                case LayoutDirection.z:
                    nextPosition.z = i > 0 ? nextPosition.z + dimensions.z / 2 : nextPosition.z;
                    pos.z = (reverse ? -1 : 1) * nextPosition.z - activeChilds[i].centerOffset.z;
                    nextPosition.z += dimensions.z / 2 + spacing;
                    break;
            }
            activeChilds[i].localRotation = Quaternion.Euler(defaultRotation + activeChilds[i].rotationOffset);
            activeChilds[i].localPosition = pos + startPosition;
        }
    }

    private void PopulateElementsFromChildren()
    {
        if (layoutElements == null)
        {
            layoutElements = new List<Layout3DElement>();
        }
        layoutElements.Clear();

        foreach (Transform child in transform)
        {
            Layout3DElement layoutElement = child.GetComponent<Layout3DElement>();
            if (layoutElement != null)
            {
                layoutElements.Add(layoutElement);
            }
        }
    }
    
    public void ClearElements()
    {
        layoutElements.Clear();
    }

    public Vector3 GetNextOffset()
    {
        return startPosition;
    }
    
    public Vector3 GetNextOffset(Layout3DElement element)
    {
        return startPosition;
    }

    public Vector3 GetNextRotation()
    {
        return defaultRotation;
    }

    private void OnValidate()
    {
        RebuildLayout();
    }
}