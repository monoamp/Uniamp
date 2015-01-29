using System;

namespace Monoamp.Boundary
{
	public static class Logger
	{
		public delegate void Log( object message );
		public delegate void OutLogException( Exception exception );

		public static Log Normal;
		public static Log Warning;
		public static Log Error;
		public static Log Debug;
		public static OutLogException Exception;

		static Logger()
		{
			Normal = BreakNormal;
			Warning = BreakWarning;
			Error = BreakError;
			Debug = BreakDebug;
			Exception = UnityEngine.Debug.LogException;
			/*
			LogNormal = BreakNormal;
			LogWarning = BreakWarning;
			LogError = BreakError;
			LogDebug = BreakDebug;
			LogException = Debug.LogException;*/
		}

		public static void LogNull( object message )
		{

		}

		public static void LogNull( Exception exception )
		{

		}
		
		public static void BreakNormal( object message )
		{
			//UnityEngine.Debug.Log( message );
		}

		public static void BreakWarning( object message )
		{
			//UnityEngine.Debug.Log( "[Warning]:" + message );
		}

		public static void BreakError( object message )
		{
			UnityEngine.Debug.LogError( "[Error]:" + message );
		}

		public static void BreakDebug( object message )
		{
			//UnityEngine.Debug.Log( "[Debug]:" + message );
		}
	}
}
