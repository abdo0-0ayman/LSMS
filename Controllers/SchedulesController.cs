using LSMS.data_access;
using LSMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace LSMS.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        

        public SchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<Lecture> lectures;
        public IActionResult GenerateSchedule()
        {
            // Call your schedule generation logic
            init();
            lectures = _context.Lectures.Include(s => s.Students).Include(s => s.Professor).ToList();
            foreach (var lecture in lectures)
            {
                lecture.lectureNum = -1;
                _context.Entry(lecture).State = EntityState.Modified;
            }

            GenerateLectureSchedule(0);

            foreach(var lecture in lectures)
            {
                _context.Entry(lecture).State = EntityState.Modified;
            }
            _context.SaveChanges();

            return RedirectToAction("profile", "Admins"); // Redirect to the home page or another appropriate view
        }

        bool found= false;
        int num_of_halls=2;
        int num_of_lecures=6;
        List<List<int>> cur_halls;
        public void init()
        {
            cur_halls = new List<List<int>>();
            for(int i=0;i<num_of_halls;i++)
            {
                cur_halls.Add(new List<int> { });
            }
        }
        private void GenerateLectureSchedule(int idx)
        {

            if(idx==lectures.Count())found = true;
            if (found) return;
            List<int> grid = new List<int>();
            for (int i = 0; i < 20; i++)
            {
                grid.Add(0);
            }
            foreach (var st in lectures[idx].Students)
            {
                foreach (var ti in st.Lectures)
                {
                    if (ti.lectureNum != -1)
                        grid[ti.lectureNum] = num_of_halls;
                }
            }
            var prof = lectures[idx].Professor;
            foreach (var ti in prof.Lectures)
            {
                if (ti.lectureNum != -1)
                    grid[ti.lectureNum] = num_of_halls;
            }
            foreach (var ha in cur_halls)
            {
                foreach(var ti in ha)
                {
                    grid[ti]++;
                }
            }
            for(int i=0;i<20;i++)
            {
                if (grid[i]<num_of_halls)
                {
                    lectures[idx].lectureNum = i;
                    cur_halls[grid[i]].Add(i);

                    GenerateLectureSchedule(idx + 1);
                    if (found) return;
                    cur_halls[grid[i]].Remove(i);
                    lectures[idx].lectureNum = -1;
                }
            }
           
        }
    }
}
