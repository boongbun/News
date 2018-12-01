using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Mail;
using News.Models;
using News.Models.Entities;

namespace News.Common
{
    public class MailFunction
    {
        #region CommonMail
        private static string sendAccount;
        private static string sendPassword;
        private static int port;
        private static string hostMail;

        public static ResultModel<bool> SendMailToClient(MailModel parameters)
        {
            sendAccount = "boongbun1314@gmail.com";
            sendPassword = "hanoi1234";
            port = 587;
            hostMail = "smtp.gmail.com";
            var result =new ResultModel<bool>(){Data = false};
            try
            {
                var fromAddress = new MailAddress(sendAccount, "From HieuNT");
                var toAddress = new MailAddress(parameters.AddressClient, parameters.AddressClient);
                string fromPassword = sendPassword;
                string subject = parameters.SubjectClient;
                string body = parameters.BodyClient;
                var smtp = new SmtpClient
                {
                    Host = hostMail,
                    Port = port,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                
                if (parameters.DataFileBytes != null)
                {
                    //Get some binary data
                    byte[] data = parameters.DataFileBytes;

                    //save the data to a memory stream
                    MemoryStream ms = new MemoryStream(data);

                    //create the attachment from a stream. Be sure to name the data with a file and 
                    //media type that is respective of the data
                    var contentType =
                        new System.Net.Mime.ContentType
                        {
                            MediaType = System.Net.Mime.MediaTypeNames.Application.Octet,
                            Name = parameters.FileName
                        };
                   
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        Attachments = { new Attachment(ms, contentType) }
                    })
                    {
                        smtp.Send(message);
                    }
                }
                else
                {
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                    })
                    {
                        smtp.Send(message);
                    }
                }
                

                //    MailMessage mail = new MailMessage();
                //mail.From = new MailAddress(sendAccount);
                ////recipient address
                //mail.To.Add(parameters.AddressClient);
                //mail.Subject = parameters.SubjectClient;

                ////Formatted mail body
                //mail.IsBodyHtml = true;
                //string st = parameters.BodyClient;

                //mail.Body = st;

                //// The important part -- configuring the SMTP client
                //SmtpClient smtp = new SmtpClient();
                //smtp.Port = port;   // [1] You can try with 465 also, I always used 587 and got success
                //smtp.EnableSsl = true;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // [2] Added this
                //smtp.UseDefaultCredentials = false; // [3] Changed this
                //smtp.Credentials = new NetworkCredential(sendAccount, sendPassword);  // [4] Added this. Note, first parameter is NOT string.
                //smtp.Host = hostMail;

                //Check file is attachment


                //smtp.Send(mail);

                result = new ResultModel<bool>() { Data = true, IsError = false, Message = "Mail sent" };
            }
            catch (Exception e)
            {
                result = new ResultModel<bool>() { Data = false, IsError = true,Message = e.Message};
            }

            return result;
        }

        public List<FILE_INFO> ListFileDefault = new List<FILE_INFO>()
        {
            new FILE_INFO(){FILE_NAME = "excel",FILE_TYPE = "application/vnd.ms-excel"},
            new FILE_INFO(){FILE_NAME = "excel",FILE_TYPE = "application/vnd.ms-excel"},
            new FILE_INFO(){FILE_NAME = "excel",FILE_TYPE = "application/vnd.ms-excel"},
            new FILE_INFO(){FILE_NAME = "excel",FILE_TYPE = "application/vnd.ms-excel"},
            new FILE_INFO(){FILE_NAME = "excel",FILE_TYPE = "application/vnd.ms-excel"},
        };
        #endregion
    }
}