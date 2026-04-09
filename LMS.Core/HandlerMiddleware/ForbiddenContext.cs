namespace LMS.Core.HandlerMiddleware
{
    public class ForbiddenContext : Exception
    {
        public ForbiddenContext(string message) : base(message) { }
    }
}
