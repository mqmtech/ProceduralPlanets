using UnityEngine;
using System.Collections;

public abstract class Singleton<T> where T : class, new()
{
	static T _instance = default(T);
	public static T Instance
	{
		get
		{
			if(_instance == default(T))
			{
				_instance = new T();
			}
			return _instance;
		}
	}
}

public abstract class MonoBehaviorSingleton<T> : MonoBehaviour where T : MonoBehaviour, new()
{
	static T _instance = default(T);
	public static T Instance
	{
		get
		{
			if(_instance == default(T))
			{
				_instance = FindObjectOfType<T>();
				if(_instance == default(T))
				{
					GameObject go = new GameObject();
					go.name = typeof(T).ToString();
					_instance = go.AddComponent<T>();
				}
			}
			
			return _instance;
		}
	}
}
