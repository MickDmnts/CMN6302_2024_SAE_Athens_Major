/*
* Developed by Michael-Evangelos Diamantis Aug-2024
* for SAE Athens CMN6302 - Major.
* Source: https://github.com/MichaelEvangelosD/cmn6302_majorSAE
*/
#pragma once

#include <unordered_map>
#include <iostream>
#include <fstream>
#include <stdexcept>

#ifdef SNAPSHOT_EXPORTS
#define SNAPSHOT_API __declspec(dllexport)
#else
#define SNAPSHOT_API __declspec(dllimport)
#endif

//Save path
extern "C" SNAPSHOT_API short setSavePath(const char* _savePath);
extern "C" SNAPSHOT_API const char* getSavePath();

//SMRI handling
extern "C" SNAPSHOT_API unsigned int getSmri();
extern "C" SNAPSHOT_API void decreaseSmri();
extern "C" SNAPSHOT_API unsigned int getCurrentSmri();

//Data caching and packing
extern "C" SNAPSHOT_API short cacheData(unsigned int _smri, int _dataSize, unsigned char* _data, int* _refSmris, int _refsSize);
extern "C" SNAPSHOT_API unsigned char* getData(unsigned int _smri, int* _size);
extern "C" SNAPSHOT_API short packData();

//Load from file
extern "C" SNAPSHOT_API short setLoadFileName(const char* _loadFileName);
extern "C" SNAPSHOT_API const char* getLoadFileName();
extern "C" SNAPSHOT_API short unpackData();

//DLL Cleanup
extern "C" SNAPSHOT_API short resetSmri();
extern "C" SNAPSHOT_API short resetCache();