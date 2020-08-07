using System;
using System.IO;
using System.Text;
using System.Linq;

using Newtonsoft.Json.Linq;
using Bogus;
using Bogus.DataSets;
using System.Text.RegularExpressions;

namespace faker
{
    class Program
    {
        static void Main(string[] args)
        {
           if ((args.Length < 3) || (args.Length > 4))
           {
               Console.WriteLine("wrong number of args = " + (args.Length - 1) + ", expected 2 or 3");
           }
           else if (args[1] != "ru_RU" && args[1] != "en_US" && args[1] != "be_BY")
           {
               Console.WriteLine($"args[1] should be ru_RU, en_US or be_BY, not {args[1]}");
           }
           else if (!(Regex.IsMatch(args[2], @"^\d+$")))
           {
               Console.WriteLine($"args[2] should be an integer, not {args[2]}");
           }
           else if (args.Length == 4 && !(Regex.IsMatch(args[3], @"^\d+\.?\d+$")) && !(Regex.IsMatch(args[3], @"^\d+\.?$")))
           {
               Console.WriteLine($"args[3] should be an number, not {args[3]}");
           }
           else
           {   
                double errorsNumber;
                if (args.Length == 3)
                {
                    errorsNumber = 0;
                }
                else
                {
                    errorsNumber = Convert.ToDouble(args[3]);
                }
                string language = args[1];
                int linesNumber = Convert.ToInt32(args[2]);
 
                if (args[1] == "ru_RU")
                {
                    PrintRU(linesNumber, errorsNumber);
                }

                if (args[1] == "en_US")
                {
                    PrintUS(linesNumber, errorsNumber);
                }

                if (args[1] == "be_BY")
                {
                    PrintBY(linesNumber, errorsNumber);
                }
           }                                                                      
        }
        static void PrintRU(int linesNumber, double errorsNumber)
        {
            String fake_data;
            dynamic datafile = JObject.Parse(File.ReadAllText("ru_RUdata.json"));
            dynamic charfile = JObject.Parse(File.ReadAllText("RU_BYcharset.json"));
            Random rnd = new Random();
            var faker = new Faker("ru");
            for (int i = 0; i < linesNumber; i++)
            {
                fake_data = "";

                if (rnd.Next(2) == 1)
                {
                    fake_data += faker.Name.FullName(Name.Gender.Female) + " " + datafile.female_middle_name[rnd.Next(datafile.female_middle_name.Count)] + "; ";
                }
                else
                {
                    fake_data += faker.Name.FullName(Name.Gender.Male) + " " + datafile.male_middle_name[rnd.Next(datafile.male_middle_name.Count)] + "; ";
                }

                fake_data += faker.Address.ZipCode() + ", " + datafile.country + ", " + faker.Address.City() + ", " +
                faker.Address.StreetName() + ", " + datafile.prefix[0] + rnd.Next(1, 333).ToString();
                if (rnd.Next(5) != 0)
                {
                    fake_data += datafile.prefix[1] + rnd.Next(1, 5).ToString();
                }

                fake_data += ", " + faker.Address.SecondaryAddress() + "; " + faker.Phone.PhoneNumberFormat() + "; ";
                
                if (errorsNumber == 0)
                {
                    Console.WriteLine(fake_data);
                }
                else
                {   
                    Console.WriteLine(MakeErrors(fake_data, errorsNumber, rnd, charfile.ru_RU));
                }
            }
        }
        static void PrintUS(int linesNumber, double errorsNumber)
        {
            String fake_data;
            dynamic charfile = JObject.Parse(File.ReadAllText("RU_BYcharset.json"));
            Random rnd = new Random();
            var faker = new Faker("en_US");
            for (int i = 0; i < linesNumber; i++)
            {
                fake_data = "";

                if (rnd.Next(2) == 1)
                {
                    fake_data += faker.Name.FullName(Name.Gender.Female) + "; ";
                }
                else
                {
                    fake_data += faker.Name.FullName(Name.Gender.Male) + "; ";
                }
                fake_data += faker.Address.ZipCode() + ", " + "USA, " + faker.Address.City() + ", " +
                faker.Address.StreetAddress() + " " + faker.Address.StreetSuffix() + ", " + faker.Address.SecondaryAddress() + "; " +
                faker.Phone.PhoneNumberFormat() + ";";

                if (errorsNumber == 0)
                {
                    Console.WriteLine(fake_data);
                }
                else
                {   
                    Console.WriteLine(MakeErrors(fake_data, errorsNumber, rnd, charfile.en_US));
                }
            }
        }
        static void PrintBY(int linesNumber, double errorsNumber)
        {
            String fake_data;
            Random rnd = new Random();
            dynamic datafile = JObject.Parse(File.ReadAllText("be_BYdata.json"));
            dynamic charfile = JObject.Parse(File.ReadAllText("RU_BYcharset.json"));
            for (int i = 0; i < linesNumber; i++)
            {
                fake_data = "";

                if (rnd.Next(2) == 1)
                {
                    fake_data += datafile.male_first_name[rnd.Next(82)] + " " + datafile.male_second_name[rnd.Next(88)] +
                    " " + datafile.male_middle_name[rnd.Next(55)] + "; ";
                }
                else
                {
                    fake_data += datafile.female_first_name[rnd.Next(63)] + " " + datafile.female_second_name[rnd.Next(81)] +
                    " " + datafile.female_middle_name[rnd.Next(52)] + "; ";
                }
                fake_data += datafile.zip_codes[rnd.Next(125)] + ", " + datafile.country + ", " +
                datafile.city[rnd.Next(111)] + ", " + datafile.street_address[rnd.Next(342)] + ", " + 
                datafile.prefix[0] + rnd.Next(1, 333).ToString() + ", " + datafile.prefix[2] +
                rnd.Next(1, 600).ToString() + "; " + datafile.phonenumber_codes[rnd.Next(4)] + rnd.Next(1000000, 9999999).ToString() + "; ";

                if (errorsNumber == 0)
                {
                    Console.WriteLine(fake_data);
                }
                else
                {
                    Console.WriteLine(MakeErrors(fake_data, errorsNumber, rnd, charfile.be_BY));                    
                }   
            }
        }
        static String MakeErrors(string fake_data, double errorsNumber , Random rnd, dynamic charfile)
        {
            int switchOption;
            var fake_data_builder = new StringBuilder(fake_data);
            for (int i = 0; i < RoundChance(errorsNumber, rnd); i++)
            {
                int rand = rnd.Next(fake_data_builder.Length);

                if (fake_data_builder.Length < 65) {switchOption = rnd.Next(2);}
                else if (fake_data_builder.Length > 90) {switchOption = rnd.Next(1,3);}
                else {switchOption = rnd.Next(3);}
                switch (switchOption)
                {
                    case 0:
                        fake_data_builder = fake_data_builder.Insert(rand, charfile[rnd.Next(charfile.Count)]);
                        break;
                    case 1:
                        if (rand > fake_data_builder.Length - 2) {rand -= 2;}
                        fake_data_builder = fake_data_builder.Insert(rand, Reverse(fake_data_builder.ToString(rand, 2)));
                        fake_data_builder = fake_data_builder.Remove(rand+2, 2);  
                        break; 
                    case 2:
                        fake_data_builder = fake_data_builder.Remove(rand, 1);     
                        break;                           
                }                
            }
            return fake_data_builder.ToString();
        }
        public static string Reverse( string s )
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse( charArray );
            return new string( charArray );
        }

        public static int RoundChance( double number, Random rnd)
            {
                int p = (int)((number - (int)number) * 100);
                if (p > (int)rnd.Next(100))
                {
                    return (int)number + 1;
                }
                else
                {
                    return (int)number;
                }
            }
    }
}