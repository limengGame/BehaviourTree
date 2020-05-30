@echo off  

echo 正在生成APK文件...  

REM C:\Program Files\Unity5.6.7\Editor\Unity.exe -projectPath C:\Resource\Project\Download\BehaviourTree\Tsiu_AIToolkit_Project -quit -batchmode -executeMethod JenkinsAdapter.Build -logFile build.log

C:
cd C:\Program Files\Unity5.6.7\Editor\Unity.exe -projectPath C:\Resource\Project\Download\BehaviourTree\Tsiu_AIToolkit_Project -quit -batchmode -executeMethod JenkinsAdapter.Build -logFile build.log

echo APK文件生成完毕!  
pause