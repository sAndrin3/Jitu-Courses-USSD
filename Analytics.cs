using System;
using System.Collections.Generic;
using System.IO;
public class Analytics
{
    public void SavePurchaseToAnalytics(UserDTO user, Course course)
{
    string purchaseData = $"{course.Name},{user.Username},{course.Price}";
    File.AppendAllText("Data/analytics.txt", purchaseData + Environment.NewLine);
}
    public void ViewAnalytics()
{
    Console.WriteLine("Analytics:");
    Console.WriteLine("1. List of Courses and Students");
    Console.WriteLine("2. Back to Dashboard");
    Console.Write("Enter your choice: ");
    int choice = int.Parse(Console.ReadLine());

    switch (choice)
    {
        case 1:
            DisplayCourseAndStudentAnalytics();
            break;

        case 3:
            Console.WriteLine("Returning to Dashboard...");
            break;

        default:
            Console.WriteLine("Invalid choice");
            break;
    }
}

public void DisplayCourseAndStudentAnalytics()
{
    Console.WriteLine("List of Courses and Students with Purchased Courses:");
    List<string[]> analyticsData = ReadAnalyticsData();
    Dictionary<string, List<string>> studentCourses = new Dictionary<string, List<string>>();

    foreach (string[] fields in analyticsData)
    {
        if (fields.Length == 3)
        {
            string courseName = fields[0];
            string student = fields[1];

            if (!studentCourses.ContainsKey(student))
            {
                studentCourses[student] = new List<string>();
            }

            studentCourses[student].Add(courseName);
        }
    }

   foreach (var kvp in studentCourses)
    {
        string student = kvp.Key;
        List<string> purchasedCourses = kvp.Value;

        Console.WriteLine($"Student: {student}");
        Console.WriteLine("Courses Purchased:");
        foreach (string course in purchasedCourses)
        {
            Console.WriteLine($"- {course}");
        }
        Console.WriteLine(); 
    }
}


 private List<string[]> ReadAnalyticsData()
{
    List<string[]> analyticsData = new List<string[]>();
    string[] lines = File.ReadAllLines("Data/analytics.txt");

    foreach (string line in lines)
    {
        Console.WriteLine($"Read Line: {line}");
        string[] fields = line.Split(',');
        analyticsData.Add(fields);
    }

    return analyticsData;
}
    
}