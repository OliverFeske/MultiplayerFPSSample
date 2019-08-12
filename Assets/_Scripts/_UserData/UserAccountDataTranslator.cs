using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiFPS
{
	public class UserAccountDataTranslator : MonoBehaviour
	{
		private static string KILLS_SYMBOL = "[KILLS]";
		private static string DEATHS_SYMBOL = "[DEATHS]";

		public static string ValuesToData(int _kills, int _deaths)
		{
			return KILLS_SYMBOL + _kills + "/" + DEATHS_SYMBOL + _deaths;
		}

		public static int DataToKills(string data)
		{
			return int.Parse(DataToValue(data, KILLS_SYMBOL));
		}

		public static int DataToDeaths (string data)
		{
			return int.Parse(DataToValue(data, DEATHS_SYMBOL));
		}

		public static string DataToValue (string data, string symbol)
		{
			string[] dataPieces = data.Split('/');
			foreach (string piece in dataPieces)
			{
				if (piece.StartsWith(symbol))
				{
					return piece.Substring(symbol.Length); //skips [KILLS] and just takes the 0 after it
				}
			}

			Debug.Log(symbol + "not found in " + data);
			return "";
		}
	}
}
