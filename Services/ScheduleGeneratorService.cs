using LSMS.data_access;
using LSMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LSMS.Services
{
    public class ScheduleGeneratorService : IScheduleGeneratorService
    {
        private readonly ApplicationDbContext dbContext;

        public ScheduleGeneratorService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private int MaxLectureSlots = 25;
        private List<List<int>> _currentHalls;
        private int _numberOfHalls;

        public void GenerateScheduleBacktrack(List<Lecture> lectures, List<Hall> halls)
        {
            _numberOfHalls = halls.Count;
            _currentHalls = new List<List<int>>();
            for (int i = 0; i < _numberOfHalls; i++)
            {
                _currentHalls.Add(new List<int>());
            }

            GenerateLectureSchedule(lectures, 0);

            ApplyChangesToDatabase(lectures, halls);
        }

        private bool GenerateLectureSchedule(List<Lecture> lectures, int index)
        {
            if (index == lectures.Count)
            {
                return true;
            }

            List<int> grid = Enumerable.Repeat(0, MaxLectureSlots).ToList();

            foreach (var student in lectures[index].students)
            {
                foreach (var timeInfo in student.lectures)
                {
                    if (timeInfo.lectureNum != -1)
                    {
                        grid[timeInfo.lectureNum] = _numberOfHalls;
                    }
                }
            }

            var professor = lectures[index].professor;

            foreach (var timeInfo in professor.lectures)
            {
                if (timeInfo.lectureNum != -1)
                {
                    grid[timeInfo.lectureNum] = _numberOfHalls;
                }
            }

            foreach (var hall in _currentHalls)
            {
                foreach (var timeInfo in hall)
                {
                    grid[timeInfo]++;
                }
            }

            for (int i = 0; i < MaxLectureSlots; i++)
            {
                if (grid[i] <= _numberOfHalls)
                {
                    lectures[index].lectureNum = i;
                    _currentHalls[grid[i]].Add(i);
                    lectures[index].hallId = grid[i];

                    if (GenerateLectureSchedule(lectures, index + 1))
                    {
                        return true;
                    }

                    _currentHalls[grid[i]].Remove(i);
                    lectures[index].hallId = 0;
                    lectures[index].lectureNum = -1;
                }
            }

            return false;
        }
        private void ApplyChangesToDatabase(List<Lecture> modifiedLectures, List<Hall> modifiedRooms)
        {
            foreach (var lecture in modifiedLectures)
            {
                // Update the lecture in the database
                var existingLecture = dbContext.Lectures.Find(lecture.id);
                if (existingLecture != null)
                {
                    existingLecture.lectureNum = lecture.lectureNum;
                    // Update other lecture properties as needed
                }
            }

            // Save changes to the database
            dbContext.SaveChanges();
        }
    }
}
