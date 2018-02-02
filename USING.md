## Using Nimbus
This document provides information about Nimbus. Hopefully, any questions you
may have about its setup are answered here.

### Installation
1. Make sure you have .NET Core. You can get it from [here](https://www.microsoft.com/net/download/).
I'm going to assume that if you have a need for this application, you're
proficient enough with your operating system to set this up.

2. Download the latest release archine from [here](https://github.com/sildein/Nimbus/releases/latest).
Unpack this archive anywhere you wish. Then create a directory somewhere that
you wish to use to store your users' files. Then, inside this directory, create
two more named 'Files' and 'Temp'.

3. In your preferred command-line shell, navigate to wherever you unpacked
Nimbus, and run `dotnet Nimbus.dll`. If you're on Windows, you can hold shift
and right-click in Explorer to open a CMD shell in that folder.

4. Open your preferred web browser and navigate to `$address:32768/Admin` where
`$address` is your server's hostname or IP address. You may need to create a
firewall rule for this port. The default password is simply 'admin'. Now, you
may configure your server to use the correct prefix, add users, et cetera. And
of course, change that admin password.

5. Now navigate to that same address/port but leave out the '/Admin' portion,
so that you navigate to the root. This will bring you to the user login. Enter
the credentials for a user you created in the admin panel and log in. You're
now ready to use your server.

6. (Optional) If you intend to serve files over the wide open internet, I
**strongly** advise that you use a proper web server like Apache or Nginx as a
reverse proxy and put this app behind it. And secure it with SSL. The built-in
Kestrel server is meant more for development and testing. And without SSL your
passwords, files and cookies can be read by third parties in-transit.

### Configuration Files
Nimbus uses three files for its configuration. You do not need to edit these
files by hand (nor should you) but information about them is provided anyway.

- `config.ini` - The file that determines where your users' files are kept,
what port your server is bound to, and your server's title.
- `users.ini` - Contains username/SHA512 password hash pairs delimited by a
colon. Storing passwords in plaintext is bad, m'kay.
- `admin.ini` - The sole purpose of this file is to store the admin password's
hash.

### Additional Information
- When you create a user, their home folder is automatically created. But when
you delete one, their files are preserved. It's up to you to move that folder
elsewhere or delete it.
- If a file upload is cancelled or otherwise doesn't complete, there will be
leftovers in the `Temp` folder. It's up to you to clean them out.
