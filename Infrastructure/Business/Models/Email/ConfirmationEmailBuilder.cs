﻿using Notification.Helper;
using Schoolman.Student.Core.Application.Models;
using System.IO;
using System.Text;

namespace Schoolman.Student.Core.Application.Interfaces
{
    public class ConfirmationEmailBuilder : EmailBuilder, IConfirmationEmailBuilder
    {
        private string url;

        public IConfirmationEmailBuilder ConfirmationUrl(string url)
        {
            this.url = url;
            return this;
        }

        public override Email Build()
        {
            SetEmailBody();
            return email;
        }

        private void SetEmailBody()
        {
            var htmlMessage = new StringBuilder                              // ~
                               (File.ReadAllText(emailTemplatePath), capacity: 8000) 
                               .AddConfirmationUrl(url)
                               .AddUserName(email.To);

            email.Body = htmlMessage.ToString();
        }
    }
}
