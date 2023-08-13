using System;
using System.Collections.Generic;
using System.IO;

public class Course
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class Courses
{
    private List<Course> courses;
    private const string CoursesFilePath = "Data/courses.txt";

    public Courses()
    {
        
        courses = LoadCoursesFromFile();
    }

    public List<Course> GetCourses()
    {
        return courses;
    }

    public void AddCourse(Course course)
    {
        courses.Add(course);
        SaveCoursesToFile();
    }

    public void UpdateCourse(string oldCourseName, Course newCourse)
    {
        Course courseToUpdate = courses.Find(course => course.Name == oldCourseName);
        if (courseToUpdate != null)
        {
            courseToUpdate.Name = newCourse.Name;
            courseToUpdate.Price = newCourse.Price;
            SaveCoursesToFile();
        }
    }

    public void DeleteCourse(string courseName)
    {
        courses.RemoveAll(course => course.Name == courseName);
        SaveCoursesToFile();
    }

    private List<Course> LoadCoursesFromFile()
    {
        List<Course> loadedCourses = new List<Course>();

        if (File.Exists(CoursesFilePath))
        {
            string[] lines = File.ReadAllLines(CoursesFilePath);
            foreach (string line in lines)
            {
                string[] courseFields = line.Split(',');
                if (courseFields.Length == 2)
                {
                    loadedCourses.Add(new Course
                    {
                        Name = courseFields[0],
                        Price = decimal.Parse(courseFields[1])
                    });
                }
            }
        }

        return loadedCourses;
    }

    private void SaveCoursesToFile()
    {
        List<string> lines = new List<string>();
        foreach (Course course in courses)
        {
            string courseLine = $"{course.Name},{course.Price}";
            lines.Add(courseLine);
        }

        File.WriteAllLines(CoursesFilePath, lines);
    }
}
