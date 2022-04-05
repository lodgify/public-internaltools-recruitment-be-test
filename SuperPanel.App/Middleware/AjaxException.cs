namespace SuperPanel.App.Middleware
{
    internal class AjaxException
    {
        public AjaxException(int statusCode, int errorCode, string message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
            Message = message;
        }


        public int StatusCode { get; private set; }
        public int ErrorCode { get; private set; }
        public string Message { get; private set; }
    }

}
