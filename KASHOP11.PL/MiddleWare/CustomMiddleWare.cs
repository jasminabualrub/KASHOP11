namespace KASHOP11.PL.MiddleWare
{ public static class CustomMiddleWareExtensions{
        public static IApplicationBuilder UsecUSTOMM(this IApplicationBuilder app ) {
            return app.UseMiddleware<CustomMiddleWare>();
        }
    }
    public class CustomMiddleWare
    {
        private readonly RequestDelegate _next;

        public CustomMiddleWare(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("processing request");
            await _next(context);
            Console.WriteLine("processing response");
        }
    }
}
