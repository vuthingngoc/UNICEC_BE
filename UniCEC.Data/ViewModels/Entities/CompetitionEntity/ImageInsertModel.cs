using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.ViewModels.Entities.Competition;

namespace UniCEC.Data.ViewModels.Entities.CompetitionEntity
{
    public class ImageInsertModel : CompetitionEntityInsertModel
    {
        public List<AddImageModel> Images { get; set; }
    }
}
