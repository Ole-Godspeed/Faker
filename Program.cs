using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Bogus;
using Bogus.DataSets;

namespace faker
{    
    class Program
    {
        static void Main(string[] args)
        {   
            String language = args[1];
            int linesNumber = Convert.ToInt32(args[2]); //int.Parse(args[2]);
            Random rnd = new Random();
            String fake_data;
            if (language =="ru_RU") {language = "ru";}
            var faker = new Faker(language);

            var datafile = File.ReadAllText("data.json");
                                              
                
                if (language == "ru")
                {
                    for (int i = 0; i < linesNumber; i++){
                        fake_data = "";

                        if (rnd.Next(2) == 1)
                        {
                            fake_data += faker.Name.FullName(Name.Gender.Female) + " " + JObject.Parse(datafile)["male_middle_name"][rnd.Next(51)] + "; ";
                        }
                        else
                        {
                            fake_data += faker.Name.FullName(Name.Gender.Male) + " " + JObject.Parse(datafile)["male_middle_name"][rnd.Next(52)] + "; ";
                        }
                        
                        fake_data += faker.Address.ZipCode() + ", " + JObject.Parse(datafile)["country"][0] + ", " + faker.Address.City() + ", " +
                        faker.Address.StreetName() + ", " + JObject.Parse(datafile)["prefix"][0] + rnd.Next(1, 333).ToString();
                        if (rnd.Next(5) != 0)
                        {
                            fake_data += JObject.Parse(datafile)["prefix"][1] + rnd.Next(1, 5).ToString();
                        }

                        fake_data += ", " + faker.Address.SecondaryAddress() + ", " + faker.Phone.PhoneNumberFormat() + ";";                        
                        Console.WriteLine(fake_data);
                    }
                }

                if  (language == "en_US")
                {
                    for (int i = 0; i < linesNumber; i++){
                       fake_data = "";

                       if (rnd.Next(2) == 1)
                       {
                            fake_data += faker.Name.FullName(Name.Gender.Female) + "; ";                             
                       }
                       else
                       {
                           fake_data += faker.Name.FullName(Name.Gender.Male) + "; "; 
                       }
                        fake_data += faker.Address.ZipCode() + ", " + JObject.Parse(datafile)["country"][1] + ", " + faker.Address.City() + ", " +
                        faker.Address.StreetAddress() + " " + faker.Address.StreetSuffix() + ", " + faker.Address.SecondaryAddress() + "; " +
                        faker.Phone.PhoneNumberFormat() + ";";

                        Console.WriteLine(fake_data);
                    }    
                }           
            // Console.ReadLine();
        }
    }
}
