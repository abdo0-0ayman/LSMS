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

        private int maxLectureSlots = 25;
        private List<List<int>> currentHalls;
        private int NumberOfHalls;
        public void GenerateScheduleBacktrack(List<Lecture> lectures, List<Hall> halls)
        {
            NumberOfHalls = halls.Count;
            currentHalls = new List<List<int>>();
            for (int i = 0; i < NumberOfHalls; i++)
            {
                currentHalls.Add(new List<int>());
            }
            GenerateLectureSchedule(lectures, 0);
            foreach (var lecture in lectures)
            {
                int hallindex = lecture.hallId!=null ? int.Parse(lecture.hallId) : 0; // Default value if HallId is null
                lecture.hallId = halls[hallindex].id;
            }
            ApplyChangesToDatabase(lectures);
        }

        private bool GenerateLectureSchedule(List<Lecture> lectures, int index)
        {
            if (index == lectures.Count)
            {
                return true;
            }

            List<int> capacityState = Enumerable.Repeat(0, maxLectureSlots).ToList();

            foreach (var student in lectures[index].students)
            {
                foreach (var slot in student.lectures)
                {
                    if (slot.lectureNum != -1)
                    {
                        capacityState[slot.lectureNum] = NumberOfHalls;
                    }
                }
            }

            var professor = lectures[index].professor;

            foreach (var slot in professor.lectures)
            {
                if (slot.lectureNum != -1)
                {
                    capacityState[slot.lectureNum] = NumberOfHalls;
                }
            }

            foreach (var hall in currentHalls)
            {
                foreach (var slot in hall)
                {
                    capacityState[slot]++;
                }
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int cnt = j * 5 + i;
                    if (capacityState[cnt] < NumberOfHalls)
                    {
                        lectures[index].lectureNum = cnt;
                        currentHalls[capacityState[cnt]].Add(cnt);
                        lectures[index].hallId = capacityState[cnt].ToString();
                        if (GenerateLectureSchedule(lectures, index + 1))
                        {
                            return true;
                        }
                        currentHalls[capacityState[cnt]].Remove(cnt);
                        lectures[index].hallId = null;
                        lectures[index].lectureNum = -1;
                    }
                }
            }

            return false;
        }
        private void ApplyChangesToDatabase(List<Lecture> modifiedLectures)
        {
            foreach (var lecture in modifiedLectures)
            {
                // Update the lecture in the database
                var existingLecture = dbContext.Lectures.Find(lecture.id);
                if (existingLecture != null)
                {
                    existingLecture.lectureNum = lecture.lectureNum;
                    existingLecture.hallId = lecture.hallId;
                    // Update other lecture properties as needed
                }
            }

            // Save changes to the database
            dbContext.SaveChanges();
        }
    }
}
