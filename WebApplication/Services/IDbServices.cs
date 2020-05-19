using WebApplication.Models;

namespace WebApplication.Services
{
    public interface IDbServices
    {
        public DoctorResponse GetDoctor(int id);
        public bool AddDoctor(DoctorResponse doc);
        public bool ModifyDoctor(DoctorRequest doc);
        public bool DeleteDoctor(int id);
    }
}