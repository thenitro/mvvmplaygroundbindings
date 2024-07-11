using System.Collections.Generic;
using System.Linq;
using MVVM.Bindings.Base;
using MVVM.Models;
using MVVM.ViewModels;
using MVVM.Views;
using UnityEngine;

namespace MVVM.Bindings
{
    public class ListBinding<TView, TViewModel> : BaseValueBinding<IEnumerable<TViewModel>> 
        where TViewModel : BaseViewModel
        where TView : BaseView<TViewModel>
    {
        private readonly Transform _container;
        private readonly TView _prefab;
        
        private HashSet<TView> _instantiatedViews; 

        public ListBinding(TView prefab, Transform container, IObservableValue<IEnumerable<TViewModel>> observableValue, IEnumerable<TView> instantiatedViews = null) : base(observableValue)
        {
            _instantiatedViews = instantiatedViews == null ? new HashSet<TView>() : new HashSet<TView>(instantiatedViews);
            _container = container;
            _prefab = prefab;
        }
        
        public ListBinding(TView prefab, Transform container, IEnumerable<TViewModel> observableValue, IEnumerable<TView> instantiatedViews = null) : base(observableValue)
        {
            _instantiatedViews = instantiatedViews == null ? new HashSet<TView>() : new HashSet<TView>(instantiatedViews);
            _container = container;
            _prefab = prefab;
        }
        
        protected override void OnUpdate(IEnumerable<TViewModel> value)
        {
            ManageInstantiatedViews(value);
        }

        private TView Instantiate() => Object.Instantiate(_prefab, _container);

        private void ManageInstantiatedViews(IEnumerable<TViewModel> value)
        {
            var max = Mathf.Max(_instantiatedViews.Count, value.Count());

            var viewsEnumerator = _instantiatedViews.ToList().GetEnumerator();
            var viewModelsEnumerator = value.GetEnumerator();

            while (max > 0)
            {
                var view = viewsEnumerator.MoveNext() ? viewsEnumerator.Current : null;
                var viewModel = viewModelsEnumerator.MoveNext() ? viewModelsEnumerator.Current : null;

                if (view == null)
                {
                    view = Instantiate();
                    _instantiatedViews.Add(view);
                }

                if (viewModel == null)
                {
                    view.gameObject.SetActive(false);
                }
                else
                {
                    view.Setup(viewModel);
                }

                max--;
            }
            
            viewsEnumerator.Dispose();
            viewModelsEnumerator.Dispose();
        }
    }
}