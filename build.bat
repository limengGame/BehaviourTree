@echo off  

echo ��������APK�ļ�...  

REM C:\Program Files\Unity\Editor\Unity.exe -projectPath E:\UnityProject\Tsiu_AIToolkit_Project\Tsiu_AIToolkit_Project -quit -batchmode -executeMethod JenkinsAdapter.Build -logFile build.log

%1 -projectPath %2 -quit -batchmode -executeMethod APKBuild.Build -logFile build.log

echo APK�ļ��������!  
pause