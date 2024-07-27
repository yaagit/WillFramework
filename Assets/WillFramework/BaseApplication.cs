using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WillFramework.Tiers;

namespace WillFramework
{
    public abstract class BaseApplication : MonoBehaviour
    {
        protected abstract IContext Context { get; }
        
        void Awake()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            _Awake();
        }
        
        protected virtual void _Awake()
        {
            
        }
        
        private IView[] ScanViewsInTheScene()
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

            return viewList.ToArray();
        }

        void OnActiveSceneChanged(Scene arg1, Scene arg2)
        {
            IView[] views = ScanViewsInTheScene();
            Context.CommandContainer.Dispose();
            Context.IocContainer.Dispose();
            Context.StartWithViewsOnSceneLoading(views);
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            _OnDestroy();
        }

        protected virtual void _OnDestroy()
        {
            
        }
    }
}