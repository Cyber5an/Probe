namespace GitPerformanceMonitor
{
	public class ExceptionInfo
	{
		public string Message { get; private set; } = "";
		//public string Stack { get; private set; } = "";
		public ExceptionInfo? InnerExceptionInfo { get; private set; }

		public static ExceptionInfo? FromException(Exception? exception)
		{
			if (exception == null) return null;
			var exceptionInfo = new ExceptionInfo
			{
				Message = exception.Message,
				//Stack = exception.StackTrace ?? "",
				InnerExceptionInfo = FromException(exception.InnerException)
			};
			return exceptionInfo;
		}
	}
}