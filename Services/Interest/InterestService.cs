﻿using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using DataAccess;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.Interest
{
    public class InterestService : Base.BaseService
    {
        public InterestService(SocializRUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Domain.Interest GetInterest(int id)
        {
            return unitOfWork.Interests.Query.FirstOrDefault(e => e.Id == id);
        }
        public void EditInterest(int id,string name)
        {
            var interest = GetInterest(id);
            interest.Name = name;
            unitOfWork.Interests.Update(interest);
            unitOfWork.SaveChanges();
        }
        public bool AddInterest(string name)
        {
            if (unitOfWork.Interests.Query.Any(e => e.Name == name)) return false;
            var interest = new Domain.Interest()
            {
                Name = name
            };
            unitOfWork.Interests.Add(interest);
            return unitOfWork.SaveChanges() != 0;
        }

        public bool RemoveInterest(int interestId)
        {
            unitOfWork.InterestsUserss.RemoveRange(unitOfWork.InterestsUserss.Query.Where(e => e.InterestId == interestId).ToList());
            unitOfWork.Interests.Remove(GetInterest(interestId));
            return unitOfWork.SaveChanges() != 0;
        }

        public List<Domain.Interest> getAll()
        {
            return unitOfWork.Interests.Query.AsNoTracking().ToList();
        }

        public List<Domain.Interest> GetInterests(int toSkip,int howMany)
        {
            return unitOfWork.Interests.Query.OrderBy(e => e.Id).Skip(toSkip).Take(howMany).AsNoTracking().ToList();
        }

        public List<SelectListItem> GetAllSelectListItems(int userId)
        {
            var indexi = unitOfWork.InterestsUserss.Query.Where(e => e.UserId == userId).Select(e => e.InterestId).ToList();
            var interests = getAll().Select(e =>
            {
                var item = new SelectListItem();
                item.Text = e.Name;
                item.Value = e.Id.ToString();
                item.Selected = indexi.Contains(e.Id);
                return item;

            }).ToList();
            return interests;

        }
    }
}
