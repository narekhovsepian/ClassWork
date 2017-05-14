using System;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CafeMaps
{
    class Program
    {
        static void Main(string[] args)
        {

            int k = 0;
            Cafe.ReadCafeData();
            CafeIntro();
            for (;;)
            {


                try
                {

                    int x = int.Parse(Console.ReadLine());

                    if (x == 1)
                    {
                        int i = 1;
                        Cafe.SortCafe(Cafe.cafes);

                        foreach (Cafe cafe in Cafe.cafes) { Console.WriteLine(i + " " + cafe); i += 1; }
                        Console.WriteLine("\n");

                    }
                    else if (x == 2)
                    {
                        Console.WriteLine("Enter your coordinates: ");
                        Console.Write("Enter the width: ");
                        double x1 = double.Parse(Console.ReadLine());
                        Console.Write("Enter longitude: ");
                        double y = double.Parse(Console.ReadLine());
                        Console.Write("Please enter what you want to search nearby cafes radius: ");
                        int radius = int.Parse(Console.ReadLine());

                        GeoCoordinate MyCordinate = new GeoCoordinate(x1, y);
                        int n = -1;
                        for (int i = 0; i < Cafe.cafes.Count; i++)
                        {
                            if (MyCordinate.GetDistanceTo(Cafe.cafes[i].CafesCoordinate) <= radius) { n += 1; Console.WriteLine(Cafe.cafes[i]); }

                        }
                        if (n < 0)
                        { Console.WriteLine("Cafe not found:"); }



                    }
                    else if (x == 3)
                    {
                        Console.WriteLine("If you want to search the café named, click the button F1.\n"
                            + "Simply, you want to choose a cafe menu press F2 button" + "\n"
                            + "Or press F3 if you want to see all list");
                        ConsoleKeyInfo cki = Console.ReadKey();
                        if (cki.Key == ConsoleKey.F1)
                        {
                            Console.Write("Please enter the name of the cafe: ");
                            int j;
                            string name = Console.ReadLine().ToLower().ToUpper();
                            bool isDigit = name.Length == name.Where(c => char.IsDigit(c)).Count();
                            if (isDigit == false)
                            {
                                Console.WriteLine("\n");
                                for (j = 0; j < name.Length; j++)
                                {
                                    Console.WriteLine(Cafe.cafes.Find(cc => cc.Name[j] == name[j]));
                                    break;
                                }
                                foreach (WorkingDaysAndTimes day in Cafe.cafes[j].WorkTime)
                                {
                                    Console.WriteLine(day);
                                }
                                Console.WriteLine("\n\n");
                            }
                            else Console.WriteLine("Please do not enter a number");
                        }

                        if (cki.Key == ConsoleKey.F2)
                        {
                            Console.WriteLine("Please Type a cafe that you want: ");
                            int i = 1;
                            foreach (Cafe cafe in Cafe.cafes)
                            {
                                Console.WriteLine(i + " - " + cafe.Name);
                                i++;
                            }
                            Console.WriteLine("\n");
                            int j = int.Parse(Console.ReadLine());
                            Console.WriteLine("\n");
                            Console.WriteLine(Cafe.cafes[--j]);
                            foreach (WorkingDaysAndTimes day in Cafe.cafes[j].WorkTime)
                            {
                                Console.WriteLine(day);
                            }
                            Console.WriteLine("\n\n");
                        }

                        if (cki.Key == ConsoleKey.F3)
                        {
                            foreach (Cafe cafe in Cafe.cafes)
                            {
                                Console.WriteLine(cafe);
                                foreach (WorkingDaysAndTimes day in cafe.WorkTime)
                                {
                                    Console.WriteLine("     " + day);
                                }
                            }
                        }
                    }
                    else if (x == 4)
                    {
                        Console.Write("Please enter the name of the cafe: ");
                        int j, n = -1;
                        string str = Console.ReadLine().ToLower().ToUpper();
                        bool IsDigit = str.Length == str.Where(c => char.IsDigit(c)).Count();
                        if (IsDigit == false)
                        {
                            for (j = 0; j < str.Length; j++)
                            {

                                Console.WriteLine(Cafe.cafes.Find(cc => cc.Name[j] == str[j]));


                                break;


                            }

                            foreach (WorkingDaysAndTimes day in Cafe.cafes[j].WorkTime)
                            {
                                Console.WriteLine("     " + day);
                            }
                        }
                        else if (IsDigit == true) { Console.WriteLine("Please do not enter a number"); }
                    }

                    else if (x == 5)
                    {
                        if (!User.IsLoggedin)
                        {

                            Console.Write("Please enter your UserName: ");
                            string username = Console.ReadLine();

                            Console.Write("Please enter your Password: ");
                            SecureStringPass.GetPass();
                            Console.WriteLine();

                            User user = new User(username, SecureStringPass.pass);
                            user.LogIn();
                            if (!User.IsLoggedin)
                            {
                                Console.WriteLine("Please try again.");
                            }
                            else
                            {
                                Console.WriteLine("You are logged in succesfully.");
                            }
                            CafeIntro();
                        }
                        else
                        {
                            int id = ++Cafe.lastID;
                            Console.Write("Please enter cafe name: ");
                            string name = Console.ReadLine();
                            Console.Write("Please enter address: ");
                            string address = Console.ReadLine();
                            Console.Write("Please enter coordinate X: ");
                            double cordinateX = double.Parse(Console.ReadLine());
                            Console.Write("Please enter coordinate Y: ");
                            double cordinateY = double.Parse(Console.ReadLine());
                            Cafe c = new Cafe(id, name, address, cordinateX, cordinateY);
                            for (;;)
                            {
                                Console.Write("Add new day of work ? (Y/N): ");
                                string isadd = Console.ReadLine();
                                if (isadd == "Y")
                                {
                                    Console.Write("Please enter day: ");
                                    string day = Console.ReadLine();
                                    Console.Write("Please enter time from: ");
                                    string from = Console.ReadLine();
                                    Console.Write("Please enter time to: ");
                                    string to = Console.ReadLine();
                                    c.WorkTime.Add(new WorkingDaysAndTimes(day, from, to));
                                }
                                else
                                    break;
                            }
                            Cafe.cafes.Add(c);
                            Cafe.UpdateJsonData();
                            CafeIntro();
                        }

                    }


                    else if (x == 6)
                    {
                        if (!User.IsLoggedin)
                            return;
                        int i = 1;
                        foreach (Cafe cafe in Cafe.cafes)
                        {
                            Console.WriteLine(i + " - " + cafe.Name);
                            i++;
                        }
                        Console.Write("Please type a cafe id that you want to delete: ");
                        int j = int.Parse(Console.ReadLine());
                        Cafe.cafes.RemoveAt(--j);
                        Cafe.UpdateJsonData();
                        CafeIntro();
                    }

                    else if (x == 7)
                    {
                        if (!User.IsLoggedin)
                            return;
                        Console.WriteLine("Please type a cafe that you want to rate: ");
                        int i = 1;
                        foreach (Cafe cafe in Cafe.cafes)
                        {
                            Console.WriteLine(i + " - " + cafe.Name);
                            i++;
                        }
                        int j = int.Parse(Console.ReadLine());
                        Cafe c = Cafe.cafes[--j];
                        Console.Write("Please enter your rate (From 1 to 5): ");
                        int rate = Convert.ToInt32(Console.ReadLine());
                        if (rate > 5) rate = 5;
                        Console.Write("Please enter your comment: ");
                        string comment = Console.ReadLine();
                        c.Review.Add(new Review(++j, User.Usern, rate, comment));
                        Cafe.UpdateJsonData();
                        CafeIntro();
                    }

                    else if (x == 8)
                    {
                        User.LogOut();
                    }
                    else if (x == 9)
                    {
                        Cafe.SortCafe(Cafe.cafes);
                        int i = 1;
                        foreach (Cafe cafe in Cafe.cafes) { Console.WriteLine(i + " " + cafe); i += 1; }
                        Console.WriteLine("Select what you want cafe");

                        int y = int.Parse(Console.ReadLine());
                        foreach (Review item in Cafe.cafes[y - 1].Review) { Console.WriteLine(item); }


                    }



                    else break;



                    k += 1;
                    if (x == 1 || x == 9 || x == 3 || k >= 2) { CafeIntro(); }
                }

                catch (Exception x) { MessageBox.Show(x.Message.ToString(), x.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); Console.Write("You can not enter a string expression\n"); }


            }
        }
        private static void CafeIntro()
        {
            string path;
            if (User.IsLoggedin)
                path = @"Data\CafeUserIntro.txt";
            else
                path = @"Data\CafeIntro.txt";
            using (StreamReader reader = new StreamReader(path, true))
            {
                while (!reader.EndOfStream) { Console.WriteLine(reader.ReadLine()); }
            }
        }
    }
}
