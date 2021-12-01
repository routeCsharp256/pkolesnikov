using OpenTracing;

namespace OzonEdu.MerchandiseApi.Infrastructure.Tracers
{
    // TODO Использование tracer попробовать вынести в атрибут (для уменьшения количества кода)
    public class CustomTracer
    {
        private readonly ITracer _tracer;

        public CustomTracer(ITracer tracer)
        {
            _tracer = tracer;
        }

        public IScope? GetSpan(string className, string method)
        {
            return _tracer
                .BuildSpan($"{className}.{method}")
                .StartActive();
        }

        public IScope? GetSpan(string className, string method, (string key, string value) tag)
        {
            var (key, value) = tag;
            return _tracer
                .BuildSpan($"{className}.{method}")
                .WithTag(key, value)
                .StartActive();
        }
    }
}