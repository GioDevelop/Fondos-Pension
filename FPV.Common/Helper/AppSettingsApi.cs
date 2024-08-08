using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPV.Common.Helper
{
    public class AppSettingsApi
    {
        private static AppSettingsApi settings;
        public static AppSettingsApi Settings { get => settings; set => settings = value; }
        public FPVApi FPVApi { get; set; }
        public Mailing Mailing { get; set; }
        public EstratecMasivSMS EstratecMasivSMS { get; set; }

    }

    public class FPVApi
    {
        public string BaseUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CustomerId { get; set; }
        public RoutesEmail RoutesEmail { get; set; }
    }
    public class Mailing
    {
        public string BaseApi { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MailConfiguration { get; set; }
        public string ArchivoSmtp { get; set; }
        public string EmailTemplateId { get; set; }
        public RoutesEmail RoutesEmail { get; set; }
    }
    public class Root
    {
        public AppSettingsApi AppSettings { get; set; }
    }

    public class RoutesEmail
    {
        public string SendPendingMails { get; set; }
    }

    public class EstratecMasivSMS
    {
        public string url { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public object customdata { get; set; }
        public bool isPremium { get; set; }
        public bool isFlash { get; set; }
        public bool isLongmessage { get; set; }
        public bool isRandomRoute { get; set; }
        public object scheduledDate { get; set; }
        public object shortUrlConfig { get; set; }
    }
}
