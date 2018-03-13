using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LiturgieMakerAPI.Test.Utils
{
    public static class ActionResultTestHelper
    {
        public static void AssertBadRequest(ObjectResult result, string message = null)
        {
            AssertObjectResult(result, 400, message);
        }

        public static void AssertNotFound(ObjectResult result, string message = null)
        {
            AssertObjectResult(result, 404, message);
        }

        public static void AssertUnauthorized(ObjectResult result, string message = null)
        {
            AssertObjectResult(result, 403, message);
        }

        private static void AssertObjectResult(ObjectResult result, int statusCode, string message = null)
        {
            Assert.NotNull(result);
            Assert.Equal(statusCode, result.StatusCode);
            if (message != null)
            {
                Assert.Equal(message, result.Value);
            }
        }
    }
}