using UnityEngine;
using System.Collections;
using System.Linq;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	protected static T instance;

	//Returns the instance of this singleton
	public static T Instance {
		get {
			if (instance != null)
				return instance;

			instance = (T)FindObjectOfType(typeof(T));

			if (instance == null) {
				Debug.LogError(string.Format("An instance of {0} is needed in the scene, but there is none",
					typeof(T)));
			}

			return instance;
		}
	}
}
