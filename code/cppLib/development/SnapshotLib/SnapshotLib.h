/*
* Developed by Michael-Evangelos Diamantis Aug-2024
* for SAE Athens CMN6302 - Major.
* Source: https://github.com/MichaelEvangelosD/cmn6302_majorSAE
*/
#pragma once

#include "pch.h"

#ifdef SNAPSHOT_EXPORTS
#define SNAPSHOT_API __declspec(dllexport)
#else
#define SNAPSHOT_API __declspec(dllimport)
#endif

/*
All the available library return codes for error handling and validation.
*/
enum SnapshotReturnCodes
{
	OperationSuccessful = 0,
	OperationFailed = 1,
	CouldNotOpenFile = 2,
	ReadNotSuccessful = 3,
	DirectoryNotFound = 76,
	FileNotFound = 404,
};

#pragma region Save Path
/*
setSavePath "C"-like library exposure
*/
extern "C" SNAPSHOT_API short setSavePath(const char* _savePath);
/*
getSavePath "C"-like library exposure
*/
extern "C" SNAPSHOT_API const char* getSavePath();
#pragma endregion

#pragma region SMRI Handling
/*
getSmri "C"-like library exposure
*/
extern "C" SNAPSHOT_API unsigned int getSmri();
/*
decreaseSmri "C"-like library exposure
*/
extern "C" SNAPSHOT_API void decreaseSmri();
/*
deleteSmriData "C"-like library exposure
*/
extern "C" SNAPSHOT_API short deleteSmriData(unsigned int _smri);
/*
getCurrentSmri "C"-like library exposure
*/
extern "C" SNAPSHOT_API unsigned int getCurrentSmri();
#pragma endregion


#pragma region Data Caching and Packing
/*
cacheData "C"-like library exposure
*/
extern "C" SNAPSHOT_API short cacheData(unsigned int _smri, int _dataSize, unsigned char* _data, int _refsSize, int* _refSmris);
/*
getData "C"-like library exposure
*/
extern "C" SNAPSHOT_API unsigned char* getData(unsigned int _smri, int* _size);
/*
getRefSmris "C"-like library exposure
*/
extern "C" SNAPSHOT_API int* getRefSmris(unsigned int _parentSmri, int* _size);
/*
packData "C"-like library exposure
*/
extern "C" SNAPSHOT_API short packData();
#pragma endregion

#pragma region Load from File
/*
setLoadFileName "C"-like library exposure
*/
extern "C" SNAPSHOT_API short setLoadFileName(const char* _loadFileName);
/*
getLoadFileName "C"-like library exposure
*/
extern "C" SNAPSHOT_API const char* getLoadFileName();
/*
unpackData "C"-like library exposure
*/
extern "C" SNAPSHOT_API short unpackData();
#pragma endregion

#pragma region DLL Cleanup
/*
resetSmri "C"-like library exposure
*/
extern "C" SNAPSHOT_API short resetSmri();
/*
resetCache "C"-like library exposure
*/
extern "C" SNAPSHOT_API short resetCache();
#pragma endregion

