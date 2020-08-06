using System;
using System.IO;
using System.Text;

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
           string language = args[1];
           if ((args.Length < 3) || (args.Length > 4))
           {
               Console.WriteLine("Wrong number of args = " + (args.Length - 1));
           }
           else
           {
               int linesNumber = Convert.ToInt32(args[2]);
 
               if (args[1] == "ru_RU")
               {
                   PrintRU(linesNumber);
               }
 
               if (args[1] == "en_US")
               {
                   PrintUS(linesNumber);
               }

               if (args[1] == "be_BY")
               {
                   PrintBY(linesNumber);
               }
           }    
            // string language = "en_US";                                                                                                  // я полагаю что тебе нужно будет сделать метод PrintRU_US_BY типо String с return fake_data; потом сделать типо счетчика который будет срабатывать на 3 параметр (проблема с числами меньше 1) и делать метод MakeErrors относительно счетчика а не for'a и выводить строку с измененной датой
            // string fake_data = "Omar Howell; 26695-4040, USA, Gusttown, 82586 Layla Parkway Shores, Suite 210; 443-901-7769";

            // Console.WriteLine(fake_data);                                                                   
            // Console.WriteLine(MakeErrors(fake_data, 10, rnd, language));
            // Console.ReadLine();
        }

        static void PrintRU(int linesNumber)
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
        static void PrintUS(int linesNumber)
        {
            String fake_data;
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

                Console.WriteLine(fake_data);
            }
        }
        static void PrintBY(int linesNumber)
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
            var fake_data_builder = new StringBuilder(fake_data);
            for (int i = 0; i < num_error; i++)
            {
                int rand = rnd.Next(fake_data.Length);
                fake_data_builder = fake_data_builder.Remove(rand, 1);

                if (language == "en_US")
                {
                    switch (rnd.Next(3))
                    {
                        case 0:
                            fake_data_builder = fake_data_builder.Insert(rand, (char)rnd.Next(48, 57));     // numbers
                            break;
                        case 1:
                            fake_data_builder = fake_data_builder.Insert(rand, (char)rnd.Next(97, 122));    // low en
                            break;
                        case 2:
                            fake_data_builder = fake_data_builder.Insert(rand, (char)rnd.Next(65, 90));     // high en
                            break;
                    }
                }
                else if (language == "ru")
                {
                    switch (rnd.Next(2))
                    {
                        case 0:
                            fake_data_builder = fake_data_builder.Insert(rand, (char)rnd.Next(48, 57));     // numbers
                            break;
                        case 1:
                            fake_data_builder = fake_data_builder.Insert(rand, (char)rnd.Next(192, 255));   // all ru
                            break;
                    }        
                }
                else
                {
                    switch (rnd.Next(2))
                    {
                        case 0:
                            fake_data_builder = fake_data_builder.Insert(rand, (char)rnd.Next(48, 57));     // numbers
                            break;
                        case 1:
                            fake_data_builder = fake_data_builder.Insert(rand, (char)rnd.Next(192, 255));   // all ru 
                            break;
                                                                                                            // !!! нужно доделать бел язык 
                    }        
                }
                
                
            }
                return fake_data_builder.ToString();
        }
    }
}
 
// бел алфовит я думаю нужно засунуть в базу данных или в файлик и из него достать потому как если это делать через ascii то ну нах или даже все языки так сделать в одной строке
//А а	Б б	В в	Г г	Д д	(Дж дж)	(Дз дз)	Е е
//Ё ё	Ж ж	З з	І і	Й й	К к	Л л	М м
//Н н	О о	П п	Р р	С с	Т т	У у	Ў ў
//Ф ф	Х х	Ц ц	Ч ч	Ш ш	Ы ы	Ь ь	Э э
//Ю ю	Я я	

// + я коментил потому как не могу воспользоваться консолью.