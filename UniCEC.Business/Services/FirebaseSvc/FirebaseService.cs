using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Business.Services.UserSvc;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.Business.Services.FirebaseSvc
{
    public class FirebaseService : IFirebaseService
    {
        private IUserService _userService;
        public FirebaseService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ViewUser> AuthenUser(HttpRequest request)
        {
            var header = request.Headers;
            string idToken = "";
            if (header.ContainsKey("Authorization"))
            {
                string tempId = header["Authorization"].ToString();
                idToken = tempId.Split(" ")[1];
            }

            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
            FirebaseToken decodedToken = await auth.VerifyIdTokenAsync(idToken);
            IReadOnlyDictionary<string, dynamic> info = decodedToken.Claims;
            //ViewUser user = _userService;

            

            throw new NotImplementedException();
        }
    }
}
