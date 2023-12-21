using LSMS.Models;

namespace LSMS.Services
{
    public interface IScheduleGeneratorService
    {
        void GenerateScheduleBacktrack(List<Lecture> lectures, List<Hall> halls);
    }
}
