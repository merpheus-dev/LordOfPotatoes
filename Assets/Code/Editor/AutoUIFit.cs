using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class AutoUIFit : MonoBehaviour
{
    [MenuItem("Tools/AutoFit/Anchor Rectangle #%&w")]
    static void Hizala()
    {
        RectTransform t = Selection.activeTransform as RectTransform;
        RectTransform pt = Selection.activeTransform.parent as RectTransform;

        if (t == null || pt == null) return;

        Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width, t.anchorMin.y + t.offsetMin.y / pt.rect.height);
        Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width, t.anchorMax.y + t.offsetMax.y / pt.rect.height);

        t.anchorMin = newAnchorsMin;
        t.anchorMax = newAnchorsMax;
        t.offsetMin = t.offsetMax = new Vector2(0, 0);

    }
    [MenuItem("Tools/AutoFit/Anchor Selections #%&e")]
    static void SecilenlerinHepsini()
    {
        Transform[] p = Selection.transforms;
        foreach (var n in p)
        {
            RectTransform t = n as RectTransform;
            RectTransform pt = n.parent as RectTransform;

            if (t == null || pt == null) return;

            Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width, t.anchorMin.y + t.offsetMin.y / pt.rect.height);
            Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width, t.anchorMax.y + t.offsetMax.y / pt.rect.height);

            t.anchorMin = newAnchorsMin;
            t.anchorMax = newAnchorsMax;
            t.offsetMin = t.offsetMax = new Vector2(0, 0);
        }
        Debug.Log("All Selections Fitted");
    }
    [MenuItem("Tools/AutoFit/Anchor All Objects #%&r")]
    static void ObjelerinHepsini()
    {
        GameObject[] p = FindGameObjectsWithLayer(5);
        foreach (var q in p)
        {
            Transform n = q.transform;
            RectTransform t = n as RectTransform;
            RectTransform pt = n.parent as RectTransform;

            if (t == null || pt == null) return;

            Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width, t.anchorMin.y + t.offsetMin.y / pt.rect.height);
            Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width, t.anchorMax.y + t.offsetMax.y / pt.rect.height);

            t.anchorMin = newAnchorsMin;
            t.anchorMax = newAnchorsMax;
            t.offsetMin = t.offsetMax = new Vector2(0, 0);
        }
        Debug.Log("All Objects Fitted");
    }
     [MenuItem("Tools/AutoFit/Text Namer %4")]
    static void Textleri_isimYap()
    {
        GameObject[] p = FindGameObjectsWithLayer(5);//UI Layerındaki tüm objeleri al
        GameObject[] q = Selection.gameObjects;
            foreach (var m in p)
            {
                foreach(var o in q){
                    if(m==o){ //ui layerının içinde seçilen objelerden var mı bak.tüm seçimleri tek tek döndür varsa onun için işlem yap
                            if (m.GetComponent<Text>())
                            {
                        m.name = m.GetComponent<Text>().text; // her text componentinin textini objenin ismi yap
                            }
                    }
                }
               
            }
     }
    
    public static GameObject[] FindGameObjectsWithLayer (int layer){ //Works with layerID(From Editor)
     var goArray = FindObjectsOfType<GameObject>(); // get all objects
     var goList = new System.Collections.Generic.List<GameObject>(); // create a list for type of gameobject
     for (var i = 0; i < goArray.Length; i++) {  //look every one of them
         if (goArray[i].layer == layer) { //if layer of a piece equals the layer that we want,
             goList.Add(goArray[i]); //assign it to list
         }
     }
     if (goList.Count == 0) { //if list is empty return null
         return null;
     }
     return goList.ToArray(); //otherwise return the list that we filled above
    }
}