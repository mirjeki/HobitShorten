using HobitShorten.Models.Dtos;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HobitShorten.Models
{
    public class URLShortener
    {
        ILiteCollection<URLDto> urls;

        private string GenerateUniqueToken()
        {
            bool uniqueTokenGenerated = false;
            string token = "";

            while (!uniqueTokenGenerated)
            {
                Random rng = new Random();
                var availableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

                int tokenLength = rng.Next(2, 6);

                for (int i = 0; i < tokenLength; i++)
                {
                    token += availableChars.Substring(rng.Next(0, availableChars.Length), 1);
                }

                if (!urls.Exists(e => e.Token == token))
                {
                    uniqueTokenGenerated = true;
                }
            }

            return token;
        }

        public string ShortenURL(string url, string host)
        {
            using (var database = new LiteDatabase("Data/Urls.db"))
            {
                urls = database.GetCollection<URLDto>();

                if (urls.Exists(u => u.URL == url))
                {
                    //return existing ShortURL from the DB where one has already been made for that URL
                    return urls.FindOne(f => f.URL == url).ShortURL;
                }
                else
                {
                    //generate new URL row in the DB and return the generated ShortURL
                    URLDto newURL = new URLDto();
                    var token = GenerateUniqueToken();

                    newURL.Token = token;
                    newURL.URL = url;
                    newURL.ShortURL = host + "/" + token;

                    urls.Insert(newURL);
                    return newURL.ShortURL;
                }
            }
        }
    }
}