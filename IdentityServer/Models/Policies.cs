using System;
namespace IdentityServer.Models
{
    public static class Policies
    {
        public static class Article
        {
            public const string ReadOnly = "article.read";
            public const string FullAccess = "article.write";
        }

        public static class Identity
        {
            public const string ReadOnly = "identity.read";
        }
    }
}
