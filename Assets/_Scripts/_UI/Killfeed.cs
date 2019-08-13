using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiFPS
{
	public class Killfeed : MonoBehaviour
	{
		[SerializeField] private GameObject killfeedItemPrefab;

		void Start()
		{
			GameManager.instance.onPlayerKilledCallback += OnKill;
		}

		public void OnKill(string _player, string _source)
		{
			GameObject go = (GameObject)Instantiate(killfeedItemPrefab, this.transform);
			go.GetComponent<KillfeedItem>().Setup(_player, _source);

			Destroy(go, 5f);
		}
	}
}
