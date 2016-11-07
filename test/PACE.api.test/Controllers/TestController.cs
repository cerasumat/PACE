
using System.Web.Http;
using PACE.api.test.Models;
using PACE.client.Aop;

namespace PACE.api.test.Controllers
{
	[PACE.client.Aop.PaceFilter]
    public class TestController : ApiController
    {
		[HttpGet]
		[ActionName("version")]
		public IHttpActionResult Version()
        {
	        var biz = new TestBiz();
	        var version = biz.GetVersion();
			return Ok(version);
        }

		[HttpPost]
		[PaceIgnore]
		public IHttpActionResult AddVersion()
		{
			var biz = new TestBiz();
			var version = biz.GetVersion();
			return Ok(version);
		}
    }
}
