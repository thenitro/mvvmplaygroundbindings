using System.Collections.Generic;
using MVVM.Bindings.Base;
using MVVM.Models;
using UnityEngine;
using UnityEngine.UI;

namespace MVVM.Bindings {
    public class LookupImageBinding : BaseValueBinding<string> {
        private readonly Image _image;
        private readonly Sprite _defaultSprite;
        private readonly Dictionary<string, Sprite> _lookupTable = new();

        public LookupImageBinding(IObservableValue<string> observableValue, Sprite defaultSprite, LookupImagePair[] lookupTable, Image image) : base(observableValue) {
            _image = image;
            _defaultSprite = defaultSprite;
            foreach (LookupImagePair lookupPair in lookupTable) {
                _lookupTable.Add(lookupPair.name, lookupPair.sprite);
            }
        }

        public LookupImageBinding(string url, Sprite defaultSprite, LookupImagePair[] lookupTable, Image image):base(url) {
            _image = image;
            _defaultSprite = defaultSprite;
            foreach (LookupImagePair lookupPair in lookupTable) {
                _lookupTable.Add(lookupPair.name, lookupPair.sprite);
            }
        }
        
        protected override void OnUpdate(string value) {
            if (!string.IsNullOrEmpty(value) && _lookupTable.TryGetValue(value, out Sprite lookupResult)) {
                _image.sprite = lookupResult; 
                return;
            }

            _image.sprite = _defaultSprite;
        }
    }

    [System.Serializable]
    public class LookupImagePair {
        public string name;
        public Sprite sprite;
    }
}
