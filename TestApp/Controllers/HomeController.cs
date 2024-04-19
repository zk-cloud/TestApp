using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using TestApp.Entity;
using TestApp.Filter;
using TestApp.Services;

namespace TestApp.Controllers
{
    public class HomeController : Controller
    {
        public UserServices userServices { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        [OAuthFilter]
        public JsonResult Login(User user)
        {
            string plaintext = "Hello, world!";
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

            IDigest digest = new SM3Digest();

            byte[] hashBytes = new byte[digest.GetDigestSize()];

            digest.BlockUpdate(plaintextBytes, 0, plaintextBytes.Length);
            digest.DoFinal(hashBytes, 0);

            string hashString = Hex.ToHexString(hashBytes);
            Console.WriteLine("SM3 Hash: " + hashString);

            Task.Factory.StartNew(() =>
            {
                var result = userServices.Login(user);
            });
            return Json(new { res = "OK" });
        }
        [System.Web.Http.HttpPost]
        public JsonResult Test2([FromBody]string obj)
        {
            var txt = "";
            var r = JsonConvert.DeserializeObject<JObject>(obj);
            IEnumerable<JProperty> properties = r.Properties();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach(var prop in properties)
            {
                dic[prop.Name] = prop.Value.Type.ToString() == "String" ? (string)prop.Value: prop.Value.ToString().ToLower();
            }
            string[] arrKeys = dic.Keys.ToArray();
            //Array.Sort(arrKeys, string.CompareOrdinal);
            foreach(var key in arrKeys)
            {
                if(key != "signature")
                {
                    txt += key + "=" + dic[key] + "&";
                }
            }
            txt = txt.Substring(0, txt.Length - 1).ToString();
            //txt = "bimRemotePwd=gbgdfgsdvxcg&bimRemoteUser=dtsDataSynName&bimRequestId=5286c8f3750d451d8c5a62bee93f5df8&__ENABLE__=false&bimUid=00950206";
            IDigest digest = new SM3Digest();
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(txt);

            byte[] hashBytes = new byte[digest.GetDigestSize()];

            digest.BlockUpdate(plaintextBytes, 0, plaintextBytes.Length);
            digest.DoFinal(hashBytes, 0);

            string hashString = Hex.ToHexString(hashBytes);
            Console.WriteLine("SM3 Hash: " + hashString);
            return Json(obj);
        }
    }
}