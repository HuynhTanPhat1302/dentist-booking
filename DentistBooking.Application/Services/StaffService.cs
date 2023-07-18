using DentistBooking.Application.Interfaces;
using DentistBooking.Infrastructure.Repositories;
using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DentistBooking.Application.Services
{
    public class StaffService : IStaffService
    {
        private readonly StaffRepository _StaffRepository;


        public StaffService(StaffRepository StaffRepository)
        {
            _StaffRepository = StaffRepository;
        }

        public IEnumerable<staff> GetAllStaffs()
        {
            return _StaffRepository.GetAll();
        }

        public staff GetStaffById(int id)
        {
            return _StaffRepository.GetById(id);
        }

        public void CreateStaff(staff Staff)
        {
            _StaffRepository.Add(Staff);
            _StaffRepository.SaveChanges();
        }

        public void UpdateStaff(staff Staff)
        {
            _StaffRepository.Update(Staff);
            _StaffRepository.SaveChanges();
        }

        public void DeleteStaff(int id)
        {
            var Staff = _StaffRepository.GetById(id);
            if (Staff != null)
            {
                _StaffRepository.Delete(Staff);
                _StaffRepository.SaveChanges();
            }
        }

        //check duplicated email
        public async Task<bool> IsEmailUnique(string email)
        {
            bool isUnique = false;

            if (!string.IsNullOrEmpty(email))
            {
                isUnique = await _StaffRepository.IsEmailUnique(email);
            }

            return isUnique;
        }

        public staff? GetStaffByEmail(string email)
        {
            return _StaffRepository.GetStaffByEmail(email);
        }
    }
}
