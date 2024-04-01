#region Imports
using System.Text;
#endregion

namespace BlogPlatform.Helpers
{
    public class MiddlewareLoggerHelper
    {
        #region Properties
        private readonly RequestDelegate _nextAction;
        private readonly ILogger<MiddlewareLoggerHelper> _logger;
        #endregion

        #region Constructor
        public MiddlewareLoggerHelper(RequestDelegate nextAction,
        ILogger<MiddlewareLoggerHelper> logger)
        {
            _nextAction = nextAction;
            _logger = logger;
        }
        #endregion

        #region Public methods
        public async Task Invoke(HttpContext context)
        {
            await LogHttpRequest(context);

            await _nextAction(context);
            var req = context.Request;
            req.EnableBuffering();

            LogHttpResponse(context);
        }
        #endregion

        #region Private methods
        private async Task LogHttpRequest(HttpContext context)
        {          
            var request = context.Request;

            try
            {
                var bodyContent = string.Empty;

                if (request.Method != "GET")
                {
                    request.EnableBuffering();
                    var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                    await request.Body.ReadAsync(buffer, 0, buffer.Length);
                    bodyContent = Encoding.UTF8.GetString(buffer);

                    request.Body.Position = 0;
                }

                _logger.LogInformation(
                    $"{Environment.NewLine}" +
                    $"Request Time: {DateTime.UtcNow} {Environment.NewLine}" +
                    $"Http Header: {request.Method} {request.Path} {Environment.NewLine}" +
                    $"Host: {request.Host} {Environment.NewLine}" +
                    $"Content-Type: {request.ContentType} {Environment.NewLine}" +
                    $"Body: {Environment.NewLine}{bodyContent}{(string.IsNullOrEmpty(bodyContent) ? "" : Environment.NewLine)}");
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }
        }

        private void LogHttpResponse(HttpContext context)
        {
            var response = context.Response;

            try
            {
                _logger.LogInformation(
                    $"{Environment.NewLine}" +
                    $"Response Time: {DateTime.UtcNow} {Environment.NewLine}" +
                    $"Http Status Code: {response.StatusCode} {Environment.NewLine}");
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }
        }
        #endregion
    }
}
