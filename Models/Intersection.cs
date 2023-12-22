namespace LSMS.Models
{
    public class Intersection
    {
        public string departmentId {  get; set; }
        public string lectureId {  get; set; }

        // studentdep != lecture.course.dep 
        /* intersection.add(new intersection (){
         * departmentId=studentdep,
         * lectureId=lectureId
         * });
         */
        // int cntdep =context.Intersections.where(d=>d.departmentId == dep ).count();
    }
}
