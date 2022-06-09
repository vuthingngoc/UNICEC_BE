﻿using System;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.SponsorRepo;
using UniCEC.Data.ViewModels.Common;
using UniCEC.Data.ViewModels.Entities.Sponsor;

namespace UniCEC.Business.Services.SponsorSvc
{
    public class SponsorService : ISponsorService
    {
        private ISponsorRepo _sponsorRepo;

        public SponsorService(ISponsorRepo sponsorRepo)
        {
            _sponsorRepo = sponsorRepo;
        }



        public Task<PagingResult<ViewSponsor>> GetAllPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }

        //Role Admin
        public async Task<ViewSponsor> GetBySponsorId(int id)
        {
            try
            {

                Sponsor sp = await _sponsorRepo.Get(id);
                if (sp != null)
                {
                    return TransformViewModel(sp);
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Role Admin
        public async Task<bool> Delete(int id)
        {
            try
            {                    
                //
                Sponsor sp = await _sponsorRepo.Get(id);
                //
                if (sp != null)
                {
                    sp.Status = false;

                    await _sponsorRepo.Update();
                    return true;
                }            
                else
                {
                    throw new ArgumentException(" Sponsor not found");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Admin Insert
        public async Task<ViewSponsor> Insert(SponsorInsertModel sponsor)
        {
            try
            {
                bool checkSponsorIsCreated = await _sponsorRepo.CheckSponsorIsCreated(sponsor.Email);
                if (checkSponsorIsCreated == false)
                {
                    Sponsor sp = new Sponsor()
                    {

                        Address = sponsor.Address,
                        Description = sponsor.Description,
                        Email = sponsor.Email,
                        Logo = sponsor.Logo,
                        Name = sponsor.Name,
                        Phone = sponsor.Phone,
                        Status = sponsor.Status,
                    };

                    int id = await _sponsorRepo.Insert(sp);
                    if (id > 0)
                    {
                        Sponsor sp1 = await _sponsorRepo.Get(id);
                        return TransformViewModel(sp1);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Role Admin
        public async Task<bool> Update(SponsorUpdateModel sponsor)
        {
            try
            {
                if (sponsor.Id == 0)
                    throw new ArgumentNullException("Sponsor Id Null");

                //
                Sponsor sp = await _sponsorRepo.Get(sponsor.Id);
                //
                if (sp != null)
                {
                    sp.Name = (!string.IsNullOrEmpty(sponsor.Name)) ? sponsor.Name : sp.Name;
                    sp.Description = (!string.IsNullOrEmpty(sponsor.Description)) ? sponsor.Description : sp.Description;
                    sp.Logo = (!string.IsNullOrEmpty(sponsor.Logo)) ? sponsor.Logo : sp.Logo;
                    sp.Phone = (!string.IsNullOrEmpty(sponsor.Phone)) ? sponsor.Phone : sp.Phone;
                    sp.Address = (!string.IsNullOrEmpty(sponsor.Address)) ? sponsor.Address : sp.Address;

                    await _sponsorRepo.Update();
                    return true;
                }
                else
                {
                    throw new ArgumentException(" Sponsor not found");
                }
              
            }
            catch (Exception)
            {
                throw;
            }
        }


        private ViewSponsor TransformViewModel(Sponsor sponsor)
        {
            return new ViewSponsor()
            {
                Id = sponsor.Id,
                Address = sponsor.Address,
                Description = sponsor.Description,
                Email = sponsor.Email,
                Logo = sponsor.Logo,
                Phone = sponsor.Phone,
                Name = sponsor.Name,
                Status = sponsor.Status,
            };
        }
    }
}
