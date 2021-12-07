using System;
using UnityEngine;

namespace Other {
    /// <summary>
    /// Defines a singleton component, or a component that can only exist once. Usually used for manager classes, it
    /// also defines a public getter and lazy initializer for this component, so it can be accessed anywhere without
    /// <c>FindObjectsOfType();</c>
    /// </summary>
    /// <typeparam name="T">Will most likely be the type of the inheritor. Defines what type the singleton is.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// A lazy, memoized instance of the singleton component.
        /// </summary>
        private static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateSingleton);

        public static T Instance => LazyInstance.Value;

        /// <summary>
        /// Initializes the singleton component, assigning it to the LazyInstance field and
        /// creating a new DontDestroyOnLoad GameObject to add it to.
        /// </summary>
        /// <returns>The instance of the singleton object to be used</returns>
        private static T CreateSingleton()
        {
            var ownerObject = new GameObject($"{typeof(T).Name} (singleton)");
            var instance    = ownerObject.AddComponent<T>();
            DontDestroyOnLoad(ownerObject);
            return instance;
        }
    }
}

