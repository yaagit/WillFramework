using System.Collections.Generic;
using UnityEngine;
using WillFramework.Tiers;

namespace WillFramework
{
    public class BaseApplication : MonoBehaviour
    {
        public void StartWithContext(IContext context)
        {
            MonoBehaviour[] scripts = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
            List<IView> viewList = new();
            foreach (var s in scripts)
            {
                if (s is IView view)
                {
                    viewList.Add(view);
                }
            }
            context.StartWithViews(viewList.ToArray());
        }
    }
}