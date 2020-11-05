using System;
namespace IdentityServer.Models
{
    public static class Scopes
    {
        public static class Article
        {
            public const string Read = "article.read";
            public const string Write = "article.write";
        }

        public static class Identity
        {
            public const string Read = "identity.read";
        }
    }
}
