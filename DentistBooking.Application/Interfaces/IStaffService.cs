﻿using DentistBooking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBooking.Application.Interfaces
{
    public interface IStaffService
    {
        IEnumerable<staff> GetAllStaffs();
        staff GetStaffById(int id);
        void CreateStaff(staff staff);
        void UpdateStaff(staff staff);
        void DeleteStaff(int id);

        staff? GetStaffByEmail(string email);
    }
}
