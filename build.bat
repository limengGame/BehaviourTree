@echo off  

echo 正在生成APK文件...  

REM C:\Program Files\Unity\Editor\Unity.exe -projectPath E:\UnityProject\Tsiu_AIToolkit_Project\Tsiu_AIToolkit_Project -quit -batchmode -executeMethod JenkinsAdapter.Build -logFile build.log

%1 -projectPath %2 -quit -batchmode -executeMethod APKBuild.Build -logFile build.log

echo APK文件生成完毕!  
pause