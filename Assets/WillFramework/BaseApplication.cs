using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using WillFramework.Context;
using WillFramework.Tiers;

namespace WillFramework
{
    public class BaseApplication : MonoBehaviour
    {
        protected IContext Context { get => WillFramework.Context.Context.Instance; }

        void Awake()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            _Awake();
        }
        
        protected virtual void _Awake()
        {
            
        }
        
        private BaseView[] ScanViewsInTheScene(out Assembly localAssembly)
        {
            //可以找到 inactive 状态的对象
            BaseView[] views = Resources.FindObjectsOfTypeAll<BaseView>();
            localAssembly = GetType().Assembly;
            List<BaseView> resultViewList = new();
            //Resources.FindObjectsOfTypeAll 会找到上一个场景的预制件,因此要过滤掉
            foreach (var view in views)
            {
                Assembly viewAssembly = view.GetType().Assembly;
                if (viewAssembly == localAssembly)
                {
                    resultViewList.Add(view);
                }
            }
            return resultViewList.ToArray();
        }

        void OnActiveSceneChanged(Scene arg1, Scene arg2)
        {
            Context.CommandContainer.Dispose();
            Context.IocContainer.Dispose();
            BaseView[] views = ScanViewsInTheScene(out Assembly localAssembly);
            Context.StartWithViewsOnSceneLoading(localAssembly, views);
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