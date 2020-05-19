using System;
using System.Linq;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class DbServices : IDbServices
    {

        private readonly ClinicDbContext _context;

        public DbServices(ClinicDbContext clinicDbContext)
        {
            _context = clinicDbContext;
        }
        public DoctorResponse GetDoctor(int id)
        {
            var res = (from doc in _context.Doctor where doc.IdDoctor == id select new DoctorResponse
            {
                Email = doc.Email,
                FirstName = doc.FirstName,
                LastName = doc.LastName
            }).FirstOrDefault();
            if(res == null)
            {
                throw new Exception();
            }

            return res;
        }

        public bool AddDoctor(DoctorResponse doc)
        {
            try
            {
                _context.Doctor.Add(new Doctor {Email = doc.Email, FirstName = doc.FirstName, LastName = doc.LastName});
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool ModifyDoctor(DoctorRequest doc)
        {
            var res = (from d in _context.Doctor where d.IdDoctor == doc.IdDoctor select d).FirstOrDefault();
            if (res == null) return false;

            res.FirstName = doc.FirstName;
            res.Email = doc.Email;
            res.LastName = doc.LastName;

            _context.SaveChanges();

            return true;
        }

        public bool DeleteDoctor(int id)
        {
            var res = (from doc in _context.Doctor where doc.IdDoctor == id select doc).FirstOrDefault();
            if (res == null)
            {
                return false;
            }

            _context.Doctor.Remove(res);
            _context.SaveChanges();
            return true;
        }
    }
}