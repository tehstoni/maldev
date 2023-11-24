@ECHO OFF

cl.exe /nologo /Ox /MT /W0 /GS- /DNDEBUG /Tctrivia.cpp /link /OUT:trivia.exe /SUBSYSTEM:WINDOWS /MACHINE:x64