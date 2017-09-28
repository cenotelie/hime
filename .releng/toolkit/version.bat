@ECHO OFF

FINDSTR "AssemblyVersion" .releng\VersionInfo.cs > match
FOR /F tokens^=2-4^ delims^=^". %%a IN (match) DO ECHO %%a.%%b.%%c
DEL match