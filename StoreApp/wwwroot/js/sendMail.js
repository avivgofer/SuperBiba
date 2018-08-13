console.log('im here ');
$.getScript('https://smtpjs.com/v2/smtp.js', function () {
    var sendMail = $('#sendMail').click(function () {
        console.log('trying to send mail');
        Email.send("elikos1@gmail.com",
            "elikos1@gmail.com",
            "חסין זאת דוגמא",
            "התוכן של הדוגמא",
            "smtp.gmail.com",
            { token: "63cb3a19-2684-44fa-b76f-debf422d8b00" });
    });
});