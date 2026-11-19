using System.Reflection;
using UnityEngine;
using Oculus.Interaction;

public class RayInteractableAudit : MonoBehaviour
{
    System.Collections.IEnumerator Start()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1f);
            var all = FindObjectsByType<RayInteractable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            Debug.Log($"[AUDIT] pass {i}: {all.Length} RayInteractables");
            foreach (var r in all)
            {
                string line = $"[AUDIT] {GetPath(r.transform)} active={r.isActiveAndEnabled}";
                foreach (var f in typeof(RayInteractable).GetFields(
                             BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (!typeof(Object).IsAssignableFrom(f.FieldType) &&
                        !f.FieldType.IsInterface) continue;
                    var v = f.GetValue(r) as Object;
                    line += $" {f.Name}={(v == null ? "NULL" : v.GetType().Name)}";
                }
                Debug.Log(line, r);
            }
        }
    }

    static string GetPath(Transform t)
    {
        string p = t.name;
        while (t.parent != null) { t = t.parent; p = t.name + "/" + p; }
        return p;
    }
}