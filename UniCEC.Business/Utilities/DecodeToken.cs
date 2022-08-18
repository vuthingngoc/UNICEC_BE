using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace UniCEC.Business.Utilities
{
    public class DecodeToken
    {
        private JwtSecurityTokenHandler _tokenHandler;

        public DecodeToken()
        {
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public int Decode(string token, string nameClaim)
        {
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
            return Int32.Parse(claim.Value);
        }

        public string DecodeText(string token, string nameClaim)
        {
            var claim = _tokenHandler.ReadJwtToken(token).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));
            return claim.Value;
        }
    }
}
