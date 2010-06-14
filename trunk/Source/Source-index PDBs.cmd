@echo off
REM This file requires Debugging Tools for Windows, Perl (e.g., ActivePerl), and svn.exe (e.g., CollabNet/Tigris)
call "c:\Program Files\Debugging Tools for Windows (x86)\srcsrv\ssindex.cmd" /SYSTEM=SVN /SYMBOLS=..\Binaries\35.NoRx /Debug
call "c:\Program Files\Debugging Tools for Windows (x86)\srcsrv\ssindex.cmd" /SYSTEM=SVN /SYMBOLS=..\Binaries\35.Rx /Debug
call "c:\Program Files\Debugging Tools for Windows (x86)\srcsrv\ssindex.cmd" /SYSTEM=SVN /SYMBOLS=..\Binaries\40.NoRx /Debug
call "c:\Program Files\Debugging Tools for Windows (x86)\srcsrv\ssindex.cmd" /SYSTEM=SVN /SYMBOLS=..\Binaries\40.Rx /Debug
call "c:\Program Files\Debugging Tools for Windows (x86)\srcsrv\ssindex.cmd" /SYSTEM=SVN /SYMBOLS=..\Binaries\SL3.NoRx /Debug
call "c:\Program Files\Debugging Tools for Windows (x86)\srcsrv\ssindex.cmd" /SYSTEM=SVN /SYMBOLS=..\Binaries\SL3.Rx /Debug
call "c:\Program Files\Debugging Tools for Windows (x86)\srcsrv\ssindex.cmd" /SYSTEM=SVN /SYMBOLS=..\Binaries\SL4.NoRx /Debug
call "c:\Program Files\Debugging Tools for Windows (x86)\srcsrv\ssindex.cmd" /SYSTEM=SVN /SYMBOLS=..\Binaries\SL4.Rx /Debug
pause