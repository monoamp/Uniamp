using UnityEngine;

using Unity.GuiStyle;

namespace Unity.Data
{
	public static class GuiStyleSet
	{
		public static StyleFolder StyleFolder{ get; private set; }
		public static StyleGeneral StyleGeneral{ get; private set; }
		public static StyleList StyleList{ get; private set; }
		public static StyleLoopTool StyleLoopTool{ get; private set; }
		public static StyleMenu StyleMenu{ get; private set; }
		public static StylePlayer StylePlayer{ get; private set; }
		public static StyleProgressbar StyleProgressbar{ get; private set; }
		public static StyleScrollbar StyleScrollbar{ get; private set; }
		public static StyleSlider StyleSlider{ get; private set; }
		public static StyleTable StyleTable{ get; private set; }
		public static StyleWindow StyleWindow{ get; private set; }

		static GuiStyleSet()
		{
			GameObject obj = ( GameObject )Resources.Load( "Prefab/GuiStyleSet", typeof( GameObject ) );

			StyleFolder = obj.GetComponent<StyleFolder>();
			StyleGeneral = obj.GetComponent<StyleGeneral>();
			StyleList = obj.GetComponent<StyleList>();
			StyleLoopTool = obj.GetComponent<StyleLoopTool>();
			StyleMenu = obj.GetComponent<StyleMenu>();
			StylePlayer = obj.GetComponent<StylePlayer>();
			StyleProgressbar = obj.GetComponent<StyleProgressbar>();
			StyleScrollbar = obj.GetComponent<StyleScrollbar>();
			StyleSlider = obj.GetComponent<StyleSlider>();
			StyleTable = obj.GetComponent<StyleTable>();
			StyleWindow = obj.GetComponent<StyleWindow>();
		}
		
		public static void Reset( GameObject aObj )
		{
			StyleFolder = aObj.GetComponent<StyleFolder>();
			StyleGeneral = aObj.GetComponent<StyleGeneral>();
			StyleList = aObj.GetComponent<StyleList>();
			StyleLoopTool = aObj.GetComponent<StyleLoopTool>();
			StyleMenu = aObj.GetComponent<StyleMenu>();
			StylePlayer = aObj.GetComponent<StylePlayer>();
			StyleProgressbar = aObj.GetComponent<StyleProgressbar>();
			StyleScrollbar = aObj.GetComponent<StyleScrollbar>();
			StyleSlider = aObj.GetComponent<StyleSlider>();
			StyleTable = aObj.GetComponent<StyleTable>();
			StyleWindow = aObj.GetComponent<StyleWindow>();
		}
	}
}
