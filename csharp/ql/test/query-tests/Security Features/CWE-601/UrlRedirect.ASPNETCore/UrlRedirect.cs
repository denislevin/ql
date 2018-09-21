// semmle-extractor-options: /r:System.Private.Uri.dll

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace Testing.Controllers
{
    public class SomeController : ControllerBase
    {
        private static string SomeValue = "HeaderValue";

        [HttpPost]
        public void Post([FromBody] string value)
        {
            // BAD: straight up controller redirect
            Redirect(value);

            // BAD: Setting response headers collection, location = redirect
            Response.Headers["location"] = value;

            // GOOD: Setting response header to a constant value
            Response.Headers["location"] = SomeValue;

            // BAD: Setting response headers collection, location = redirect via add method
            Response.Headers.Add("location", value);

            // GOOD: Setting response header to a constant value
            Response.Headers.Add("location", "foo");

            // BAD: redirect via location
            Response.Headers.SetCommaSeparatedValues("location", value);

            // BAD = redirect via setting location value from tainted source
            Response.Headers.Append("location", value);

            // BAD: redirect via setting location header from comma-separated values
            Response.Headers.AppendCommaSeparatedValues("location", value);

            // BAD: tainted redirect to Action
            RedirectToActionPermanent("Error" + value);
        }

        // PUT: api/Some/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

            RedirectToPage(value);

            var headers = new ResponseHeaders(Response.Headers);

            // BAD: redirect via header helper class
            headers.Location = new Uri(value);

            // BAD: response redirect
            Response.Redirect(value);

            // GOOD: whitelisted redirect
            if(Url.IsLocalUrl(value))
                Redirect(value);
        }
    }
}
