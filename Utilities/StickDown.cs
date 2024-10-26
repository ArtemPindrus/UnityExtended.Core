using System.Collections;
using UnityEngine;

namespace UnityExtended.Core.Utilities {
	/// <summary>
	/// In-editor tool to stick an object (with colliders) to the ground.
	/// </summary>
	public class StickDown : MonoBehaviour {
#if UNITY_EDITOR
		private void Reset() {
			StartCoroutine(Impl());

			IEnumerator Impl() {
				yield return new WaitForSecondsRealtime(0.01f);
				Stick();
			}
		}

		[ContextMenu(nameof(Stick))]
		private void Stick() {
			if (UnityEngine.Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitDown)) {
				if (UnityEngine.Physics.Raycast(hitDown.point, Vector3.up, out RaycastHit traced)) {
					Vector3 difference = transform.position - traced.point;

					transform.position = hitDown.point + difference;
				}
			}

			DestroyImmediate(this);
		}
#endif
	}
}
