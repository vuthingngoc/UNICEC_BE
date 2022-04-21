using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Entities.University;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.Data.ViewModels.Firebase.Auth
{
    public class ViewUserInfo
    {
        //TokenTemp
        public string tokenTemp { get; set; }
        //
        public List<ViewUniversity> listUniBelongToEmail { get; set; }
    }
}
