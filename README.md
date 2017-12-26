## Nimbus
A filesharing webapp built on ASP .NET Core MVC. I'm writing this for my own
personal use, and as a learning experience.

### What Works
The basics. No access control whatsoever. Just uploads, downloads, folder
creation, and deletion. I advise against exposing this to the open internet.

### Download
Get the latest release archive from [here](https://github.com/sildein/Nimbus/releases/latest).

### Configuration
See the file `config.ini` in the release archive. You'll notice the following
options:
- `Title` - Your server's name. This name is  used in the `<title>` tag, and
	the menu bar at the top of the page.
- `Prefix` - Where your files are actually kept. You need to create this folder
and two subfolders named `Files` and `Temp`. I recommend using forward slashes.
- `Port` - The port used to access your server. This one shouldn't need to be
explained. 

### Planned for Version 2
- Multiuser support. This isn't meant to serve a bunch of people, so I'll most
likely forgo a full-blown database in favor of a plaintext config file
containing usernames and password hashes.
- An admin panel that lets you configure user accounts and anonymous access.

### License
```
####################### Sildein's Free Software License #######################

Copyright (c) 2017-present Jesse Jones.

This license exists because I believe the mainstream free software licenses all
suck. The GNU GPL is a communist tumor, and the more permissive licenses allow
developers to get ramrodded.

You may use this license for your own software, but changing it is disallowed.
-------------------------------------------------------------------------------



Usage:

You may use this software for any imaginable purpose, without restriction. This
includes commercial purposes, with or without financial gain.
-------------------------------------------------------------------------------

Disclaimer:

This software is provided to you free of charge. As such, it does not carry any
kind of warranty whatsoever, express or implied.

You are solely responsible for any undesirable things that happen as a
result of your use of this software, including (but not limited to) data loss,
hardware/software damage, acts of God, hurt feelings, and theft by a meth
junkie. Under no circumstances shall the developer(s) be held liable for any
damages.
-------------------------------------------------------------------------------

Redistribution:

You may redistribute this application in source or binary form, as long as the
following conditions are met:

The origin of the software must not be misrepresented. You may not claim to
have written the original software, or claim to be affiliated with the project.

Unmodified copies of the software must include this license and all copyright
notices must be left intact.

You may not redistribute this software for the sole purpose of throwing ads
around it. URL shorteners are especially unacceptable.

The host must not restrict users' access to the software through any means.
This includes paywalls, registration walls, and freemium limitations such as
throttling and wait times. This does not apply to private cloud storage
accounts.
-------------------------------------------------------------------------------

Derivative Works:

You may create derivative software packages based on this software and release
them under any license you wish, as long as the following conditions are met:

The derivative work must include a notice somewhere easily accessible to the
user that acknowledges the use of this software, such as an "about" section or
the license document.

Rebranding and/or renaming the software does not constitute modification. There
must be moderate changes made to the application logic, or it must be used as
part of a larger project. If neither of those statements apply, all of the
statements under "Redistribution" remain in effect.
-------------------------------------------------------------------------------

Shrink-wrap Clause:

Executing, modifying, or redistributing this software constitutes agreement to
the terms of this license. If you do not agree, do not use the software.
```
