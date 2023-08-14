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
        private void SimulateTopUp(UserDTO user, decimal amount)
{
    
    // Console.WriteLine($"Simulating top-up of {amount:C} for user {user.Username}");
}
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

    
    public void AddCourse()
    {
           Console.Write("Enter course name: ");
        string courseName = Console.ReadLine();
        Console.Write("Enter course price: ");
        decimal coursePrice = decimal.Parse(Console.ReadLine());

        Course newCourse = new Course
        {
            Name = courseName,
            Price = coursePrice
        };

        courses.Add(newCourse);
        SaveCoursesToFile();
        Console.WriteLine("Course added successfully");

    }

    public void UpdateCourse()
    {
        Console.Write("Enter the name of the course to update: ");
        string oldCourseName = Console.ReadLine();
        Console.Write("Enter new course name: ");
        string newCourseName = Console.ReadLine();
        Console.Write("Enter new course price: ");
        decimal newCoursePrice = decimal.Parse(Console.ReadLine());


        Course updatedCourse = new Course
        {
            Name = newCourseName,
            Price = newCoursePrice
        };

        Course courseToUpdate = courses.Find(course => course.Name == oldCourseName);
        if (courseToUpdate != null)
        {
            courseToUpdate.Name = updatedCourse.Name;
            courseToUpdate.Price = updatedCourse.Price;
            SaveCoursesToFile();
        }
        Console.WriteLine("Course updated successfully");
    }

    public void DeleteCourse()
    {
        Console.Write("Enter the name of the course to delete: ");
        string courseName = Console.ReadLine();

        // courses.DeleteCourse(courseName);
        Console.WriteLine("Course deleted successfully");
        courses.RemoveAll(course => course.Name == courseName);
        SaveCoursesToFile();
    }

    public List<Course> LoadCoursesFromFile()
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

    public void SaveCoursesToFile()
    {
        List<string> lines = new List<string>();
        foreach (Course course in courses)
        {
            string courseLine = $"{course.Name},{course.Price}";
            lines.Add(courseLine);
        }

        File.WriteAllLines(CoursesFilePath, lines);
    }
     public void PurchaseCourse(UserDTO user, Courses courses)
    {
        Console.WriteLine("Available Courses:");
    List<Course> availableCourses = courses.GetCourses();

    if (availableCourses.Count == 0)
    {
        Console.WriteLine("No courses available for purchase.");
        return;
    }

    Console.WriteLine("Select a course to purchase:");
    for (int i = 0; i < availableCourses.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {availableCourses[i].Name} - {availableCourses[i].Price:C}");
    }

    Console.Write("Enter the number of the course you want to purchase: ");
    int courseNumber = int.Parse(Console.ReadLine());

    if (courseNumber >= 1 && courseNumber <= availableCourses.Count)
    {
        Course selectedCourse = availableCourses[courseNumber - 1];

       
        decimal topUpAmount = selectedCourse.Price;
        SimulateTopUp(user, topUpAmount);

        Analytics analytics = new Analytics();
        analytics.SavePurchaseToAnalytics(user, selectedCourse);

        Console.WriteLine($"Congratulations! You have purchased the course: {selectedCourse.Name} for {selectedCourse.Price:C}");
    }
    else
    {
        Console.WriteLine("Invalid course selection.");
    }
    }
}
