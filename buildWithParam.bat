@echo off

set UnityPath=C:\"Program Files"\Unity5.6.7\Editor\Unity.exe
set filename=%date:~0,4%%date:~5,2%%date:~8,2%%time:~0,2%%time:~3,2%%time:~6,2%
set "filename=%filename: =0%"

REM 1,打AssetBudle资源;    2,生成APK;    3,Copy资源到目标路径

REM 接受Jenkins传入的参数
set parameter=%1
echo parameter:%parameter%

REM 将Jenkins传入的参数写到本地,Editor下读取在jenkins中设置的版本号和渠道名称
echo %parameter%>parameter.txt


echo Start Build AssetBundle
REM BuildAssetBundle
REM %UnityPath% -projectPath C:\Resource\Project\Download\BehaviourTree\Tsiu_AIToolkit_Project -quit -batchmode -executeMethod AssetBundleFramework.AssetBundleBuilder.BuildAssetBundleCommond -logFile %filename%_buildAssetbundle.log
echo Build AssetBundle Finished


echo Start Build APK
REM Build APK
%UnityPath% -projectPath C:\Resource\Project\Download\BehaviourTree\Tsiu_AIToolkit_Project -quit -batchmode -executeMethod JenkinsAdapter.Build -logFile %filename%_buildApk.log
REM %1 -projectPath %2 -quit -batchmode -executeMethod APKBuild.Build -logFile build.log


if not %errorlevel%==0 ( goto fail ) else ( goto success )
 
:success
echo Build APK OK
REM Copr Dir
goto end
 
:fail
echo Build APK Fail
goto end
 
:end

pause