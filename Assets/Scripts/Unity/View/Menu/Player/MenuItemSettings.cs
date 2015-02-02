using UnityEngine;

using Unity.Data;
using Unity.GuiStyle;

using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.View.Player
{
	public class MenuItemSettings : AMenuItem
	{
		private ApplicationPlayer applicationPlayer;
		private DialogSettings dialogSettings;
		private bool isShow;

		public MenuItemSettings( string aTitle, string aFilePathLanguage, ApplicationPlayer aApplicationPlayer )
			: base( aTitle )
		{
			Dictionary<string, string> lDictionaryDescription = ReadDictionaryLanguage( aFilePathLanguage );
			dialogSettings = new DialogSettings( ChangeSettings, lDictionaryDescription );
			applicationPlayer = aApplicationPlayer;
		}
		
		private void ChangeSettings( int a )
		{
			isShow = false;

			if( a == 1 )
			{
				applicationPlayer.SetIsLoop( false );
			}
			else
			{
				applicationPlayer.SetIsLoop( true );
			}
		}

		public override void Select()
		{
			isShow = true;
		}

		public override void OnGUI()
		{
			if( isShow == true )
			{
				dialogSettings.OnGUI();
			}
		}
	}
}
