@echo off
REM This file requires Debugging Tools for Windows, Perl (e.g., ActivePerl), and svn.exe (e.g., CollabNet/Tigris)
call "c:\Program Files\Debugging Tools for Windows (x86)\srcsrv\ssindex.cmd" /SYSTEM=SVN /SYMBOLS=35.NoRx\bin\Release /Debug
call "c:\Program Files\Debugging Tools for Windows (x86)\srcsrv\ssindex.cmd" /SYSTEM=SVN /SYMBOLS=35.Rx\bin\Release /Debug
call "c:\Program Files\Debugging Tools for Windows (x86)\srcsrv\ssindex.cmd" /SYSTEM=SVN /SYMBOLS=40.NoRx\bin\Release /Debug
call "c:\Program Files\Debugging Tools for Windows (x86)\srcsrv\ssindex.cmd" /SYSTEM=SVN /SYMBOLS=SL3.NoRx\bin\Release /Debug
call "c:\Program Files\Debugging Tools for Windows (x86)\srcsrv\ssindex.cmd" /SYSTEM=SVN /SYMBOLS=SL3.Rx\bin\Release /Debug
pause