#!/bin/sh

echo "Build.sh begin"

#UNITY程序的路径#
UNITY_PATH="/Applications/Unity/Unity.app/Contents/MacOS/Unity"

#游戏程序路径#
PROJECT_PATH=$PWD
echo "PROJECT_PATH" + ${PROJECT_PATH}

platform=$1

if [ ${platform} == "android" ]; then
	#在Unity中构建apk#
	$UNITY_PATH -projectPath $PROJECT_PATH -executeMethod JoyyouBuild.AndroidBuild -quit -batchmode -logFile build.log
else
	#导出xcode工程
	$UNITY_PATH -projectPath $PROJECT_PATH -executeMethod JoyyouBuild.IosBuild -quit -batchmode -logFile build.log
fi




sleep 5
echo "Build.sh end"
