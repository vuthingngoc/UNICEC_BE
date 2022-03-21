﻿using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.CompetitionEntityRepo
{
    public class CompetitionEntityRepo : Repository<CompetitionEntity>, ICompetitionEntityRepo
    {
        public CompetitionEntityRepo(UNICSContext context) : base(context)
        {

        }
    }
}
