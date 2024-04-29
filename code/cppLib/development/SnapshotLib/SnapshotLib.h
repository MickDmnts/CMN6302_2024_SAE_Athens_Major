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

/*
@TODO: Summary
*/
struct DataContainer
{
	int _DataSize;
	unsigned int _Smri;
	unsigned char* _Data;

	template<class T>
	void pack(T& pack) {
		std::vector<unsigned char> temp;
		for (int i = 0; i < _DataSize; ++i) {
			temp.push_back(_Data[i]);
		}

		pack(_Smri, _DataSize, temp);
	}
};

extern "C" SNAPSHOT_API short setSavePath(const char* _savePath);
extern "C" SNAPSHOT_API const char* getSavePath();
extern "C" SNAPSHOT_API unsigned int getSmri();
extern "C" SNAPSHOT_API void decreaseSmri();
extern "C" SNAPSHOT_API unsigned int getCurrentSmri();
extern "C" SNAPSHOT_API short resetSmri();
extern "C" SNAPSHOT_API short cacheData(DataContainer _model);
extern "C" SNAPSHOT_API unsigned char* getData(unsigned int _smri, int* _size);
extern "C" SNAPSHOT_API short packData();
extern "C" SNAPSHOT_API short resetCache();