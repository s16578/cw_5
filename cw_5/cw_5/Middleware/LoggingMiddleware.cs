using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace cw_5.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            if(context.Request != null)
            {
                string method = context.Request.Method;
                string path = context.Request.Path;
                string type = context.Request.ContentType;
                string queryString = context.Request.QueryString.ToString();
                string time = DateTime.Now.ToString();
                string bodyString = "";

                using(var reader = new StreamReader(context.Request.Body,
                    Encoding.UTF8, true, 1024, true))
                {
                    
                    bodyString = await reader.ReadToEndAsync();
                    bodyString = bodyString
                        + Environment.NewLine + "method: " + method
                        + Environment.NewLine + "path: " + path
                        + Environment.NewLine + "type: " + type
                        + Environment.NewLine + "time: " + time
                        + Environment.NewLine + "queryString: " + queryString;
                    context.Request.Body.Position = 0;

                    string filePath = System.IO.Directory.GetCurrentDirectory() + "\\requestLog.txt";

                    using(StreamWriter sr = File.AppendText(filePath))
                    {
                        sr.WriteLine(bodyString);
                        sr.Close();
                    }
                }
            }

            if(_next!=null)
            await _next(context);
        }
    }
}
