namespace LMS.Data_
{
    public static class HtmlGenerator
    {
        public static string GenerateSuccessHtml() =>
        """
        <html>
            <body style="margin:0; padding:0; font-family:Arial, sans-serif; background:#f4f6f9;">
                <div style="display:flex; justify-content:center; align-items:center; height:100vh;">
                    <div style="background:#fff; padding:40px; border-radius:12px; box-shadow:0 10px 25px rgba(0,0,0,0.1); text-align:center; width:350px;">
                        <div style="font-size:50px;"> </div>
                        <h1 style="color:#28a745; margin-bottom:10px;">Payment Successful</h1>
                        <p style="color:#555;">Your subscription is now active.</p>
                    </div>
                </div>
            </body>
        </html>
        """;

        public static string GenerateFailedHtml() =>
        """
        <html>
            <body style="margin:0; padding:0; font-family:Arial, sans-serif; background:#f4f6f9;">
                <div style="display:flex; justify-content:center; align-items:center; height:100vh;">
                    <div style="background:#fff; padding:40px; border-radius:12px; box-shadow:0 10px 25px rgba(0,0,0,0.1); text-align:center; width:350px;">
                        <div style="font-size:50px;">❌</div>
                        <h1 style="color:#dc3545; margin-bottom:10px;">Payment Failed</h1>
                        <p style="color:#555;">Something went wrong. Please try again.</p>
                    </div>
                </div>
            </body>
        </html>
        """;

        public static string GenerateSecurityHtml() =>
        """
        <html>
            <body style="margin:0; padding:0; font-family:Arial, sans-serif; background:#f4f6f9;">
                <div style="display:flex; justify-content:center; align-items:center; height:100vh;">
                    <div style="background:#fff; padding:40px; border-radius:12px; box-shadow:0 10px 25px rgba(0,0,0,0.1); text-align:center; width:350px;">
                        <div style="font-size:50px;">⚠️</div>
                        <h1 style="color:#ffc107; margin-bottom:10px;">Security Error</h1>
                        <p style="color:#555;">Invalid or unauthorized request.</p>
                    </div>
                </div>
            </body>
        </html>
        """;
    }
}