using System;
using System.Reflection;
using UnityEngine;

namespace UnityExtended.Core.Extensions {
    /// <summary>
    /// Provides extensions to manage <see cref="GameObject"/>s.
    /// </summary>
    public static class GameObjectExtensions {
        /// <summary>
        /// Duplicates component onto another <see cref="GameObject"/>.
        /// </summary>
        /// <typeparam name="T">Type derived from <see cref="Component"/>.</typeparam>
        /// <param name="gameObj"><see cref="GameObject"/> on which a duplicate is attached.</param>
        /// <param name="duplicant">Component to duplicate.</param>
        /// <returns>Reference to a duplicate.</returns>
        public static T AddDuplicate<T>(this GameObject gameObj, T duplicant) where T : Component {
            T target = gameObj.AddComponent<T>();

            foreach (PropertyInfo x in typeof(T).GetProperties()) {
                if (x.CanWrite) {
                    x.SetValue(target, x.GetValue(duplicant));
                }
            }

            return target;
        }

        /// <summary>
        /// Adds duplicate of collider onto another <see cref="GameObject"/>.
        /// Use instead of <see cref="AddDuplicate{T}(GameObject, T)"/> for <see cref="Collider"/>s.
        /// </summary>
        /// <param name="gameObj"><see cref="GameObject"/> on which a duplicate is attached.</param>
        /// <param name="duplicant"><see cref="Collider"/> to duplicate.</param>
        /// <returns>Reference to a duplicate.</returns>
        public static Collider AddDuplicateCollider(this GameObject gameObj, Collider duplicant) {
            if (duplicant is BoxCollider box) return gameObj.AddDuplicate(box);
            else if (duplicant is SphereCollider sphere) return gameObj.AddDuplicate(sphere);
            else if (duplicant is MeshCollider mesh) return gameObj.AddDuplicate(mesh);
            else throw new NotImplementedException();
        }

        /// <summary>
        /// Sets layers of all <see cref="GameObject"/>'s children.
        /// </summary>
        /// <param name="gameObj"><see cref="GameObject"/> that contains all the children.</param>
        /// <param name="layer">Index of a layer in a layer mask (NOT A LAYER MASK ITSELF).</param>
        /// <param name="includeParent">Whether to set the layer of a <paramref name="gameObj"/>.</param>
        public static void SetChildrenLayers(this GameObject gameObj, int layer, bool includeParent = true) {
            if (includeParent) gameObj.layer = layer;

            foreach (Transform child in gameObj.transform) {
                child.gameObject.layer = layer;
            }
        }
    }
}