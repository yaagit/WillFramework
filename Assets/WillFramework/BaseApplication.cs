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
        
        private BaseView[] ScanViewsInTheScene()
        {
            //可以找到 inactive 状态的对象
            BaseView[] views = Resources.FindObjectsOfTypeAll<BaseView>();
            return views;
        }

        void OnActiveSceneChanged(Scene arg1, Scene arg2)
        {
            BaseView[] views = ScanViewsInTheScene();
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