using System.Collections.Generic;
using UniCEC.Data.ViewModels.Entities.University;

namespace UniCEC.Data.ViewModels.Firebase.Auth
{
    public class ViewUserInfo
    {
        //TokenTemp
        public string Token { get; set; }
        //
        public List<ViewUniversity> ListUniBelongToEmail { get; set; }
    }
}
