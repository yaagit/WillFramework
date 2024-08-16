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
        
        private BaseView[] ScanViewsInTheScene()
        {
            //可以找到 inactive 状态的对象
            BaseView[] views = Resources.FindObjectsOfTypeAll<BaseView>();
            return views;
        }

        void OnActiveSceneChanged(Scene arg1, Scene arg2)
        {
            Context.CommandContainer.Dispose();
            Context.IocContainer.Dispose();
            BaseView[] views = ScanViewsInTheScene();
            Context.StartWithViewsOnSceneLoading(this, views);
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