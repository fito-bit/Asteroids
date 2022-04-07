using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int count;
    List<GameObject> objects=new List<GameObject>();

    private void Start()
    {
        Initialize();
    }

    public void DeactivateObjects()
    {
        foreach (var obj in objects)
        {
            obj.gameObject.SetActive(false);
        }
    }
    
    public GameObject GetObject() {
        for (int i=0; i<objects.Count; i++) {
            if (objects[i].gameObject.activeInHierarchy==false) {
                return objects[i];
            }
        }
        CreateObj();
        return objects[objects.Count-1];
    }

    void Initialize()
    {
        for (int i = 0; i < count; i++)
        {
            CreateObj();
        }
    }

    void CreateObj()
    {
        GameObject obj = Instantiate(prefab,transform.position,Quaternion.identity);
        obj.transform.SetParent(transform);
        objects.Add(obj);
        obj.SetActive(false);
    }
}
