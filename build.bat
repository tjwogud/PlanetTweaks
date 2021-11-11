cd G:\불얼모드
G:
copy /Y %1%2.dll %2\%2\%2.dll
del %2.zip
cd %2
zip -r G:\불얼모드\%2.zip %2
copy /Y G:\불얼모드\%2.zip G:\UnityModManager\모드\%2.zip