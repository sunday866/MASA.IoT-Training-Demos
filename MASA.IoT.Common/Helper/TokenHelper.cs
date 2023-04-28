using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASA.IoT.Common.Helper
{
    public static class TokenHelper
    {

        /// <summary>
        /// 创建token
        /// </summary>
        /// <returns></returns>
        public static string CreateJwtToken(IDictionary<string, object> payload, string secret, IDictionary<string, object> extraHeaders = null)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, secret);
            return token;
        }
    }
}
