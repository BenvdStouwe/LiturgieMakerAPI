using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LiturgieMakerAPI.Test.Utils
{
    public static class ActionResultTestHelper
    {
        public static void AssertOk(IActionResult result, object value)
        {
            AssertIActionResult(result, 200, value);
        }

        public static void AssertNoContent(IActionResult result)
        {
            AssertIActionResult(result, 204);
        }
        public static void AssertBadRequest(IActionResult result, object value = null)
        {
            AssertIActionResult(result, 400, value);
        }

        public static void AssertUnauthorized(IActionResult result)
        {
            AssertIActionResult(result, 401);
        }

        public static void AssertNotFound(IActionResult result, object value = null)
        {
            AssertIActionResult(result, 404, value);
        }

        private static void AssertIActionResult(IActionResult result, int statuscode, object value = null)
        {
            if (result is StatusCodeResult)
            {
                AssertStatusCodeResult(result, statuscode);
            }
            else if (result is ObjectResult)
            {
                AssertObjectResult(result, statuscode, value);
            }
        }

        private static void AssertStatusCodeResult(IActionResult result, int statusCode)
        {
            var statusResult = result as StatusCodeResult;
            Assert.NotNull(statusResult);
            Assert.Equal(statusCode, statusResult.StatusCode);
        }

        private static void AssertObjectResult(IActionResult result, int statusCode, object value)
        {
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            // Extra check of de value not null is, omdat dat het hele nut van een ObjectResult is
            Assert.NotNull(objectResult.Value);
            Assert.Equal(statusCode, objectResult.StatusCode);
            Assert.Equal(value, objectResult.Value);
        }
    }
}