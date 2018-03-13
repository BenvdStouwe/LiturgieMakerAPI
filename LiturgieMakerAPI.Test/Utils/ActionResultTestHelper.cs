using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LiturgieMakerAPI.Test.Utils
{
    public static class ActionResultTestHelper
    {
        public static void AssertBadRequest(IActionResult result, string message = null)
        {
            AssertObjectResult(result, 400, message);
        }

        public static void AssertNotFound(IActionResult result, string message = null)
        {
            AssertObjectResult(result, 404, message);
        }

        public static void AssertUnauthorized(IActionResult result)
        {
            AssertObjectResult(result, 401);
        }

        public static void AssertOk(IActionResult result, object value)
        {
            AssertObjectResult(result, 200, value);
        }

        public static void AssertNoContent(IActionResult result)
        {
            AssertObjectResult(result, 204);
        }

        private static void AssertObjectResult(IActionResult result, int statusCode, object value = null)
        {
            Assert.NotNull(result);
            if (result is StatusCodeResult statusResult)
            {
                Assert.Equal(statusCode, statusResult.StatusCode);
            }
            else if (result is ObjectResult objectResult)
            {
                Assert.Equal(statusCode, objectResult.StatusCode);
                if (result is ObjectResult && value != null)
                {
                    Assert.Equal(value, objectResult.Value);
                }
            }
        }
    }
}