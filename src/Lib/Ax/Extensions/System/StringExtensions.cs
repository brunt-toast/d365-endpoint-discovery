namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.Lib.Ax.Extensions.System;

internal static class StringExtensions
{
    extension(string source)
    {
        public Stream ToStream()
        {
            var ret = new MemoryStream();
            var writer = new StreamWriter(ret);
            writer.Write(source);
            writer.Flush();
            ret.Position = 0;
            return ret;
        }
    }
}
