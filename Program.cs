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
               Console.WriteLine($"args[1] should be a number, not {args[2]}");
           }
           else
           {   
                int num_error;
                if (args.Length == 3)
                {
                    num_error = 0;
                }
                else
                {
                    num_error = Convert.ToInt32(args[3]);
                }
                string language = args[1];
                int linesNumber = Convert.ToInt32(args[2]);
 
                if (args[1] == "ru_RU")
                {
                    PrintRU(linesNumber, language);
                }

                if (args[1] == "en_US")
                {
                    PrintUS(linesNumber, language, num_error);
                }

                if (args[1] == "be_BY")
                {
                    PrintBY(linesNumber, language);
                }
           }                                                                      
        }

        static void PrintRU(int linesNumber, string language)
        {
            String fake_data;
            Random rnd = new Random();
            var faker = new Faker("ru");
            dynamic datafile = JObject.Parse(File.ReadAllText("ru_RUdata.json"));
            for (int i = 0; i < linesNumber; i++)
            {
                fake_data = "";

                if (rnd.Next(2) == 1)
                {
                    fake_data += faker.Name.FullName(Name.Gender.Female) + " " + datafile.male_middle_name[rnd.Next(51)] + "; ";
                }
                else
                {
                    fake_data += faker.Name.FullName(Name.Gender.Male) + " " + datafile.male_middle_name[rnd.Next(52)] + "; ";
                }

                fake_data += faker.Address.ZipCode() + ", " + datafile.country + ", " + faker.Address.City() + ", " +
                faker.Address.StreetName() + ", " + datafile.prefix[0] + rnd.Next(1, 333).ToString();
                if (rnd.Next(5) != 0)
                {
                    fake_data += datafile.prefix[1] + rnd.Next(1, 5).ToString();
                }

                fake_data += ", " + faker.Address.SecondaryAddress() + "; " + faker.Phone.PhoneNumberFormat() + "; ";
                Console.WriteLine(fake_data);
            }
        }
        static void PrintUS(int linesNumber, string language, int num_error)
        {
            String fake_data;
            Random rnd = new Random();
            var faker = new Faker(language);
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

                if (num_error == 0)
                {
                    Console.WriteLine(fake_data);
                }
                else
                {
                    Console.WriteLine(MakeErrors(fake_data, num_error, rnd, language));
                }
            }
        }
        static void PrintBY(int linesNumber, string language)
        {
            String fake_data;
            Random rnd = new Random();
            dynamic datafile = JObject.Parse(File.ReadAllText("be_BYdata.json"));
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

                Console.WriteLine(fake_data);   
            }
        }
        static String MakeErrors(string fake_data, int num_error , Random rnd, string language)
        {
            int switchOption;
            var fake_data_builder = new StringBuilder(fake_data);
            for (int i = 0; i < num_error; i++)
            {
                int rand = rnd.Next(fake_data_builder.Length);

                if (fake_data_builder.Length < 45) {switchOption = rnd.Next(2);}
                else if (fake_data_builder.Length > 90) {switchOption = rnd.Next(1,3);}
                else {switchOption = rnd.Next(3);}
                switch (switchOption)
                {
                    case 0:     // adding symbol
                        switch (rnd.Next(3))
                        {
                            case 0:
                                fake_data_builder = fake_data_builder.Insert(rand, (char)rnd.Next(48, 57));     // number
                                break;
                            case 1:
                                fake_data_builder = fake_data_builder.Insert(rand, (char)rnd.Next(97, 122));    // lowcase char
                                break;
                            case 2:
                                fake_data_builder = fake_data_builder.Insert(rand, (char)rnd.Next(65, 90));     // highcase char
                                break;
                        }
                        break;
                    case 1: // swapping 2 adjacent symbols
                        if (rand > fake_data_builder.Length - 2) {rand -= 2;}
                        fake_data_builder = fake_data_builder.Insert(rand, Reverse(fake_data_builder.ToString(rand, 2)));
                        fake_data_builder = fake_data_builder.Remove(rand+2, 2);  
                        break; 
                    case 2: // deleting symbol
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
    }
}
 
//А а	Б б	В в	Г г	Д д	(Дж дж)	(Дз дз)	Е е
//Ё ё	Ж ж	З з	І і	Й й	К к	Л л	М м
//Н н	О о	П п	Р р	С с	Т т	У у	Ў ў
//Ф ф	Х х	Ц ц	Ч ч	Ш ш	Ы ы	Ь ь	Э э
//Ю ю	Я я	