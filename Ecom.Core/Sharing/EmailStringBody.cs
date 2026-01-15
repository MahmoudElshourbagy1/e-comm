using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Core.Sharing
{
    public class EmailStringBody
    {
        public static string send(string email, string token, string component, string massage)
        {
            string encodeToken = Uri.EscapeDataString(token);

            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{
            font-family: Arial, Helvetica, sans-serif;
            background-color: #f4f6f9;
            margin: 0;
            padding: 0;
        }}

        .email-container {{
            max-width: 600px;
            margin: 40px auto;
            background-color: #ffffff;
            border-radius: 12px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.08);
            overflow: hidden;
        }}

        .email-header {{
            background: linear-gradient(135deg, #0d6efd, #0dcaf0);
            color: #ffffff;
            padding: 25px;
            text-align: center;
        }}

        .email-header h1 {{
            margin: 0;
            font-size: 24px;
        }}

        .email-body {{
            padding: 30px;
            color: #333333;
            line-height: 1.6;
        }}

        .email-body p {{
            font-size: 16px;
            margin-bottom: 20px;
        }}

        .button {{
            display: inline-block;
            padding: 14px 30px;
            background-color: #0d6efd;
            color: #ffffff !important;
            text-decoration: none;
            border-radius: 8px;
            font-size: 16px;
            font-weight: bold;
            transition: background-color 0.3s ease;
        }}

        .button:hover {{
            background-color: #0b5ed7;
        }}

        .email-footer {{
            background-color: #f8f9fa;
            text-align: center;
            padding: 15px;
            font-size: 13px;
            color: #6c757d;
        }}

        .divider {{
            height: 1px;
            background-color: #e9ecef;
            margin: 25px 0;
        }}
    </style>
</head>

<body>

    <div class='email-container'>
        <div class='email-header'>
            <h1>E-Store Notification</h1>
        </div>

        <div class='email-body'>
            <p><strong>{massage}</strong></p>

            <div class='divider'></div>

            <p>
                Please click the button below to continue the process.
            </p>

            <a class='button'
               href='http://localhost:4200/Account/{component}?email={email}&code={encodeToken}'>
                {massage}
            </a>
        </div>

        <div class='email-footer'>
            © {DateTime.Now.Year} E-Store. All rights reserved.
        </div>
    </div>

</body>
</html>
";
        }
    }
}

