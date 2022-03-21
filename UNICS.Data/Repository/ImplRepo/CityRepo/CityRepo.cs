﻿using UNICS.Data.Models.DB;
using UNICS.Data.Repository.GenericRepo;

namespace UNICS.Data.Repository.ImplRepo.CityRepo
{
    public class CityRepo : Repository<City>, ICityRepo
    {
        public CityRepo(UNICSContext context) : base(context)
        {

        }
    }
}
